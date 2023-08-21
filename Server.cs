```csharp
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkManager
{
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;

    // Maximum number of connections
    public const int MAX_CONNECTIONS = 25;

    // List of connected users
    private List<NetworkConnection> connectedUsers;

    // Queue of waiting users
    private Queue<NetworkConnection> waitingUsers;

    // Current round
    private int currentRound;

    // Constructor
    public Server()
    {
        server = "localhost";
        database = "UnityGame";
        uid = "root";
        password = "password";
        string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        connection = new MySqlConnection(connectionString);

        connectedUsers = new List<NetworkConnection>();
        waitingUsers = new Queue<NetworkConnection>();
    }

    // When a client connects
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (connectedUsers.Count < MAX_CONNECTIONS)
        {
            connectedUsers.Add(conn);
            Debug.Log("User connected. Total users: " + connectedUsers.Count);
        }
        else
        {
            waitingUsers.Enqueue(conn);
            Debug.Log("User waiting. Total waiting users: " + waitingUsers.Count);
        }
    }

    // When a client disconnects
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        connectedUsers.Remove(conn);
        Debug.Log("User disconnected. Total users: " + connectedUsers.Count);

        if (waitingUsers.Count > 0)
        {
            NetworkConnection nextUser = waitingUsers.Dequeue();
            connectedUsers.Add(nextUser);
            Debug.Log("User from queue connected. Total users: " + connectedUsers.Count);
        }
    }

    // Start a new round
    public void StartRound()
    {
        if (connectedUsers.Count >= 2)
        {
            currentRound++;
            Debug.Log("Round started.");
        }
        else
        {
            Debug.Log("Not enough users to start a round.");
        }
    }

    // End the current round
    public void EndRound()
    {
        Debug.Log("Round ended.");
    }

}
```

