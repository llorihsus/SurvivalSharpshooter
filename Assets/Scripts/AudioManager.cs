using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] AudioSource musicSource;

    [Header("SFX")]
    [SerializeField] AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] AudioClip gunshotSFX;
    [SerializeField] AudioClip zombieAttackSFX;
    [SerializeField] AudioClip playerDamageSFX;
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] AudioClip zombieDeathSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void PlayGunShot()  => sfxSource.PlayOneShot(gunshotSFX);
    public void PlayZombieAttack() => sfxSource.PlayOneShot(zombieAttackSFX);
    public void PlayPlayerDamage() => sfxSource.PlayOneShot(playerDamageSFX);
    public void PlayPlayerDeath() => sfxSource.PlayOneShot(playerDeathSFX);
    public void PlayZombieDeath() => sfxSource.PlayOneShot(zombieDeathSFX);
}
