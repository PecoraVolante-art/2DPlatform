using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    public string exitName;

    [SerializeField] private GameObject pressEIcon;
    private bool playerInRange = false;

    [SerializeField] private GameObject portal;      
    [SerializeField] private Animator portalAnimator; 

    private void Start()
    {

        if (portal != null)
            portal.SetActive(false);

        // Controlla se tutti gli obiettivi sono completati
        if (GameManager.Instance != null && GameManager.Instance.AreAllObjectivesCompleted(4))
        {
            ShowAndActivatePortal();
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetString("LastExitName", exitName);
            SceneManager.LoadScene(sceneToLoad);

        }
    }


    public void ShowAndActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);         
            if (portalAnimator != null)
                portalAnimator.SetBool("isActive", true); 
            Debug.Log("Portale apparso e attivato!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            pressEIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEIcon.SetActive(false);
        }
    }
}