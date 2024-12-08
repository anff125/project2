using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private Transform Boss;
    [SerializeField] private Transform Ending;
    [SerializeField]private AudioSource audioSource;
    private bool Endingshow = false;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Boss == null && Endingshow == false)
        {
            StartCoroutine(ShowEnding());
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