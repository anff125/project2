using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathCountUI : MonoBehaviour
{
    [SerializeField] TMP_Text deathCountText;
    private int deathCount = 0;
    private void Start()
    {
        Player.Instance.OnDeath += OnDeathCountChanged;
    }
    private void OnDeathCountChanged(object sender, EventArgs e)
    {
        deathCountText.text = "Death Count: " + deathCount++;

    }

}