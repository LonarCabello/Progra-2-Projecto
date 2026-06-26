using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [Header("Combate")]
    public AudioClip swordSwing;
    public AudioClip swordSwing2;
    public AudioClip swordSwing3;
    public AudioClip ghiir1;
    public AudioClip ghiir2;
    public AudioClip ghiir3;
    public AudioClip Hit;
    public AudioClip Death;
    public AudioClip FemaleDeath;
    public AudioClip DeathBodyDrop;
    public AudioClip shieldBlock;
    public AudioClip hurt;
    public AudioClip femalehurt;
    public AudioClip Jump;
    public AudioClip EnemyBowLoading;
    public AudioClip EnemyBowRealease;
    public AudioClip EnemySwingSword;

    [Header("Items")]
    public AudioClip potionDrink;
    public AudioClip throwAxe;
    public AudioClip pickupItem;
    public AudioClip potionFill;
    public AudioClip DropItem;

    [Header("Pisadas")]
    public AudioClip dirtFootStep1;
    public AudioClip dirtFootStep2;
    public AudioClip WoodFootStep1;
    public AudioClip WoodFootStep2;
    public AudioClip FloorFootStep1;
    public AudioClip FloorFootStep2;

    [Header("Musica")]
    public AudioClip BackgroundMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    private void Start()
    {
        PlayMusic(BackgroundMusic);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
