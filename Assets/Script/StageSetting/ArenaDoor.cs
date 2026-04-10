using System.Collections.Generic;
using UnityEngine;

public class ArenaDoor : MonoBehaviour
{

    public GameObject portaPrefab;
    public Transform spawnPoint1;
    public Transform spawnPoint2;

    public GameObject Door1;
    public GameObject Door2;


    public List<GameObject> enemies = new List<GameObject>();


    private bool playerActivate = false;
    private bool porteAperte = false;


    void Start()
    {

    }


    void Update()
    {
        enemies.RemoveAll(item => item == null);

 
        if (playerActivate && enemies.Count == 0 && !porteAperte)
        {
            if (GestioneSFX.Instance != null)
                GestioneSFX.Instance.PlayUnlockDoor();
            SbloccaPorte();
            porteAperte = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerActivate)
        {
            AttivaTrappola();
        }
    }

    void AttivaTrappola()
    {
        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.PlayLockDoor();
        playerActivate = true;
        Door1 = Instantiate(portaPrefab, spawnPoint1.position, Quaternion.identity);
        Door2 = Instantiate(portaPrefab, spawnPoint2.position, Quaternion.identity);
        Debug.Log("Trappola attivata! Porte chiuse.");

    }

    public void SbloccaPorte()
    {
        if (Door1 != null) Destroy(Door1);
        if (Door2 != null) Destroy(Door2);
        Debug.Log("Porte rimosse!");
    }
}
