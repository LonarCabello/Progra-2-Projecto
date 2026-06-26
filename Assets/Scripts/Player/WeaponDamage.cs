/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 03 / 06 / 2026
 * Title        : WeaponDamage
 * Description  : Controla el daño que hacen las armas, como tambien sus colisiones.
 * =============<< ********* >>=============
 */

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
