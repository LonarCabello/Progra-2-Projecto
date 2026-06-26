using System.Collections;
using UnityEditor.Search;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Transform player;
    ObjectPooler pooler;
    [SerializeField] private GameObject firePoint;
    string projectileTag;
    EnemyData enemyData;
    public bool inAttack;
    EnemyMotion motion;
    Animator anim;
    float attackProbability = 0.5f; // Probabilidad de ataque (50%)


    public void Initialize(EnemyData data, EnemyMotion motion, Transform player)
    {
        anim = GetComponentInChildren<Animator>();
        this.enemyData = data;
        this.motion = motion;
        this.player = player;
        if(data.enemyType == EnemyType.Ranged || data.enemyType == EnemyType.Spectrum)
        {
            pooler = ObjectPooler.Instance;
            if (pooler == null)
            {
                Debug.LogWarning("No se encontró una instancia de ObjectPooler en la escena.");
            }
            projectileTag = enemyData.projectileTag;
        }

    }

    public void Attack(Vector3 direction)
    {
        if (inAttack) return;
        inAttack = true;

        switch (enemyData.enemyType)
        {
            case EnemyType.Ranged:
                anim.SetTrigger("Shoot");
                inAttack = false;
                break;
            case EnemyType.Melee:
                golpear(direction);
                inAttack = false;
                break;
            case EnemyType.Spectrum:
                selectAttackOfSpectrum(direction);
                break;
            default:
                Debug.LogWarning("Tipo de enemigo no reconocido.");
                break;
        }
    }
    public void Shot(bool towardsPlayer = true)
    {
        
        GameObject projectile = pooler.SpawnFromPool(projectileTag, firePoint, towardsPlayer ? player.position - transform.position : transform.forward);
    }

    
    private void golpear(Vector3 direction)
    {
        // Aquí iría la lógica para el ataque cuerpo a cuerpo, como aplicar daño al jugador si está dentro del rango de ataque.
        Debug.Log("Golpeando al jugador!");
    }
    IEnumerator attackSpectrumGolpe(Vector3 direction)
    {
        // Pasar a modo tangible
        Vector3 initialPosition = transform.position;
        Debug.LogWarning("Iniciando ataque de espectro desplazamiento");
              
        Vector3 positionToGo = transform.position + direction + direction.normalized * 3f;
        while (Vector3.Distance(transform.position, positionToGo) > 0.5f)
        {
            motion.GoTo(positionToGo, true);
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);
        // Pasar a modo intangible
        motion.GoTo(initialPosition);
        inAttack = false;
        Debug.LogWarning("Finalizando ataque de espectro desplazamiento");
    }
    IEnumerator attackSpectrumShoot(float duration, float shootInterval, float degreesTotal)
    {
        Debug.LogWarning("Iniciando ataque de espectro Disparos giratorios");
        Quaternion initialRotation = transform.rotation;
        float timeElapsed = 0f;
        float lastShootTime = 0f;
        while (timeElapsed < duration)
        {
            yield return null;
            timeElapsed += Time.deltaTime;
            transform.rotation = initialRotation * Quaternion.Euler(0, degreesTotal / duration * timeElapsed, 0);
            if (timeElapsed - lastShootTime >= shootInterval)
            {
                lastShootTime = timeElapsed;
                Shot(false);
            }
        }
        yield return new WaitForSeconds(2f); // Espera un segundo antes de finalizar el ataque
        inAttack = false;
        Debug.LogWarning("Finalizando ataque de espectro Disparos giratorios");
    }

    private void selectAttackOfSpectrum(Vector3 direction)
    {
        // Lógica para seleccionar el tipo de ataque del enemigo espectro.
        float aux = Random.Range(0f, 1f);
        if (aux < attackProbability)
        {
            StartCoroutine(attackSpectrumGolpe(direction));
            attackProbability -= 0.15f; // Reducir la probabilidad de ataque cuerpo a cuerpo en un 10%
        }
        else
        {
            StartCoroutine(attackSpectrumShoot(6f, 0.1f, 1440f)); // 1440 grados(4 vueltas) en 6 segundos, disparando cada 0.1 segundos
            attackProbability += 0.15f; // Aumentar la probabilidad de ataque cuerpo a cuerpo en un 10%
        }
    }
}
