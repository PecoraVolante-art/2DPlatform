using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;
    public Slider healthSlider;
    public Gradient gradient;
    public Image fill;
    private static UIManager instance;

    [Header("Stato di gioco")]
    public bool isGameOver = false;
    public bool isVictory = false;

    private void Awake()
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
        if (scene.name == "Menu")
        {
            Destroy(gameObject);
            return;
        }

        isGameOver = false;
        isVictory = false;

        if (GameOverPanel != null)
            GameOverPanel.SetActive(false);

    }


    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

    }

    public void Victory()
    {
        if (!isVictory)
        {
            isVictory = true;
            Debug.Log("Hai sconfitto il boss!");
            if (GameOverPanel != null)
                VictoryPanel.SetActive(true);
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;

        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }

}
