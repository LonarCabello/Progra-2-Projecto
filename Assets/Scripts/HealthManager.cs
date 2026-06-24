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
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        anim.SetTrigger("TakenDamage");

        Debug.Log(gameObject.name + " recibió " + damage + " de daño");

        canTakeDamage = false;
        Invoke(nameof(ResetDamageCoolDown), damageCoolDown);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        currentHealth += amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
}
