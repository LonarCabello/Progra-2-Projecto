using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] GameObject player;
    ObjectPooler pooler;
    [SerializeField] private GameObject firePoint;
    string projectileTag;
    EnemyData enemyData;
    bool inAttack;
    EnemyMotion motion;

    public void Initialize(EnemyData data, EnemyMotion motion)
    {
        this.enemyData = data;
        this.motion = motion;
        if(data.enemyType == EnemyType.Ranged)
        {
            pooler = ObjectPooler.Instance;
            if (pooler == null)
            {
                Debug.LogWarning("No se encontró una instancia de ObjectPooler en la escena.");
            }
            projectileTag = "Arrow";
        }

    }

    public void Attack(Vector3 direction)
    {
        if (inAttack) return;
        inAttack = true;

        switch (enemyData.enemyType)
        {
            case EnemyType.Ranged:
                Shot(direction);
                break;
            case EnemyType.Melee:
                golpear(direction);
                break;
            case EnemyType.Spectrum:
                atackSpectrum(direction);
                break;
            default:
                Debug.LogWarning("Tipo de enemigo no reconocido.");
                break;
        }
    }
    private void Shot(Vector3 direction)
    {
        GameObject projectile = pooler.SpawnFromPool(projectileTag, firePoint, direction.normalized);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        inAttack = false;
        // projectile.GetComponent<Rigidbody>().velocity = direction * pooler.pools.Find(pool => pool.tag == projectileTag).velocity;
    }
    private void golpear(Vector3 direction)
    {
        // Aquí iría la lógica para el ataque cuerpo a cuerpo, como aplicar daño al jugador si está dentro del rango de ataque.
        Debug.Log("Golpeando al jugador!");
        inAttack = false;
    }
    private void atackSpectrum(Vector3 direction)
    {
        // Pasar a modo tangible
        motion.GoTo(transform.position + direction+transform.forward*3, true);
        inAttack = false;
    }
}
