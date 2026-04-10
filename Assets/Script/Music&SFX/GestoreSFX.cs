using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

[RequireComponent(typeof(AudioSource))]
public class GestioneSFX : MonoBehaviour
{
    public static GestioneSFX Instance;

    [Header("AudioClips")]
    public AudioClip death;
    public AudioClip attack;
    public AudioClip destroyEnemy1;
    public AudioClip destroyEnemy2;
    public AudioClip destroyBoss;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip EnemyAttack;
    public AudioClip bossAttack;
    public AudioClip playerHurt;
    public AudioClip hurtEnemy1;
    public AudioClip hurtEnemy2;
    public AudioClip hurtBoss;
    public AudioClip scale;
    public AudioClip portal;
    public AudioClip statue;
    public AudioClip statueBoss;
    public AudioClip fire;
    public AudioClip unlockDoor;
    public AudioClip lockdoor;
    public AudioClip spawnEnemy;
    public AudioClip playerHurtTrap;
    public AudioClip musicaVittoria;


    [Range(0f, 1f)]
    public float volume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        if (PlayerPrefs.HasKey("SFXVolume"))
            volume = PlayerPrefs.GetFloat("SFXVolume");
        else
            PlayerPrefs.SetFloat("SFXVolume", volume);

        audioSource.volume = volume;
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        Debug.Log("GestioneSFX attivo in scena: " + scene.name);
    }

    public void PlayJump()
    {
        PlaySFX(jump);
    }

    public void PlayAttack()
    {
        PlaySFX(attack);
    }

    public void PlayDash()
    {
        PlaySFX(dash);
    }

    public void PlayDeath()
    {
        PlaySFX(death);
    }


    public void PlayDestroyEnemy1()
    {
        PlaySFX(destroyEnemy1);
    }

    public void PlayDestroyEnemy2()
    {
        PlaySFX(destroyEnemy2);
    }

    public void PlayDestroyEnemyBoss()
    {
        PlaySFX(destroyBoss);
    }

    public void PlayEnemyAttack()
    {
        PlaySFX(EnemyAttack);
    }

    public void PlayHurtEnemy1()
    {
        PlaySFX(hurtEnemy1);
    }

    public void PlayHurtEnemy2()
    {
        PlaySFX(hurtEnemy2);
    }

    public void PlayHurtBoss()
    {
        PlaySFX(hurtBoss);
    }

    public void PlayScale()
    {
        PlaySFX(scale);
    }

    public void PlayPortal()
    {
        PlaySFX(portal);
    }

    public void PlayStatue()
    {
        PlaySFX(statue);
    }

    public void PlayStatueBoss()
    {
        PlaySFX(statueBoss);
    }

    public void PlayFire()
    {
        PlaySFX(fire);
    }

    public void PlayLockDoor()
    {
        PlaySFX(lockdoor);
    }

    public void PlayUnlockDoor()
    {
        PlaySFX(unlockDoor);
    }

    public void PlaySpawnEnemy()
    {
        PlaySFX(spawnEnemy);
    }

    public void PlayPlayerHurt()
    {
        PlaySFX(playerHurt);
    }

    public void PlayPlayerHurtTrap()
    {
        PlaySFX(playerHurtTrap);
    }

    public void PlaybossAttack()
    {
        PlaySFX(bossAttack);
    }

    public void PlayVictorySound()
    {
        PlaySFX(musicaVittoria);
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip, volume); //suona la clip senza interrompere le gli altri audio in base al volume
    }


    public void AddSFXToButton(Button button, AudioClip clip)
    {
        if (button == null || clip == null) return;

        button.onClick.AddListener(() => PlaySFX(clip)); //per ogni click riproduce sfx
    }

    // Cambia volume e salva
    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume); // salva il volume sfx
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

