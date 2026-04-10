using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] private GameObject pressEIcon;
    [SerializeField] private GameObject flame;

    private bool playerInRange = false;
    private bool flameActive = false;
    private bool counted = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !flameActive)
        {
            if (GestioneSFX.Instance != null)
                GestioneSFX.Instance.PlayFire();
            pressEIcon.SetActive(false);
            flame.SetActive(true);
            flameActive = true;

            if (!counted)
            {
                GameManager.Instance.CompleteObjective();
                counted = true;
            }

        }

        if (flameActive)
            pressEIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !flameActive)
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