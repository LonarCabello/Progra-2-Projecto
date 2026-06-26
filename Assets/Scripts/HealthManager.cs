/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 03 / 06 / 2026
 * Title        : HealthManager
 * Description  : Manager de vida de jugador/enemigos.
 * =============<< ********* >>=============
 */

using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 150;

    private int currentHealth;

    [SerializeField] float damageCoolDown = 0.5f;
    private bool canTakeDamage = true;  

    public bool IsDead { get; private set; }

    private EnemyBrain enemyBrain;

    private Animator anim;

    void Start()
    {  
        currentHealth = maxHealth;
    }

    private void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        if (!canTakeDamage) 
        {
            return;
        }

        if (this.CompareTag("Player"))
        {
            anim = GetComponent<Animator>();
            PlayerMov playerMov = GetComponent<PlayerMov>();
            if (playerMov.isBlocking == true)
            {
                Debug.Log("bloqueando, no recibe daño");
                anim.SetTrigger("TakenDamage");
                playerMov.isAttacking = false;
                return;
            }
            playerMov.isAttacking = false;
        }

        if (this.CompareTag("Enemy"))
        {
            anim = GetComponentInChildren<Animator>();
            enemyBrain = GetComponent<EnemyBrain>();
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        anim.SetTrigger("TakenDamage");

        Debug.Log(gameObject.name + " recibió " + damage + " de daño. Ahora tiene: " + currentHealth + " puntos de vida ");

        canTakeDamage = false;
        Invoke(nameof(ResetDamageCoolDown), damageCoolDown);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HealTrigger()
    {
        if (IsDead) return;

        Debug.Log("por curarse");
        anim.SetTrigger("Heal");

    }

    public void Heal()
    {
        if (IsDead)
            return;

        //currentHealth += 50;

        currentHealth = Mathf.Clamp(currentHealth + 50, 0, maxHealth);

        Debug.Log(gameObject.name + " Se curo " + 50 + " de vida. Ahora tiene: " + currentHealth + " puntos de vida ");

        PlayerMov playerMov = GetComponent<PlayerMov>();
        playerMov.isHealing = false;
    }

    private void ResetDamageCoolDown()
    {
        canTakeDamage = true;
    }

    void Die()
    {
        IsDead = true;

        anim.SetTrigger("Death");

        Debug.Log(gameObject.name + " murió");

        if (this.CompareTag("Enemy"))
        {
            enemyBrain.Muerto();
        }
        if (this.CompareTag("Player"))
        {
            DeathUIManager.Instance.ShowDeathScreen();
        }
    }

    //Devuelve la vida actual

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    //Devuelve el porcentaje de la vida actual.

    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        IsDead = false;
    }
}
