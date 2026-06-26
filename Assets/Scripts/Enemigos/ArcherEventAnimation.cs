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
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAtack = GetComponentInParent<EnemyAttack>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySounds(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void ShootinAnimation()
    {
        enemyAtack.Shot();
        PlaySounds(SoundManager.Instance.EnemyBowRealease);
    }
    public void EnemyBowLoading()
    {
        PlaySounds(SoundManager.Instance.EnemyBowLoading);
    }
    public void PlayFootStep1()
    {
        PlaySounds(SoundManager.Instance.FloorFootStep1);
    }
    public void PlayFootStep2()
    {
        PlaySounds(SoundManager.Instance.FloorFootStep2);
    }

    public void PlayEnemyHit()
    {
        PlaySounds(SoundManager.Instance.femalehurt);
    }
    public void PlayEnemyDeath()
    {
        PlaySounds(SoundManager.Instance.FemaleDeath);
    }
    public void PlayEnemyDeathDropBody()
    {
        PlaySounds(SoundManager.Instance.DeathBodyDrop);
    }
}
