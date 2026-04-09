
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowSetter : MonoBehaviour
{
    void Start()
    {
        CinemachineCamera cam = GetComponent<CinemachineCamera>();

        if (PlayerManager.Instance != null)
        {
            cam.Follow = PlayerManager.Instance.transform;
        }
    }
}