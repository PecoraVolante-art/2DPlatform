using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private PlayerController player;
    UIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            PlayerPrefs.DeleteKey("LastExitName");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Se torni al menu, distruggi il giocatore per evitare bug
        if (scene.name == "Menu")
        {
            if (player != null)
            {
                Destroy(player.gameObject);
                player = null;
            }
        }
        else
        {
            // Trova il giocatore in scena
            if (player == null)
            {
                player = Object.FindAnyObjectByType<PlayerController>();
            }
        }
    }

    // Metodo pubblico per resettare il giocatore
    public void ResetPlayerToStart(Vector3 startPosition)
    {
        if (player == null)
        {
            player = Object.FindAnyObjectByType<PlayerController>();
            if (player == null)
            {
                Debug.LogWarning("Nessun PlayerController trovato in scena!");
                return;
            }
        }

        player.uiManager = Object.FindAnyObjectByType<UIManager>();

        player.ResetPlayer();
        player.transform.position = startPosition;
    }
}

