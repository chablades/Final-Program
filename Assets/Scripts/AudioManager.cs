using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip swordSwingSound;
    [SerializeField] private AudioClip playerDamageSound;
    [SerializeField] private AudioClip enemyDamageSound;
    [SerializeField] private AudioClip enemyDeathSound;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Create audio sources if not assigned
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.volume = 0.5f;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
        }
    }

    public void PlaySwordSwing()
    {
        if (swordSwingSound != null)
            sfxSource.PlayOneShot(swordSwingSound);
    }

    public void PlayPlayerDamage()
    {
        if (playerDamageSound != null)
            sfxSource.PlayOneShot(playerDamageSound);
    }

    public void PlayEnemyDamage()
    {
        if (enemyDamageSound != null)
            sfxSource.PlayOneShot(enemyDamageSound);
    }

    public void PlayEnemyDeath()
    {
        if (enemyDeathSound != null)
            sfxSource.PlayOneShot(enemyDeathSound);
    }
} 