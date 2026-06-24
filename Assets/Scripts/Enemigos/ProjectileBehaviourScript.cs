
using UnityEngine;

public class ProjectileBehaviourScript : MonoBehaviour
{
    ObjectPooler pooler = ObjectPooler.Instance;
    int damage;

    void Start()
    {
        damage = pooler.pools.Find(pool => pool.tag == gameObject.tag).damage;
}
    void OnEnable()
    {
        // Desactivar el objeto después de 3 segundos.
        Invoke("Deactivate", 3f);
    }

    private void Deactivate()
    {
        pooler.ReturnToPool(gameObject);
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        // Insertar lógica de daño al player
        Debug.Log("Hit " + collision.gameObject.name);
        pooler.ReturnToPool(gameObject);
    }
    */

    private void OnTriggerEnter(Collider col)
    {
        // Insertar lógica de daño al player
        if (col.CompareTag("Player"))
        {
            HealthManager health = col.GetComponent<HealthManager>();
            health.TakeDamage(damage);
        }

        Debug.Log("Hit " + col.gameObject.name);
        pooler.ReturnToPool(gameObject);
    }
}
