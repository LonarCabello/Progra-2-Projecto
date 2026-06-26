using UnityEngine;

public class AnimationSoundEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sonidos en Animation events.
    public void PlayPotionSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.potionDrink);
    }
    public void PlayThrowAxeSound()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.throwAxe);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.ghiir1);
    }
    public void PlayShieldHit()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.shieldBlock);
    }
    public void PlaySwordSwing()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.swordSwing);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.ghiir1);

    }
    public void PlaySwordSwing2()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.swordSwing2);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.ghiir2);

    }
    public void PlaySwordSwing3()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.swordSwing3);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.ghiir3);

    }
    public void PlayWoodFootStep1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.WoodFootStep1);
    }
    public void PlayWoodFootStep2()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.WoodFootStep2);
    }
    public void PlayDirtFootStep1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.FloorFootStep1);
    }
    public void PlayDirtFootStep2()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.FloorFootStep2);
    }
    public void PlayHurt()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.hurt);

    }
    public void PlayJump()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.Jump);

    }
    public void PlayDeath()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.Death);
    }
    public void PlayDeathBodyDrop()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.DeathBodyDrop);
    }
}
