```csharp
using UnityEngine;
using System.Collections;

public class Meditation : MonoBehaviour
{
    public Character character; // Reference to the character
    public float meditationRate = 5f; // The rate at which health is restored during meditation
    public float maxHealth = 100f; // The maximum health a character can have

    // Start is called before the first frame update
    void Start()
    {
        // Nothing to initialize
    }

    // Update is called once per frame
    void Update()
    {
        // If the character is meditating and not at max health, restore health
        if (character.IsMeditating && character.Health < maxHealth)
        {
            RestoreHealth();
            
            // Create instance of MeditationGFX
            MeditationGFX meditationGFX = new MeditationGFX();
            // Call Update on MeditationGFX each frame from Update in the Meditation class
            meditationGFX.Update();
        }
    }

    // Restore health to the character
    private void RestoreHealth()
    {
        character.Health += meditationRate * Time.deltaTime;

        // Ensure health does not exceed max health
        if (character.Health > maxHealth)
        {
            
            character.Health = maxHealth;
        }
    }
    // Start the meditation process
    public void StartMeditating()
    {
        character.IsMeditating = true;
        if (character.Health == 100)
        {
            character.CreateFlamingBall();
        }
        InvokeRepeating("Update", 0f, 5f);
    }
    
    // Start the meditation process
    public void StartMeditation()
    {
        character.StartMeditating();
    }

    // Stop the meditation process
    public void StopMeditation()
    {
        character.StopMeditating();
    }
}
```
