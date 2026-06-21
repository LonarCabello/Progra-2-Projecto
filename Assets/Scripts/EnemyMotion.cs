/**
 * =============<< ********* >>=============
 * Author       : Guillermo KÖSTER
 * Email        : guillermo.fullstack@gmail.com
 * Organization : 
 * Created On   : 
 * Title        : EnemyMotion
 * Description  : Controla el movimiento del enemigo, incluyendo caminar, rotar y atacar.
 * Copyright (c) 2025 Guillermo KÖSTER.
 * =============<< ********* >>=============
 */
using UnityEngine;
using UnityEngine.AI;

public class EnemyMotion : MonoBehaviour
{

    NavMeshAgent agent;
    Rigidbody rb;
    EnemyData enemyData;
    [SerializeField] GameObject a;
    public void Initialize(EnemyData data, NavMeshAgent agent)
    {
        this.agent = agent;
        rb = GetComponent<Rigidbody>();
        enemyData = data;
        agent.speed = enemyData.speed;
    }

    
    public void Stop()
    {
        rb.linearVelocity = Vector3.zero;
    }
    
    public void GoTo(Vector3 position, bool spectrumInAttack = false)
    {
        if(!spectrumInAttack && enemyData.enemyType == EnemyType.Spectrum)
        {
            RotateTo(position);
            Vector3 direction = (position - transform.position)/2f;
            Vector3 rigthSide = Vector3.Cross(Vector3.up, direction.normalized);
            float desvioLateral = Mathf.Cos(Time.time * enemyData.frecuencySerpenteo) * direction.magnitude;
            Vector3 newPosition = transform.position+direction + rigthSide * desvioLateral;
            agent.SetDestination(newPosition);
            a.transform.position = newPosition;
        }
        else
        {
            agent.SetDestination(position);
        }
    }
    public void RotateTo(Vector3 targetToSee)
    {
        Vector3 direction = targetToSee - transform.position;
        direction.y = 0; // Mantener la rotación solo en el plano horizontal
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 50f);
    }
    // public void Attack(Vector3 direction)
    // {
    //     Debug.Log("Attacking the target!");
    //     if (enemyData.enemyType == EnemyType.Ranged && rangedShoter != null)
    //     {
    //         rangedShoter.Shot(direction);
    //     }
    // }
}
