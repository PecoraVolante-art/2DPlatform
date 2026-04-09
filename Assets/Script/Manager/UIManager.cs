using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;
    public GameObject PausePanel;
    public Slider healthSlider;
    public Gradient gradient;
    public Image fill;
    private static UIManager instance;

    [Header("Stato di gioco")]
    public bool isGameOver = false;
    public bool isVictory = false;
    public bool isPaused = false;

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

        if (PausePanel != null)
            PausePanel.SetActive(false);

        Time.timeScale = 1f;

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

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (PausePanel != null)
            PausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

}
