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
    private AudioSource audioSource;
    private Animator anim;
    private CapsuleCollider capsuleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAtack = GetComponentInParent<EnemyAttack>();
        audioSource = GetComponentInParent<AudioSource>();
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySounds(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
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


    public void EnableCapsuleColliderHitbox()
    {
        capsuleCollider.enabled = true;
    }
    public void DisableCapsuleColliderHitbox()
    {
        capsuleCollider.enabled = false;
    }

    public void AtravesandoTrue()
    {
        anim.SetBool("Atravesando", true);
    }

    public void PlayEnemyHit()
    {
        PlaySounds(SoundManager.Instance.Hit);
    }
    public void PlayEnemyDeath()
    {
        PlaySounds(SoundManager.Instance.Death);
    }
    public void PlayFootStep1()
    {
        PlaySounds(SoundManager.Instance.FloorFootStep1);
    }
    public void PlayFootStep2()
    {
        PlaySounds(SoundManager.Instance.FloorFootStep2);
    }
    public void PlayEnemyDeathDropBody()
    {
        PlaySounds(SoundManager.Instance.DeathBodyDrop);
    }
    public void PlayEnemySwingSword()
    {
        PlaySounds(SoundManager.Instance.EnemySwingSword);
    }
    public void PlayBossLaughing()
    {
        PlaySounds(SoundManager.Instance.BossLaughing);
    }
    public void PlayBossAttackProjectiles()
    {
        PlaySounds(SoundManager.Instance.BossAttackProjectiles);

    }
    public void PlayBossAttackAtravesando()
    {
        PlaySounds(SoundManager.Instance.BossAttackAtravesando);
    }
    public void PlayBossHurt()
    {
        PlaySounds(SoundManager.Instance.BossHurt);

    }
    public void PlayBossDeath()
    {
        PlaySounds(SoundManager.Instance.BossDead);

    }
}
