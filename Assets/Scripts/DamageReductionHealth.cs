using System;
using UnityEngine;

public class DamageReductionHealth : MonoBehaviour, IHealth {
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private int reductionAmount = 5;
    private int currentHealth;

    public event Action<float> OnHPPctChanged = delegate {};
    public event Action OnDied = delegate{};

    private void Start() {
        currentHealth = startingHealth;
    }

    public float CurrentHpPct {
        get{return (float) currentHealth / (float) startingHealth;}
    }

    public void TakeDamage(int amount) {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

        if (amount > reductionAmount) {
            amount -= reductionAmount;
            currentHealth -= amount;

            OnHPPctChanged(CurrentHpPct);

            if (CurrentHpPct <= 0)
                Die();    
        }

        
    }

    private void Die() {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }
}