/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 25 / 06 / 2026
 * Title        : EnemyEventAnimation.
 * Description  : Controla los eventos de las animaciones de los enemigos.
 * =============<< ********* >>=============
 */

using UnityEngine;

public class EnemyEventAnimation : MonoBehaviour
{
    private EnemyAttack enemyAtack;
    [SerializeField] private GameObject enemyWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAtack = GetComponentInParent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootinAnimation()
    {
        enemyAtack.Shot();
    }

    public void EnableWeaponHitbox()
    {
        EnemyWeaponDamage enemyWD = enemyWeapon.GetComponentInChildren<EnemyWeaponDamage>();
        if (enemyWD != null)
        {
            enemyWD.EnableHitBox();
        }
    }
    public void DisableWeaponHitbox()
    {
        EnemyWeaponDamage enemyWD = enemyWeapon.GetComponentInChildren<EnemyWeaponDamage>();
        if (enemyWD != null)
        {
            enemyWD.DisableHitBox();
        }
    }
}
