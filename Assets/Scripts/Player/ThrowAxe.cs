/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 11 / 06 / 2026
 * Title        : ThrowAxe
 * Description  : Comportamiento de hachas arrojadizas.
 * =============<< ********* >>=============
 */

using UnityEngine;

public class ThrowAxe : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float rotationSpeed = 1000f;

    private Rigidbody rb;
    private bool isStuck = false;

    CapsuleCollider capsuleCol;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        capsuleCol = GetComponent<CapsuleCollider>();
    }
    private void Update()
    {
        if (!isStuck)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger con: " + col.name);

        if (isStuck)
        {
            if (col.CompareTag("Player"))
            {
                Debug.Log("Tocando Player");

                PlayerMov player = col.GetComponent<PlayerMov>();

                if (player != null)
                {
                    player.currentAxes++;

                    if (player.currentAxes > player.maxAxes)
                    {
                        player.currentAxes = player.maxAxes;
                    }
                }

                Destroy(gameObject);
            }

            return;
        }

            if (col.CompareTag("Enemy"))
        {
            HealthManager health = col.GetComponent<HealthManager>();

            if (health != null)
                {
                    health.TakeDamage(damage);
                }

        }

        if (col.CompareTag("Ground"))
        {
            StickToGround();
        }

    }

    private void StickToGround()
    {
        isStuck = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        capsuleCol.gameObject.SetActive(true);

        GetComponent<Collider>().isTrigger = true;
    }
}
