
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MLAPI
{
    private Server server;
    private List<Character> characters;
    private List<FightingStyle> styles;
    private int roundTime;
    private bool roundStarted;
    private int defeatedCount;

    // Constructor
    public GameController()
    {
        server = new Server();
        characters = new List<Character>();
        styles = new List<FightingStyle>();
        roundTime = 25 * 60; // 25 minutes in seconds
        roundStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create MySQL database connection
        MySqlConnection connection = new MySqlConnection("server=localhost;user=root;database=mydatabase;password=mypassword");
        connection.Open();

        // Get styles from the database
        MySqlCommand styleCommand = new MySqlCommand("SELECT * FROM styles", connection);
        MySqlDataReader styleReader = styleCommand.ExecuteReader();
        while (styleReader.Read())
        {
            int styleId = styleReader.GetInt32("id");
            string styleName = styleReader.GetString("name");
            FightingStyle style = new FightingStyle(styleId, styleName);
            styles.Add(style);
        }
        styleReader.Close();

        // Get characters from the database
        MySqlCommand characterCommand = new MySqlCommand("SELECT * FROM characters", connection);
        MySqlDataReader characterReader = characterCommand.ExecuteReader();
        while (characterReader.Read())
        {
            int characterId = characterReader.GetInt32("id");
            string characterName = characterReader.GetString("name");
            Character character = new Character(characterId, characterName);
            characters.Add(character);
        }
        characterReader.Close();

        // Close the database connection
        connection.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (!roundStarted && server.ConnectedUsers.Count >= 2)
        {
            StartRound();
        }
        else if (roundStarted)
        {
            roundTime -= Time.deltaTime;
            if (roundTime <= 0 || AllButOneDefeated())
            {
                EndRound();
            }
        }
    }

    // Start a new round
    private void StartRound()
    {
        roundStarted = true;
        Character me = new Character();
        me.Health = 100;
        me.ResetHealthBar();
        me.LoadMapPositions();
        me.SetCharacterPosition();
        // TODO: Send the start round message to all connected users
    }

    // End the current round
    private void EndRound()
    {
        roundStarted = false;
        roundTime = 25 * 60;
        // TODO: Determine the winner and update their skill level
        // TODO: Send the end round message to all connected users
    }

    // Check if all but one characters are defeated
    private bool AllButOneDefeated()
    {
        int defeatedCount = 0;
        foreach (Character character in characters)
        {
            if (character.IsDefeated)
            {
                defeatedCount++;
            }
        }
        return defeatedCount >= characters.Count - 1;
    }

}

