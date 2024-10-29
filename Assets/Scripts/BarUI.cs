using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class BarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image frozenBarImage;

    private IDamageable _hasIDamageable;

    private void Start()
    {
        _hasIDamageable = hasProgressGameObject.GetComponent<IDamageable>();
        if (_hasIDamageable == null)
        {
            Debug.Log("hasProgress is null");
            return;
        }
        _hasIDamageable.OnHealthChange += HasIDamageableOnHealthChanged;
        _hasIDamageable.OnFrozenProgressChange += HasIDamageableOnFrozenChanged;
        healthBarImage.fillAmount = 1;
        frozenBarImage.fillAmount = 0;
        Show();
    }
    
    private void HasIDamageableOnFrozenChanged(object sender, IDamageable.OnFrozenProgressChangedEventArgs e)
    {
        frozenBarImage.fillAmount = e.frozenProgressNormalized;
    }

    private void HasIDamageableOnHealthChanged(object sender, IDamageable.OnHealthChangedEventArgs e)
    {
        healthBarImage.fillAmount = e.healthNormalized;
        if (e.healthNormalized == 0f)
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