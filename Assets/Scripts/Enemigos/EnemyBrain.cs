/**
 * =============<< ********* >>=============
 * Author       : Guillermo KÖSTER
 * Email        : guillermo.fullstack@gmail.com
 * Organization : 
 * Created On   :
 * Title        : EnemyBrain
 * Description  : Cerebro de enemigo con estados de Idle, Patrol, Alert, Chase, Search y Attack.
 * Copyright (c) 2025 Guillermo KÖSTER.
 * =============<< ********* >>=============
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
    {
        Idle,
        Patrol,
        Alert,
        Chase,
        Search,
        Attack,
        Dead
    }
public class EnemyBrain : MonoBehaviour
{
    Coroutine seeCoroutine;
    public Transform player;
    [SerializeField] State currentState = State.Idle;
    EnemyMotion motion;
    EnemySensor sensor;    
    Vector3 initialPosition;
    Vector3 initialPointToSee;
    // float attackRange = 1.5f;
    float timeLastAttack = 0f;
    // float attackCooldown = 2f;
    Queue<Vector3> searchPoints = new Queue<Vector3>();
    NavMeshAgent agent;
    [SerializeField] EnemyData enemyData;
    Animator anim;
    EnemyEventAnimation animEvent;
    CapsuleCollider capsuleCol;

    // gameObject sin collider para marcar puntos de búsqueda
    //[SerializeField] GameObject refe;
    EnemyAttack enemyAttack;
    void Start()
    {
        capsuleCol = GetComponent<CapsuleCollider>();
        animEvent = GetComponentInChildren<EnemyEventAnimation>();
        anim = GetComponentInChildren<Animator>();
        motion = GetComponent<EnemyMotion>();
        sensor = GetComponent<EnemySensor>();
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        initialPosition = transform.position;
        initialPointToSee = transform.forward * 100f + transform.position;
        motion.Initialize(enemyData,agent);
        sensor.Initialize(enemyData, player);
        enemyAttack.Initialize(enemyData, motion, player);
    }

    void Update()
    {
        if(currentState == State.Dead)
        {
            return;
        }
        switch(currentState)
        {
            case State.Idle:
                anim.SetBool("IsMoving", false);
                UpdateIdle();
                break;
            case State.Patrol:
                anim.SetBool("IsMoving", true);
                UpdatePatrol();
                break;
            case State.Alert:
                UpdateAlert();
                break;
            case State.Chase:
                anim.SetBool("IsMoving", true);
                UpdateChase();
                break;
            case State.Search:
                anim.SetBool("IsMoving", true);
                UpdateSearch();
                break;
            case State.Attack:
                anim.SetBool("IsMoving", false);
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle(){
        motion.Stop();
        if(seeCoroutine == null)
        {
            seeCoroutine = StartCoroutine(See(45f,initialPointToSee));
        }
        if (sensor.canSeeTarget())
        {
            Debug.Log("Target detected, switching to Chase state.");
            changeState( State.Chase);
            return;
        }
        if(sensor.canHearTarget(player))
        {
            Debug.Log("Target detected, switching to Alert state.");
            changeState( State.Alert);
        }
    }
    void UpdatePatrol(){
        // Implementación del comportamiento de patrulla
    }
    void UpdateAlert(){
        if (sensor.canSeeTarget())
        {
            Debug.Log("Target detected, switching to Chase state.");
            changeState( State.Chase);
        }
        motion.RotateTo(player.position);
    }
    void UpdateChase(){
        motion.RotateTo(player.position);
        if (!sensor.canSeeTarget())
        {
            Debug.Log("Lost sight and sound of target, switching to Search state.");
            changeState(State.Search);
            RefreshSearchPoints(player.transform);
            return;
        }
        AlertEventManager.SendAlert(player, transform);
        float sqrDistanceToTarget = (transform.position - player.position).sqrMagnitude;
        if(sqrDistanceToTarget > enemyData.attackRange*enemyData.attackRange)
        {
            motion.GoTo(player.position);
        }
        else changeState( State.Attack);
    }
    void UpdateAttack(){
        Vector3 direction = player.position - transform.position;
        motion.Stop();
        agent.ResetPath();
        if(!sensor.canSeeTarget() && !enemyAttack.inAttack)
        {
            changeState(State.Search);
            RefreshSearchPoints(player.transform);
            return;
        }
        AlertEventManager.SendAlert(player, transform);
        if(direction.normalized != transform.forward)
        {
            motion.RotateTo(player.position);
        }
        if(direction.sqrMagnitude > enemyData.attackRange*enemyData.attackRange) 
        {
            changeState( State.Chase);
            return;
        }
        if(Time.time - timeLastAttack >= enemyData.attackCooldown)
        {
            timeLastAttack = Time.time;
            enemyAttack.Attack(direction);
        }
    }
    void UpdateSearch(){
        if (sensor.canSeeTarget())
        {
            Debug.Log("Target detected, switching to Chase state.");
            changeState(State.Chase);
            return;
        }
        if(sensor.canHearTarget(player))
        {
            RefreshSearchPoints(player.transform);
            motion.GoTo(player.position);
            return;
        }
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if(searchPoints.Count > 0)
            {
                motion.GoTo(searchPoints.Dequeue());
                return;
            }
            changeState( State.Idle);
        }
    }

    IEnumerator See(float angulo, Vector3 referencePoint)
    {
        float delta = Mathf.Tan(angulo * Mathf.Deg2Rad);
        Vector3 pointToSee = referencePoint + Vector3.Cross(referencePoint,Vector3.up) * UnityEngine.Random.Range(-delta, delta);

        while(Vector3.Angle(transform.forward * 100f, pointToSee - transform.position) > .1f)
        {
            pointToSee.y = transform.position.y;
            motion.RotateTo(pointToSee);
            yield return null;
        }
        while(Vector3.Angle(transform.forward * 100f, referencePoint - transform.position) > .1f)
        {
            referencePoint.y = transform.position.y;
           motion.RotateTo(referencePoint);
           yield return null;
        }
        yield return new WaitForSeconds(UnityEngine.Random.Range(.5f, 2f));
        seeCoroutine = null;
    }

    void RefreshSearchPoints(Transform pointToSearch)
    {
        //refe.transform.position = pointToSearch.position;
        searchPoints.Clear();
        searchPoints.Enqueue(pointToSearch.position);
        searchPoints.Enqueue(pointToSearch.position+pointToSearch.forward*3f);
        searchPoints.Enqueue(pointToSearch.position+Vector3.right*3f);
        searchPoints.Enqueue(pointToSearch.position+Vector3.left*3f);
        searchPoints.Enqueue(pointToSearch.position+Vector3.forward*3f);
        searchPoints.Enqueue(pointToSearch.position+Vector3.back*3f);
        searchPoints.Enqueue(pointToSearch.position);
        searchPoints.Enqueue(initialPosition);
    }

    public void Muerto()
    {
        changeState(State.Dead);
        capsuleCol.enabled = false;
        if (animEvent != null)
        {
        animEvent.DisableWeaponHitbox();
        }
    }

    void changeState(State newState)
    {
        if(seeCoroutine != null)
        {
            StopCoroutine(seeCoroutine);
            seeCoroutine = null;
        }
        currentState = newState;
    }

    private void receiveAlert(Transform target, Transform sender)
    {
        if (currentState == State.Dead) return;
        if (sender == transform) return; // Ignorar alertas propias
        if (currentState == State.Chase || currentState == State.Attack) return; // No cambiar de estado si ya está persiguiendo o atacando
        if (sensor.canHearTarget(sender))
        {
            Debug.Log("Received alert from " + sender.name + ", switching to Search state.");
            changeState(State.Search);
            RefreshSearchPoints(target);
        }
    }
    private void OnEnable()
    {
        if (currentState == State.Dead) return;
        AlertEventManager.OnAlert -= receiveAlert;
        AlertEventManager.OnAlert += receiveAlert;
    }
    private void OnDisable()
    {
        AlertEventManager.OnAlert -= receiveAlert;
    }
}
