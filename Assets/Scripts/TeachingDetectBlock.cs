using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeachingDetectBlock : MonoBehaviour
{
    [SerializeField] private Transform text;
    [SerializeField] private String level;
    [SerializeField] private bool forNextLevel = false;
    private bool isPlayerIn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //set activate
            text.gameObject.SetActive(true);
            isPlayerIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
            isPlayerIn = false;
        }
    }
    private void Update()
    {
        if (isPlayerIn && forNextLevel && Input.GetKeyDown(KeyCode.F))
        {
            //load next level
            SceneLoader.Instance.LoadLevel(level);
        }
    }
}