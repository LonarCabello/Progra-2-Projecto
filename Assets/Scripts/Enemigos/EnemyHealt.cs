using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health;
    private float maxHealth;
    private bool isDead;
    private Rigidbody rb;

    
    public void Initialize(EnemyData data)
    {
        maxHealth = data.maxHealth;
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
    }
    private void Die()
    {
        rb.isKinematic = true;
        isDead = true;
    }
}