using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private bool isFalling = false;
    private Rigidbody rb;
    private PlatformSpawner platformSpawner; // Referencja do skryptu PlatformSpawner.

    public float vanishDelay = 3.0f; // Czas opóźnienia przed zniknięciem platformy po przekroczeniu przez gracza.
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Ustaw platformę jako kinematyczną na początku
        platformSpawner = FindObjectOfType<PlatformSpawner>(); // Znajdź PlatformSpawner w scenie.
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isFalling)
        {
            isFalling = true;
            StartCoroutine(FallPlatform());
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        StartCoroutine(VanishAfterDelay());
        // Odtwórz dźwięk monet
    }
}


    private IEnumerator VanishAfterDelay()
    {
        yield return new WaitForSeconds(vanishDelay);
        DestroyPlatform();
    }

    private IEnumerator FallPlatform()
    {   
        yield return new WaitForSeconds(0.5f); // Poczekaj 0.2 sekundy przed spadnięciem
        rb.isKinematic = false; // Ustaw platformę jako niekinematyczną
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Zablokuj rotację platformy
        rb.velocity = new Vector3(0f, -5f, 0f); // Ustaw prędkość spadania w kierunku "dół"
        
    }

    private void DestroyPlatform()
{
    if (platformSpawner != null)
    {
        platformSpawner.DecreaseGeneratedPlatforms(); // Zmniejsz licznik wygenerowanych platform przed zniszczeniem.
    }

    Destroy(gameObject);
}

}
