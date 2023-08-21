using UnityEngine;
using System.Collections.Generic;

public class CloseCombatAttack
{
    private Character attacker;
    private Character target;

    public CloseCombatAttack(Character attacker, Character target)
    {
        this.attacker = attacker;
        this.target = target;
    }

    public void ExecuteAttack()
    {
        if (IsAttackPossible())
        {
            CalculateDamage();
            ApplyDamage();
            ApplyKnockback();
            CheckDefeat();
        }
    }

    private bool IsAttackPossible()
    {
        // Check if both attacker and target are in a valid state for an attack
        return !attacker.IsDefeated && !target.IsDefeated;
    }

    private void CalculateDamage()
    {
        // Calculate the damage based on attacker's strength and target's defense
        float damage = attacker.Strength - target.Defense;

        // Apply a random factor to the damage
        float randomFactor = Random.Range(0.8f, 1.2f);
        damage *= randomFactor;

        // Ensure the damage is positive
        damage = Mathf.Max(damage, 0);

        // Apply the damage to the target
        target.ApplyDamage(damage);
    }

    private void ApplyDamage()
    {
        // Apply visual effects or animations related to the attack
        // For example, play a hit animation or show blood splatter
        
    }

    private void ApplyKnockback()
    {
        // Calculate the knockback force based on attacker's strength and target's weight
        float knockbackForce = attacker.Strength * target.Weight;

        // Apply the knockback force to the target
        target.ApplyKnockback(knockbackForce);
    }

    private void CheckDefeat()
    {
        // Check if the target is defeated after the attack
        if (target.Health <= 0)
        {
            target.IsDefeated = true;
            attacker.PlayersDefeated++;
        }
    }
}