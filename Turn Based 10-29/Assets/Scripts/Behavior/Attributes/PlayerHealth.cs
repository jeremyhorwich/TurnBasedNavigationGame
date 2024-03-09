using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public static event Action<int, int> OnPlayerHealthChange;

    [SerializeField] private int maxHealth;
    private int health;

    public int Health { get { return health; } }


    private void Awake()
    {
        health = maxHealth;
    }

    private void Start()
    {
        OnPlayerHealthChange?.Invoke(health, maxHealth);
    }

    public void ChangeHealth(Func<int, int> changeType)
    {
        health = Mathf.Clamp(changeType(health), 0, maxHealth);
        if (health <= 0)
            MakeDead();
        OnPlayerHealthChange?.Invoke(health, maxHealth);
    }

    public void MakeDead()
    {
        gameObject.SetActive(false);
        Debug.Log("Player has been killed");
    }
}
