using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] AudioSource musicSource;

    [Header("SFX")]
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip gunshotSFX;
    [SerializeField] AudioClip takeDamageSFX;
    [SerializeField] AudioClip deathSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayGunShot()  => sfxSource.PlayOneShot(gunshotSFX);
    public void PlayTakeDamage() => sfxSource.PlayOneShot(takeDamageSFX);
    public void PlayDeath() => sfxSource.PlayOneShot(deathSFX);
}
