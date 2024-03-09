using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HealthTextDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;

    private void Awake()
    {
        PlayerHealth.OnPlayerHealthChange += UpdateHealthTextDisplay;
    }

    private void UpdateHealthTextDisplay(int healthAmount, int maxHealthAmount)
    {
        healthText.text = "Health: " + healthAmount.ToString() + "/" + maxHealthAmount.ToString();
    }
}
