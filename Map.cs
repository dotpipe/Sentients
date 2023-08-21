```csharp
using UnityEngine;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class Map : MonoBehaviour
{
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    // List of all map positions
    private List<Vector3> mapPositions;

    // Constructor
    public Map()
    {
        server = "localhost";
        database = "UnityGame";
        uid = "root";
        password = "password";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        connection = new MySqlConnection(connectionString);

        mapPositions = new List<Vector3>();
    }

    // Load map positions from database
    public void LoadMapPositions()
    {
        string query = "SELECT PositionX, PositionY, PositionZ FROM UserRound WHERE RoundID = @RoundID";

        if (OpenConnection())
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@RoundID", GameController.currentRound);

            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                float x = float.Parse(dataReader["PositionX"].ToString());
                float y = float.Parse(dataReader["PositionY"].ToString());
                float z = float.Parse(dataReader["PositionZ"].ToString());

                mapPositions.Add(new Vector3(x, y, z));
            }

            dataReader.Close();
            CloseConnection();
        }
    }

    // Open connection to the database
    private bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Debug.LogError("Cannot connect to server. Contact administrator");
                    break;

                case 1045:
                    Debug.LogError("Invalid username/password, please try again");
                    break;
            }
            return false;
        }
    }

    //Close connection
    private bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }
}
```
