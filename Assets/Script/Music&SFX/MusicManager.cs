using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GestoreMusica : MonoBehaviour
{
    public AudioClip musicaMenu;
    public AudioClip musicaBaseDungeon;
    public AudioClip musicaBoss;
    public AudioClip musicaGameOver;

    private AudioSource audioSource;
    private static GestoreMusica instance;

    public static GestoreMusica Instance
    {
        get { return instance; }
    }

    [Range(0f, 1f)]
    public float volume = 1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("Volume"))
        {
            volume = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Volume", volume);
        }

        audioSource.volume = volume;
    }

    void Start()
    {
        CambiaMusica(musicaMenu);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void CambiaMusica(AudioClip nuovaMusica)
    {
        if (audioSource.clip != nuovaMusica)
        {
            audioSource.Stop();
            audioSource.clip = nuovaMusica;
            audioSource.Play();
        }
    }

    public void ImpostaVolume(float nuovoVolume)
    {
        volume = nuovoVolume;
        audioSource.volume = volume;

        PlayerPrefs.SetFloat("Volume", volume);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            PlayMuicaMenu();
        }
        else if (scene.name == "Base" || scene.name == "Dungeon")
        {
            PlayMuicaDungeon();
        }
    }

    public void PlayMuicaDungeon()
    {
        CambiaMusica(musicaBaseDungeon);
    }

    public void PlayMuicaMenu()
    {
        CambiaMusica(musicaMenu);
    }

    public void PlayMusicaGameOver()
    {
        CambiaMusica(musicaGameOver);

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
