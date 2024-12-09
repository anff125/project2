using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private Transform Boss;
    [SerializeField] private Transform Ending;
    [SerializeField] private Transform Victory;
    [SerializeField] private AudioSource audioSource;
    public List<Transform> spawnedEnemies = new List<Transform>();
    private bool Endingshow = false;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Boss == null && Endingshow == false)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] != null)
                {
                    spawnedEnemies[i].gameObject.SetActive(false);
                }
            }

            StartCoroutine(ShowVictory());
        }
    }

    public void TrackSpawnedEnemies(Transform enemy)
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
        spawnedEnemies.Add(enemy);
    }
    
    IEnumerator ShowEnding()
    {
        yield return new WaitForSeconds(2);
        Endingshow = true;
        Ending.gameObject.SetActive(true);
        audioSource.Stop();
    }

    IEnumerator ShowVictory()
    {
        yield return new WaitForSeconds(2);
        Endingshow = true;
        Victory.gameObject.SetActive(true);
        audioSource.Stop();
    }
}