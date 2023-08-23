
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Queue : MonoBehaviour
{
    private Server server;
    private List<NetworkConnection> waitingUsers;

    // Constructor
    public Queue()
    {
        server = new Server();
        waitingUsers = new List<NetworkConnection>();
    }

    // Method to add a user to the queue
    public void EnqueueUser(NetworkConnection user)
    {
        waitingUsers.Add(user);
        Debug.Log("User added to queue. Total waiting users: " + waitingUsers.Count);
    }

    // Method to remove a user from the queue
    public NetworkConnection DequeueUser()
    {
        if (waitingUsers.Count > 0)
        {
            NetworkConnection user = waitingUsers[0];
            waitingUsers.RemoveAt(0);
            Debug.Log("User removed from queue. Total waiting users: " + waitingUsers.Count);
            return user;
        }
        else
        {
            Debug.Log("No users in queue.");
            return null;
        }
    }

    // Method to get the number of users in the queue
    public int GetQueueCount()
    {
        return waitingUsers.Count;
    }

    // Method to clear the queue
    public void ClearQueue()
    {
        waitingUsers.Clear();
        Debug.Log("Queue cleared.");
    }
}

