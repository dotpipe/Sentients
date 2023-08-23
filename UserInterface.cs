
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class UserInterface : MonoBehaviour
{
    public Text timerText; // Reference to the Timer UI component
    public Text roundText; // Reference to the Round UI component
    public Text queueText; // Reference to the Queue UI component
    public GameController gameController; // Reference to the GameController


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the UI components
        timerText.text = "Time: " + gameController.roundTime.ToString();
        roundText.text = "Round: " + gameController.currentRound.ToString();
        queueText.text = "Queue: " + gameController.waitingUsers.Count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the UI components
        timerText.text = "Time: " + gameController.roundTime.ToString();
        roundText.text = "Round: " + gameController.currentRound.ToString();
        queueText.text = "Queue: " + gameController.waitingUsers.Count.ToString();
    }

    // Method to display a message to the user
    public void DisplayMessage(string message)
    {
        StartCoroutine(DisplayMessageWithTransparency(message));
    }

    private IEnumerator DisplayMessageWithTransparency(string message)
    {
        // Create transparency
        Image transparency = Instantiate(gameController.transparencyPrefab, transform);
        transparency.color = new Color(1f, 1f, 1f, 0.5f);

        // Write message on transparency
        Text messageText = Instantiate(gameController.messagePrefab, transform);
        messageText.text = message;
        messageText.transform.SetParent(transparency.transform);

        // Wait for 3.5 seconds
        yield return new WaitForSeconds(3.5f);

        // Remove transparency
        Destroy(transparency.gameObject);
    }
    
    // Method to update the character's avatar
    public void UpdateAvatar(Character character)
    {
        // Create transparency
        Image transparency = Instantiate(gameController.transparencyPrefab, transform);
        transparency.color = new Color(1f, 1f, 1f, 0.5f);

        // Create 3D renderer for character avatar
        GameObject avatar = Instantiate(gameController.avatarPrefab, transform);
        avatar.transform.SetParent(transparency.transform);
        avatar.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f); // Scale down the avatar by 65%

        // Add button component to change avatar
        Button changeAvatarButton = avatar.AddComponent<Button>();
        changeAvatarButton.onClick.AddListener(ChangeAvatar);

        void ChangeAvatar()
        {
            int nextAvatarIndex = (character.AvatarID + 1) % gameController.availableAvatars.Length;
            character.AvatarID = nextAvatarIndex;
            avatar.GetComponent<Renderer>().material.mainTexture = gameController.availableAvatars[nextAvatarIndex];
        }

        // Write message on transparency
        Text messageText = Instantiate(gameController.messagePrefab, transform);
        messageText.text = message;
        messageText.transform.SetParent(transparency.transform);

        // Wait for 3.5 seconds
        yield return new WaitForSeconds(3.5f);

    }

    // Method to update the character's fighting style
    public void UpdateFightingStyle(Character character)
    {
        // Get the fighting style from the database
        string fightingStyle = GetFightingStyleFromDatabase(character.StyleID);

        // Create transparency
        Image transparency = Instantiate(gameController.transparencyPrefab, transform);
        transparency.color = new Color(1f, 1f, 1f, 0.5f);

        // Write fighting style on transparency
        Text fightingStyleText = Instantiate(gameController.messagePrefab, transform);
        fightingStyleText.text = fightingStyle;
        fightingStyleText.transform.SetParent(transparency.transform);

        // Create 3D renderer for character avatar
        GameObject avatar = Instantiate(gameController.avatarPrefab, transform);
        avatar.transform.SetParent(transparency.transform);
        avatar.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f); // Scale down the avatar by 65%

        // Add button components to change fighting style
        Button previousStyleButton = avatar.AddComponent<Button>();
        previousStyleButton.onClick.AddListener(PreviousStyle);
        Button nextStyleButton = avatar.AddComponent<Button>();
        nextStyleButton.onClick.AddListener(NextStyle);

        void PreviousStyle()
        {
            int previousStyleID = (character.StyleID - 1) % totalStyles;
            if (previousStyleID < 0)
            {
                previousStyleID += totalStyles;
            }
            character.StyleID = previousStyleID;
            string previousFightingStyle = GetFightingStyleFromDatabase(previousStyleID);
            fightingStyleText.text = previousFightingStyle;
        }

        void NextStyle()
        {
            int nextStyleID = (character.StyleID + 1) % totalStyles;
            character.StyleID = nextStyleID;
            string nextFightingStyle = GetFightingStyleFromDatabase(nextStyleID);
            fightingStyleText.text = nextFightingStyle;
        }

        // Wait for 3.5 seconds
        yield return new WaitForSeconds(3.5f);

    }
    private string GetFightingStyleFromDatabase(int styleID)
    {
        // TODO: Implement code to get the fighting style from the database
        string fightingStyle = ""; // Placeholder for the actual fighting style
    
        // YOUR_CODE
        fightingStyle = Database.GetFightingStyle(styleID);
    
        return fightingStyle;
    }

}

