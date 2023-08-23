
using UnityEngine;

public class Striking : MonoBehaviour
{
    private Character character;
    private FightingStyle style;
    private float strikeCooldown = 0;

    void Start()
    {
        character = GetComponent<Character>();
    }

    void Update()
    {
        if (character.IsDefeated)
        {
            return;
        }

        if (strikeCooldown > 0)
        {
            strikeCooldown -= Time.deltaTime;
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            Strike();
        }
    }

    private void Strike()
    {
        style = GetComponent<FightingStyle>();

        // Calculate the strike range based on the character's fighting style
        float strikeRange = style.CalculateRange();

        // Check for characters within the strike range
        Collider[] hitCharacters = Physics.OverlapSphere(character.Position, strikeRange);

        foreach (Collider hitCharacter in hitCharacters)
        {
            Character target = hitCharacter.GetComponent<Character>();

            // Ignore the character itself and defeated characters
            if (target == null || target == character || target.IsDefeated)
            {
                continue;
            }

            // Calculate the damage based on the character's fighting style
            float damage = style.CalculateDamage();

            // Apply the damage to the target character
            target.ApplyDamage(damage);
        }

        // Set the strike cooldown based on the character's fighting style speed
        strikeCooldown = 1 / style.Speed;
    }
}

