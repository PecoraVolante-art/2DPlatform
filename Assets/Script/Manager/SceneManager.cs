using UnityEngine;
using UnityEngine.SceneManagement;

public class Impostazione_livello : MonoBehaviour
{
    private Vector3 startingPosition = new Vector3(-11.8f, 0.247f, 0f);

    public void Playbutton()
    {

        PlayerPrefs.DeleteAll();

        SceneManager.sceneLoaded += ResetPlayerOnLoad;

        SceneManager.LoadScene("Base");
    }

    public void Goback_Menubutton()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ResetPlayerOnLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Base" && PlayerManager.Instance != null)
        {
            PlayerManager.Instance.ResetPlayerToStart(startingPosition);
        }

        // Rimuovi listener dopo l'uso
        SceneManager.sceneLoaded -= ResetPlayerOnLoad;
    }
}
