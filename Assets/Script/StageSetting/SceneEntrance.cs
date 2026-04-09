using UnityEngine;
using System.Collections;

public class SceneEntrance : MonoBehaviour
{
    public string lastExitName;

    void Start()
    {
        if (PlayerPrefs.GetString("LastExitName") == lastExitName)
        {
            PlayerManager.Instance.transform.position = transform.position;
        }
    }

}
