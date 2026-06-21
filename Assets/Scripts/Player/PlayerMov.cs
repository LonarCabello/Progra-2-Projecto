using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float speedRun = 6.5f; 
    [SerializeField] private float rotationSpeed = 10f;

    [Header ("Salto")]
    [SerializeField] private float jumpForce = 7f;
    //GroundCheck
    [SerializeField] Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] LayerMask groundLayer;

    private bool isGrounded = false;

    private bool isRunning = false;
    [Header("Block")]
    [SerializeField] private GameObject shieldObj;
    private bool isBlocking = false;


    [Header("Movimiento Camara")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Animator animator;

    private Vector3 movement;

    [Header("Sistema de Ataque")]
    [SerializeField] private float attackDuration = 1.15f;
    private bool isAttacking;

    [Header("Sistema de Combos")]
    private int comboStep = 0;
    private bool canQueueCombo = false;
    private bool comboQueue = false;    

    [Header("Cambio de Armas")]
    [SerializeField] private GameObject battleAxe;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject throwAxeObj;
    private GameObject nearbyWeapon;
    [SerializeField] private GameObject axePickUpPrefab;
    [SerializeField] private GameObject swordPickUpPrefab;
    [SerializeField] private Transform dropPosition;

    public enum WeaponType { BattleAxe, Sword, None}

    public WeaponType currentWeapon;


    [Header("Hachas Arrojadizas")]
    private bool isThrowingHold = false;
    [SerializeField] private GameObject throwAxePrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] float throwForce;
    [SerializeField] public int currentAxes = 3;
    [SerializeField] public int maxAxes = 3;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentWeapon = WeaponType.None;
    }

    void Update()
    {
        // -------------------------- INPUTS -----------------------------
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Running
        if (Input.GetKey(KeyCode.LeftShift) && !isThrowingHold && !isBlocking)
        {
            isRunning = true;
            animator.SetBool("isRunning", true);
        } 
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }

        //Block
        if (Input.GetMouseButton(1))
        {
            if (!isRunning && !isAttacking)
            {
                isBlocking = true;
                animator.SetBool("isBlocking", true);
            }
        }
        else
        {
            isBlocking = false;
            animator.SetBool("isBlocking", false);
        }

        //Recoger Arma
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickWeapon();
        }

        //Soltar Arma
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }

        //TakeDamage (Borrar) para probar el hit al jugador.
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealthManager healMan = GetComponent<HealthManager>();
            healMan.TakeDamage(20);
        }

        //Ataque Input
        if (isGrounded && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Atacando");
            if (currentWeapon == WeaponType.None) return;
            StartComboSistem();
        }

        //Hacha Arrojadiza Input
        if (Input.GetMouseButtonDown(2) && !isRunning && !isAttacking)
        {
            if (currentAxes <= 0) return;
            ThrowAxeHold();
            
        }
        if (Input.GetMouseButtonUp(2))
        {
            ThrowAxeRelease();
        }

        //Salto Input.
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // DIRECCIÓN RELATIVA A CÁMARA
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        // SOLO permitir movimiento si NO está atacando
        if (!isAttacking)
        {
            movement = (forward * vertical + right * horizontal).normalized;
        }
        else
        {
            movement = Vector3.zero;
        }

        // ANIMACIONES
        animator.SetFloat("SpeedX", horizontal);
        animator.SetFloat("SpeedY", vertical);

        // ROTACIÓN HACIA MOVIMIENTO

        if (isThrowingHold) 
        {
            RotateTowardsCrosshair();
        }
        else if (movement != Vector3.zero && !isAttacking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void FixedUpdate()
    {
        //Salto isGround check.
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);
        Debug.Log(isGrounded);

        // BLOQUEAR MOVIMIENTO DURANTE ATAQUE
        if (isAttacking)
        {
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );

            return;
        }

        if (isRunning)
        {
            rb.linearVelocity = new Vector3(
                movement.x * speedRun,
                rb.linearVelocity.y,
                movement.z * speedRun
            );

            return;
        }

        rb.linearVelocity = new Vector3(
            movement.x * speed,
            rb.linearVelocity.y,
            movement.z * speed
        );
    }


    //Activar el hitbox desede animaciones

    private WeaponDamage GetCurrentWeaponDamage()
    {
        switch (currentWeapon)
        {
            case WeaponType.BattleAxe:
                return battleAxe.GetComponentInChildren<WeaponDamage>();
            case WeaponType.Sword:
                return sword.GetComponentInChildren<WeaponDamage>();    
        }

        return null;
    }

    public void EnableHitbox()
    {
        WeaponDamage wd = GetCurrentWeaponDamage();

        if (wd != null)
        {
            wd.EnableHitBox();
        }
    }

    public void DisableHitbox()
    {
        WeaponDamage wd = GetCurrentWeaponDamage();
        if (wd != null)
        {
            wd.DisableHitBox();
        }
    }

    //Lanzar Hacha
    void ThrowAxe()
    {
        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
        }

        GameObject axe = Instantiate(
            throwAxePrefab,
            throwPoint.position,
            Quaternion.identity
        );

        Vector3 direction =
            (targetPoint - throwPoint.position).normalized;

        axe.transform.rotation =
            Quaternion.LookRotation(direction);

        Rigidbody axeRb =
            axe.GetComponent<Rigidbody>();

        axeRb.AddForce(
            direction * throwForce,
            ForceMode.Impulse
        );
    }

    private void ThrowAxeHold()
    {
        isThrowingHold = true;
        EquipThrowingAxe();
        animator.SetBool("IsThrowingHold", true);
    }

    private void ThrowAxeRelease()
    {
        if (!isThrowingHold) return;

        isThrowingHold = false;

        animator.SetBool("IsThrowingHold", false);
        animator.SetTrigger("ThrowAxe");
        currentAxes--;
        ThrowAxe();
        DesEquipThrowingAxe();
    }

    //Salto
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("Jump");
    }

    private void OnDrawGizmos()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundDistance);
    }

    //Tomar Arma

    private void PickWeapon()
    {
        if (nearbyWeapon == null) return;

        if (currentWeapon != WeaponType.None)
        {
            DropWeapon();
        }

        if (nearbyWeapon.CompareTag("AxePickUp"))
        {
            EquipAxe();
            Destroy(nearbyWeapon);
        }

        if (nearbyWeapon.CompareTag("SwordPickUp"))
        {
            EquipSword();
            Destroy(nearbyWeapon);
        }

        nearbyWeapon = null;
    }

    private void EquipAxe()
    {
        battleAxe.SetActive(true);
        sword.SetActive(false);
        throwAxeObj.SetActive(false);
        currentWeapon = WeaponType.BattleAxe;
    }

    private void EquipSword()
    {
        battleAxe.SetActive(false);
        sword.SetActive(true);
        throwAxeObj.SetActive(false);
        currentWeapon = WeaponType.Sword;
    }

    private void EquipThrowingAxe()
    {
        battleAxe.SetActive(false);
        sword.SetActive(false);
        throwAxeObj.SetActive(true);
    }

    private void DesEquipThrowingAxe()
    {
        throwAxeObj.SetActive(false);

        if (currentWeapon == WeaponType.BattleAxe)
        {
            EquipAxe();
        }
        if (currentWeapon == WeaponType.Sword)
        {
            EquipSword();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("AxePickUp") || col.CompareTag("SwordPickUp"))
        {
            nearbyWeapon = col.gameObject;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }

    //SoltarArma
    private void DropWeapon()
    {

        if (currentWeapon == WeaponType.BattleAxe)
        {
            Instantiate(axePickUpPrefab,dropPosition.position, Quaternion.identity);
            battleAxe.SetActive(false);
            currentWeapon = WeaponType.None;
        }

        if (currentWeapon == WeaponType.Sword)
        {
            Instantiate(swordPickUpPrefab, dropPosition.position, Quaternion.identity);
            sword.SetActive(false);
            currentWeapon = WeaponType.None;
        }
    }

    private void RotateTowardsCrosshair()
    {
        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
        }

        Vector3 lookDirection =
            targetPoint - transform.position;

        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(lookDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void StartComboSistem()
    {
        if (!isAttacking)
        {
            StartCombo();
            return;
        }

        if (canQueueCombo)
        {
            comboQueue = true;
        }

    }

    private void StartCombo()
    {
        Debug.Log("comenzo el combo");
        isAttacking = true;

        comboStep = 1;

        // DIRECCIÓN DE LA CÁMARA
        Vector3 attackDirection = cameraTransform.forward;
        attackDirection.y = 0f;

        // ROTAR PERSONAJE HACIA DONDE MIRA LA CÁMARA
        transform.rotation = Quaternion.LookRotation(attackDirection);

        animator.SetInteger("ComboStep", comboStep);
        animator.SetTrigger("Attack");
    }


    public void OpenComboWindow()
    {
        Debug.Log("se abrio ventana de combo");
        canQueueCombo = true;
    }

    public void CloseComboWindow()
    {
        Debug.Log("se cerro ventana de combo");
        canQueueCombo = false;

        if (comboQueue)
        {
            ContinueCombo();
        }
        else
        {
            EndCombo();
        }
    }

    private void ContinueCombo()
    {
        Debug.Log("combo continua");
        comboQueue = false;

        comboStep++;

        // DIRECCIÓN DE LA CÁMARA
        Vector3 attackDirection = cameraTransform.forward;
        attackDirection.y = 0f;

        // ROTAR PERSONAJE HACIA DONDE MIRA LA CÁMARA
        transform.rotation = Quaternion.LookRotation(attackDirection);

        int maxCombo = GetMaxCombo();

        if (comboStep > maxCombo)
        {
            EndCombo();
            return;
        }

        animator.SetInteger("ComboStep", comboStep);
        animator.SetTrigger("Attack");
    }

    private void EndCombo()
    {
        Debug.Log("Combo finalizado");
        comboQueue = false;
        canQueueCombo = false;

        comboStep = 0;

        isAttacking = false;
    }

    private int GetMaxCombo()
    {
        switch(currentWeapon)
        {
            case WeaponType.BattleAxe:
                return 3;

            case WeaponType.Sword:
                return 3;
        }

        return 1;
    }
}
