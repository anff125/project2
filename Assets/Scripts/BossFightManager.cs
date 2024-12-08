using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private Transform Boss;
    [SerializeField] private Transform Ending;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool forNextLevel;
    [SerializeField] private string level;
    private bool Endingshow = false;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Boss == null && Endingshow == false)
        {
            if (forNextLevel)
            {
                //load next level
                SceneLoader.Instance.LoadLevel(level);
            }
            else
            {
                StartCoroutine(ShowEnding());
            }
        }
    }
    
    IEnumerator ShowEnding()
    {
        yield return new WaitForSeconds(2);
        Endingshow = true;
        Ending.gameObject.SetActive(true);
        audioSource.Stop();
    }
}