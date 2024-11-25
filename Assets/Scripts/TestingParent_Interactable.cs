using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TestingParent_Interactable : TestingParent
{
    [SerializeField] private TMP_Text _text; // Text to display when interacted with
    private bool _isPlayerNearby; // Flag to check if the player is nearby
    protected override void Start()
    {
        _text.gameObject.SetActive(false);
        GameInput.Instance.OnInteract += Interact;
    }
    private void Interact(object sender, EventArgs e)
    {
        SpawnChildOnce();
    }

    // Continuously attempts to spawn a child every second if _childTransform is null
    private void SpawnChildOnce()
    {
        if (_childTransform == null && _isPlayerNearby)
        {
            _childTransform = Instantiate(childPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    //on collider enter, display text
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = true;
            _text.gameObject.SetActive(true);
        }
    }
    //on collider exit, hide text
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerNearby = false;
            _text.gameObject.SetActive(false);
        }
    }
}