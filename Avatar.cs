
using UnityEngine;
using UnityEngine.Networking;

public class Avatar : MLAPI
{
    public int AvatarID { get; set; }
    public string AvatarName { get; set; }
    public string Skin { get; set; }
    // List to store the sprite skins
    public List<Sprite> SkinSprites { get; set; }
    public Character Character { get; set; }

    // Method to load the sprite skins from the asset files
    public void LoadSkinSprites()
    {
        SkinSprites = new List<Sprite>();
        Sprite[] loadedSkins = Resources.LoadAll<Sprite>("Skins");
        foreach (Sprite skin in loadedSkins)
        {
            SkinSprites.Add(skin);
        }
    }
    
    // Constructor
    public Avatar(int avatarID, string avatarName, string skin, Character character)
    {
        AvatarID = avatarID;
        AvatarName = avatarName;
        Skin = skin;
        Character = character;
    }

    // Method to attach skin to the avatar
    public void AttachSkin(string skinName)
    {
        // Load the GUISkin from the Assets using the provided skinName
        GUISkin skin = Resources.Load<GUISkin>("Skins/" + skinName);

        // Attach the GUISkin to the avatar
        GetComponent<GUISkin>().skin = skin;

    }

    // Method to change the avatar's character
    public void ChangeCharacter(Character newCharacter)
    {
        Character = newCharacter;
        // TODO: Implement the logic to change the avatar's character in the game
    }

    // Method to update the avatar's position based on the character's position
    public void UpdatePosition()
    {
        // The avatar's position is always the same as the character's position
        transform.position = Character.Position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
}

