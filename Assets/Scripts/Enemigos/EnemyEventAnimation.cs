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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAtack = GetComponentInParent<EnemyAttack>();
        audioSource = GetComponentInParent<AudioSource>();
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
}
