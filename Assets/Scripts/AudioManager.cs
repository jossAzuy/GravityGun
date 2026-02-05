using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip shootClip;
    [Range(0f, 1f)] public float shootVolume = 1f;

    public AudioClip reloadAmmoClip;
    [Range(0f, 1f)] public float reloadVolume = 1f;

    public AudioClip enemyDeathClip;
    [Range(0f, 1f)] public float enemyDeathVolume = 1f;

    public AudioClip playerBounceClip;
    [Range(0f, 2f)] public float playerBounceVolume = 1f;
    public AudioClip playerTakeDamageClip;
    [Range(0f, 2f)] public float playerTakeDamageVolume = 1f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayShootSound()
    {
        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip, shootVolume);
        }
    }

    /*   public void PlaySound()
      {
          if (audioSource != null && reloadAmmoClip != null)
          {
              audioSource.PlayOneShot(reloadAmmoClip, reloadVolume);
          }
      } */

    public void PlayReloadAmmoSound()
    {
        if (audioSource != null && reloadAmmoClip != null)
        {
            audioSource.PlayOneShot(reloadAmmoClip, reloadVolume);
        }
    }

    public void PlayEnemyDeathSound()
    {
        if (audioSource != null && enemyDeathClip != null)
        {
            audioSource.PlayOneShot(enemyDeathClip, enemyDeathVolume);
        }
    }

    public void PlayPlayerTakeDamage()
    {
        if (audioSource != null && playerTakeDamageClip != null)
        {
            audioSource.PlayOneShot(playerTakeDamageClip, playerTakeDamageVolume);
        }
    }

    public void PlayPlayerBounce()
    {
        if (audioSource != null && playerBounceClip != null)
        {
            audioSource.PlayOneShot(playerBounceClip, playerBounceVolume);
        }
    }
}