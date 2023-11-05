using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public List<GameObject> level1Platforms;
    public List<GameObject> level2Platforms;
    public List<GameObject> level3Platforms;
    public List<GameObject> rarePlatforms;

    public Transform lastPlatform;
    private int maxPlatforms = 30;
    public int score = 0;

    private List<GameObject> spawnedPlatforms = new List<GameObject>();
    private List<GameObject> activePlatforms = new List<GameObject>();

    private bool generating = false;
    private Vector3 lastPosition;
    private Vector3 newPosition;

    void Start()
    {
    lastPosition = lastPlatform.position; // Ustaw lastPosition na pozycję lastPlatform na początku
    StartCoroutine(SpawnPlatforms());
    }


    void Update()
    {
        if (activePlatforms.Count < maxPlatforms && !generating)
        {
            StartCoroutine(SpawnPlatforms());
        }
    }

    public void DecreaseGeneratedPlatforms()
    {
        if (activePlatforms.Count > 0)
        {
            GameObject platformToRemove = activePlatforms[0];
            activePlatforms.RemoveAt(0);
            spawnedPlatforms.Remove(platformToRemove);
            Destroy(platformToRemove);
        }
    }

    IEnumerator SpawnPlatforms()
{
    generating = true;
    List<GameObject> currentPlatforms;

    if (score < 1000)
    {
        currentPlatforms = level1Platforms;
    }
    else if (score >= 1000 && score < 2000)
    {
        currentPlatforms = level2Platforms;
    }
    else
    {
        currentPlatforms = level3Platforms;
    }

    while (activePlatforms.Count < maxPlatforms)
    {   
        bool spawnedRarePlatform = false;
        int randomRare = Random.Range(0, 100);
        
        if (randomRare < 5)
        {
            int randomRareIndex = Random.Range(0, rarePlatforms.Count);
            Instantiate(rarePlatforms[randomRareIndex], newPosition, Quaternion.identity);
            spawnedRarePlatform = true;
        }
        
        GameObject newPlatform = null; // Deklaracja zmiennej poza blokiem if

        if (!spawnedRarePlatform)
        {
            int randomIndex = Random.Range(0, currentPlatforms.Count);
            newPlatform = Instantiate(currentPlatforms[randomIndex], newPosition, Quaternion.identity);
            Platform platformScript = newPlatform.GetComponent<Platform>();
        }

        GeneratePosition();
        lastPosition = newPosition;

        if (newPlatform != null)
        {
            activePlatforms.Add(newPlatform);
            spawnedPlatforms.Add(newPlatform);
        }

        yield return new WaitForSeconds(0.05f);
    }

    generating = false;
}


    void GeneratePosition()
    {
        newPosition = lastPosition;

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            newPosition.x += 1.6f;
        }
        else
        {
            newPosition.z += 1.6f;
        }
    }
}
