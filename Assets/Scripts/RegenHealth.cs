using System;
using System.Collections;
using UnityEngine;

public class RegenHealth : MonoBehaviour, IHealth {
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private int regenAmount = 5;
    [SerializeField] private int regenCooldown = 1;
    private int currentHealth;

    public event Action<float> OnHPPctChanged = delegate {};
    public event Action OnDied = delegate{};

    private void Start() {
        currentHealth = startingHealth;
        StartCoroutine(RegenTimer());
    }

    public float CurrentHpPct {
        get{return (float) currentHealth / (float) startingHealth;}
    }

    public void TakeDamage(int amount) {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("Invalid Damage amount specified: " + amount);

        currentHealth -= amount;

        OnHPPctChanged(CurrentHpPct);

        if (CurrentHpPct <= 0)
            Die();
    }

    private void Heal(int amount)
    {
        if(currentHealth+amount > startingHealth) {
            currentHealth+=startingHealth-currentHealth;
        } else {
            currentHealth+=amount;
        }
    }

    private IEnumerator RegenTimer() {
        while(currentHealth != startingHealth) {
            if(currentHealth <= 0) {
                Die();
                break;
            }
            yield return new WaitForSeconds(regenCooldown);
            Heal(regenAmount);
        }
    }

    private void Die() {
        OnDied();
        GameObject.Destroy(this.gameObject);
    }
}