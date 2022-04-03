using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WagonStatus : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, currentHealth, wagonHealthDecrease = 20f, wagonHealthIncrease = 5f;
    
    [SerializeField]
    private event Action onWagonBroken;
    
    [SerializeField]
    private List<GameObject> parts;

    public bool isBroken => currentHealth <= 0;

    public float MaxHealth => maxHealth;

    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        StartCoroutine(DecreasePartsStatus());
    }

    private IEnumerator DecreasePartsStatus()
    {
        while (true)
        {
            foreach (var status in parts.Select(part => part.GetComponent<PartStatus>()))
            {
                if (!status.isBroken)
                {
                    status.TryBreakPart();
                    IncreaseHealth(status.getHealthFactor);
                    continue; 
                }
                
                DecreaseHealth(status.getHealthFactor);
                
                if (isBroken)
                    onWagonBroken?.Invoke();
            }

            yield return new WaitForSeconds(2f);   
        }
    }

    private void DecreaseHealth(float decreaseFactor)
    {
        var newHealth = currentHealth - (wagonHealthDecrease * decreaseFactor);
        
        currentHealth = (currentHealth <= 0f) ? 0f : newHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    private void IncreaseHealth(float increaseFactor)
    {
        var newHealth = currentHealth + (wagonHealthIncrease * increaseFactor);
        
        currentHealth = (currentHealth <= maxHealth) ? maxHealth : newHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
}