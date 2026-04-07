using UnityEngine;
using UnityEngine.SceneManagement;

public class Impostazione_livello : MonoBehaviour
{
    private Vector3 startingPosition = new Vector3(-11.8f, 0.247f, 0f);

    public void Playbutton()
    {
        // Resetta tutti i PlayerPrefs
        PlayerPrefs.DeleteAll();

        // Callback per quando la scena è caricata
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Carica la scena Base (LV1)
        SceneManager.LoadScene("Base");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Base" && PlayerManager.Instance != null)
        {
            PlayerManager.Instance.transform.position = startingPosition;
        }

        // Rimuovi callback per non ripetere la chiamata
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Goback_Menubutton()
    {
        SceneManager.LoadScene("Menu");
    }
}
