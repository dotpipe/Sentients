```csharp
using UnityEngine;
using UnityEngine.Networking;

public class Scene : NetworkBehaviour
{
    private GameController gameController;
    private Map map;
    private Character character;

    // Constructor
    public Scene()
    {
        gameController = FindObjectOfType<GameController>();
        map = FindObjectOfType<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // Check if the round has started
        if (gameController.RoundStarted)
        {
            // Check if the character is defeated
            if (character.IsDefeated)
            {
                // If the character is defeated, disable its control
                DisableCharacterControl();
                
            }
            else
            {
                // If the character is not defeated, enable its control
                EnableCharacterControl();
            }
        }
        else
        {
            // If the round has not started, disable the character's control
            DisableCharacterControl();
        }
    }

    // Method to enable the character's control
    private void EnableCharacterControl()
    {
        character.enabled = true;
    }

    // Method to disable the character's control
    private void DisableCharacterControl()
    {
        character.enabled = false;
    }

    // Method to set the character's position based on the map
    public void SetCharacterPosition()
    {
        character.Position = map.GetSpawnPoint();
    }

    // Method to set the character's avatar
    public void SetCharacterAvatar(int avatarID)
    {
        character.AvatarID = avatarID;
    }

    // Method to set the character's fighting style
    public void SetCharacterStyle(int styleID)
    {
        character.StyleID = styleID;
    }
}
```
