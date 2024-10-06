using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image progressBarImage;

    private IDamageable _hasHealth;

    private void Start()
    {
        _hasHealth = hasProgressGameObject.GetComponent<IDamageable>();
        if (_hasHealth == null)
        {
            Debug.LogError("hasProgress is null");
            return;
        }
        _hasHealth.OnHealthChange += HasHealth_OnHealthChanged;
        progressBarImage.fillAmount = 1;
        Show();
    }

    private void HasHealth_OnHealthChanged(object sender, IDamageable.OnHealthChangedEventArgs e)
    {
        progressBarImage.fillAmount = e.healthNormalized;
        if (e.healthNormalized == 0f || Mathf.Approximately(e.healthNormalized, 1f))
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}