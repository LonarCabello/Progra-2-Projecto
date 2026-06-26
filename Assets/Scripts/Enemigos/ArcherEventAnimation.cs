/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 25 / 06 / 2026
 * Title        : AnimacionesArcher.
 * Description  : Controla Eventos en animaciones del enemigo archer.
 * =============<< ********* >>=============
 */

using UnityEngine;

public class ArcherEventAnimation : MonoBehaviour
{

    private EnemyAttack enemyAtack;

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
}
