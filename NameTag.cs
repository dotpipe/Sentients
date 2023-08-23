using UnityEngine;

public class NameTag : MonoBehaviour
{
    public string playerName;
    public Texture2D nameTagTexture;
    public Font nameTagFont;
    public Color nameTagColor;
    public float nameTagHeight = 2f;

    private void OnGUI()
    {
        // Convert the character's position to screen coordinates
        Vector3 characterPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(characterPosition);

        // Calculate the position of the name tag above the character's head
        float nameTagX = screenPosition.x - (nameTagTexture.width / 2);
        float nameTagY = Screen.height - screenPosition.y - nameTagHeight;

        // Set the GUI style for the name tag
        GUIStyle style = new GUIStyle();
        style.normal.background = nameTagTexture;
        style.font = nameTagFont;
        style.fontSize = 16;
        style.normal.textColor = nameTagColor;

        // Draw the name tag above the character's head
        GUI.Label(new Rect(nameTagX, nameTagY, nameTagTexture.width, nameTagTexture.height), playerName, style);
    }
}