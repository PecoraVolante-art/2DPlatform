using UnityEngine;
using UnityEngine.UI;

public class LetteraIstruzioni : MonoBehaviour
{
    [SerializeField] private GameObject pressEIcon;
    [SerializeField] private GameObject lettera;
    private bool playerInRange = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        { 
        lettera.SetActive(true);
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
