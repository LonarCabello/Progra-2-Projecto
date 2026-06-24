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
