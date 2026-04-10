using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int objectivesCompleted = 0;
    public bool sfxStatuePlayed = false;

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
    }

    public void CompleteObjective()
    {
        objectivesCompleted++;
        Debug.Log("Obiettivo completato! Totale: " + objectivesCompleted);

        if (objectivesCompleted >= 4 && sfxStatuePlayed == false)
        {
            if (GestioneSFX.Instance != null)
                GestioneSFX.Instance.PlayStatue();

            sfxStatuePlayed = true;
        }
    }

    public bool AreAllObjectivesCompleted(int totalObjectives)
    {
        return objectivesCompleted >= totalObjectives;
    }


}