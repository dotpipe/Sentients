```csharp
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Reference to the Slider UI component
    public Character character; // Reference to the character

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the health bar with the character's current health
        slider.value = character.Health;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar with the character's current health
        slider.value = character.Health;

        // Position the health bar above the character's head
        Vector3 healthBarPosition = Camera.main.WorldToScreenPoint(character.Position);
        healthBarPosition.y += 60; // Offset to position the health bar above the character's head
        transform.position = healthBarPosition;

        // Hide the health bar if the character is defeated
        if (character.CheckDefeat())
        {
            gameObject.SetActive(false);
        }
    }

    // Reset the health bar for the next round
    public void ResetHealthBar()
    {
        slider.value = 100; // Reset the health bar to 100
        gameObject.SetActive(true); // Make the health bar visible again
    }
}
```

