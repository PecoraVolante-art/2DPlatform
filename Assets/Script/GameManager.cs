using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject GameOverPanel;

    public int objectivesCompleted = 0;

    [Header("Stato di gioco")]
    public bool isGameOver = false;
    public bool isVictory = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
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
            Debug.Log("Hai sconfitto il boss! Vittoria!");
        }
    }

    public void CompleteObjective()
    {
        objectivesCompleted++;
        Debug.Log("Obiettivo completato! Totale: " + objectivesCompleted);
    }

    public bool AreAllObjectivesCompleted(int totalObjectives)
    {
        return objectivesCompleted >= totalObjectives;
    }


}
