using UnityEngine;
using System.Collections;

public class BossStartStatue : MonoBehaviour
{
    [SerializeField] private GameObject pressEIcon;
    [SerializeField] private GameObject flame;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject WhiteMoon;
    [SerializeField] private GameObject RedMoon;
    [SerializeField] private Transform BossSpawnPoint;
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] private GameObject SpawnEffect;

    private bool playerInRange = false;
    private bool flameActive = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !flameActive)
        {
            pressEIcon.SetActive(false);
            flame.SetActive(true);
            flameActive = true;
            portal.SetActive(false);
            WhiteMoon.SetActive(false);
            RedMoon.SetActive(true);

            StartCoroutine(SpawnBossAfterDelay());

        }
        if (flameActive)
            pressEIcon.SetActive(false);

    }

    IEnumerator SpawnBossAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (SpawnEffect != null)
        {
            Instantiate(SpawnEffect, BossSpawnPoint.position, Quaternion.identity);
        }


        Instantiate(Boss, BossSpawnPoint.position, Quaternion.identity);
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
