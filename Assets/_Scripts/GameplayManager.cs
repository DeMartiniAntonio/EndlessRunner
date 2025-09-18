using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    private int score;
    private float timeElapsed;

    [SerializeField] private GameObject[] badObjectsToSpawn; // Array of objects to spawn
    [SerializeField] private GameObject[] collectables; // Array of objects to spawn
    [SerializeField] private GameObject road; // Array of objects to spawn
    [SerializeField] private float hightOfObject; // Array of objects to spawn

    private int randomSpownLineIndex;
    private int randomSecondSpownLineIndex;
    private int randomNumberOfObjectsToSpown;
    private int randomObjectListToPool;
    private int randomBadObjectToSpown;
    private int randomCollectableSpown;
    private int randomChanceTOSpownCollectableOnTopOfObject;

    private float waveSpownerTimer = 1;
    private bool isEndScreenActive = false;

    private Vector3 roadStartingPlace;
    private bool isCoroutineStarted= false;
    
    private void Start()
    {
        roadStartingPlace = road.transform.position;
        score = 0;
        timeElapsed = 0f;
        Debug.Log("uso");
        isEndScreenActive = false;
        //StartCoroutine(SpowningObjects());
    }

    public void StartSpowningObjectCorutine() {
        
        if (!isCoroutineStarted) {
            StartCoroutine(SpowningObjects());
        }
        isCoroutineStarted = true;
    }

    public void RestartGameObjects() {
        score = 0;
        timeElapsed = 0f;
        isEndScreenActive = false;
        isCoroutineStarted = false;
        StopAllCoroutines();
        road.transform.position = roadStartingPlace;
        //get all generated objects with script IncomingObjectMovement and destroy them
        IncomingObjectMovement[] incomingObjects = FindObjectsOfType<IncomingObjectMovement>();    
        SofaIncoming[] incomingUndestoyables = FindObjectsOfType<SofaIncoming>();
        foreach (IncomingObjectMovement obj in incomingObjects)
        {
            Destroy(obj.gameObject);
        }

        foreach (SofaIncoming obj2 in incomingUndestoyables)
        {
            Destroy(obj2.gameObject);
        }

    }


    private void Update()
    {
        if (Input.GetKey(KeyCode.R)) {
            timeElapsed = 60;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log(Time.timeScale);
        }
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= 62 && isEndScreenActive == false)
        {
            
            GameManager.instance.GameOverPanel();
            GameManager.instance.GameOver();
            isEndScreenActive=true;
        }
    }

    private IEnumerator SpowningObjects()
    {
        yield return new WaitForSeconds(waveSpownerTimer);
        randomNumberOfObjectsToSpown = Random.Range(1, 4); // Random number of objects to spawn (1 to 3)
        SetLinesToSpownObjects(randomNumberOfObjectsToSpown);
        
        if (timeElapsed < 50)
        {
            StartCoroutine(SpowningObjects());
        }
    }

    private void SetLinesToSpownObjects(int numberOfObjectsToSpown)
    {
        //ako se spowna jedan objekt
        if (numberOfObjectsToSpown == 1)
        {
            randomSpownLineIndex = Random.Range(-1, 2);
            randomChanceTOSpownCollectableOnTopOfObject = Random.Range(0, 5);

            if (randomChanceTOSpownCollectableOnTopOfObject == 0)
            {
                randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                randomCollectableSpown = Random.Range(0, collectables.Length);
                Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);
                Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y * 3, 5), collectables[randomCollectableSpown].transform.rotation);
            }
            else if (randomChanceTOSpownCollectableOnTopOfObject >= 1)
            {
                randomObjectListToPool = Random.Range(0, 2);
                if (randomObjectListToPool == 0) // bad object
                {
                    randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                    Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);

                }
                else // collectable
                {
                    randomCollectableSpown = Random.Range(0, collectables.Length);
                    Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y, 5), collectables[randomCollectableSpown].transform.rotation);
                }
            }
        }

        else if (numberOfObjectsToSpown == 2)
        {
            randomSpownLineIndex = Random.Range(-1, 2);
            randomSecondSpownLineIndex = Random.Range(-1, 2);
            while (randomSecondSpownLineIndex == randomSpownLineIndex)
            {
                randomSecondSpownLineIndex = Random.Range(-1, 2);
            }
            for (int firstSecondObject = 0; firstSecondObject < 2; firstSecondObject++)
            {
                randomChanceTOSpownCollectableOnTopOfObject = Random.Range(0, 5);
                if (randomChanceTOSpownCollectableOnTopOfObject == 0)
                {
                    Debug.Log("Spowning collectable on top of object");
                    randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                    randomCollectableSpown = Random.Range(0, collectables.Length);
                    if (firstSecondObject == 0)
                    {
                        Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);
                        Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y * 3, 5), collectables[randomCollectableSpown].transform.rotation);
                    }
                    else
                    {
                        Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSecondSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);
                        Instantiate(collectables[randomCollectableSpown], new Vector3(randomSecondSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y * 3, 5), collectables[randomCollectableSpown].transform.rotation);
                    }
                }
                else if (randomChanceTOSpownCollectableOnTopOfObject >= 1)
                {
                    Debug.Log("Spowning only one object");
                    randomObjectListToPool = Random.Range(0, 2);
                    if (randomObjectListToPool == 0) // bad object
                    {
                        randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                        if (firstSecondObject == 0)
                        {
                            Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);
                        }
                        else
                        {
                            Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(randomSecondSpownLineIndex * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);
                        }
                    }
                    else
                    {
                        randomCollectableSpown = Random.Range(0, collectables.Length);
                        if (firstSecondObject == 0)
                        {
                            Instantiate(collectables[randomCollectableSpown], new Vector3(randomSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y, 5), collectables[randomCollectableSpown].transform.rotation);
                        }
                        else
                        {
                            Instantiate(collectables[randomCollectableSpown], new Vector3(randomSecondSpownLineIndex * 3, collectables[randomCollectableSpown].transform.position.y, 5), collectables[randomCollectableSpown].transform.rotation);
                        }
                    }

                }
            }
        }

        else if (numberOfObjectsToSpown == 3)
        {
            for (int i = -1; i < 2; i++)
            {
                randomChanceTOSpownCollectableOnTopOfObject = Random.Range(0, 5);

                if (randomChanceTOSpownCollectableOnTopOfObject == 0)
                {
                    Debug.Log("Spowning collectable on top of object");
                    randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                    randomCollectableSpown = Random.Range(0, collectables.Length);
                    Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(i * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);
                    Instantiate(collectables[randomCollectableSpown], new Vector3(i * 3, collectables[randomCollectableSpown].transform.position.y * 3, 5), collectables[randomCollectableSpown].transform.rotation);
                }
                else if (randomChanceTOSpownCollectableOnTopOfObject >= 1)
                {
                    Debug.Log("Spowning only one object");
                    randomObjectListToPool = Random.Range(0, 2);
                    if (randomObjectListToPool == 0) // bad object
                    {
                        randomBadObjectToSpown = Random.Range(0, badObjectsToSpawn.Length);
                        Instantiate(badObjectsToSpawn[randomBadObjectToSpown], new Vector3(i * 3, badObjectsToSpawn[randomBadObjectToSpown].transform.position.y, 5), badObjectsToSpawn[randomBadObjectToSpown].transform.rotation);

                    }
                    else // collectable
                    {
                        randomCollectableSpown = Random.Range(0, collectables.Length);
                        Instantiate(collectables[randomCollectableSpown], new Vector3(i * 3, collectables[randomCollectableSpown].transform.position.y, 5), collectables[randomCollectableSpown].transform.rotation);
                    }
                }
            }
        }
    }

    /// <summary>
    /// score manipulation
    /// </summary>
    /// <param name="points"></param>
    // Method to add score
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }
    // Method to get current score
    public int GetScore()
    {
        return score;
    }
}
