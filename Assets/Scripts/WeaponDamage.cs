using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    private BoxCollider boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Enemy"))
            return;

        HealthManager health = col.GetComponent<HealthManager>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }

    public void EnableHitBox()
    {
        boxCol.enabled = true;
    }
    public void DisableHitBox()
    {
        boxCol.enabled = false;
    }
}
