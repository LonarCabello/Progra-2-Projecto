
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

    void OnCollisionEnter(Collision collision)
    {
        // Insertar lógica de daño al player
        Debug.Log("Hit " + collision.gameObject.name);
        pooler.ReturnToPool(gameObject);
    }
}
