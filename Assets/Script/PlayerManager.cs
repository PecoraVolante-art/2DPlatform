using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;


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
}