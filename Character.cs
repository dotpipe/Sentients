```csharp
using UnityEngine;
using UnityEngine.Networking;

public class Character : NetworkBehaviour
{
    public float ServerSyncClock { get; set; }
    public int AvatarID { get; set; }
    public string UserName { get; set; }
    public float Health { get; set; }
    public Vector3 Position { get; set; }
    public int StyleID { get; set; }
    public bool IsMeditating { get; set; }
    public bool IsDefeated { get; set; }
    public string Style { get; set; }
    public int Rank { get; set; }
    public float swingForce { get; set; }
    public float nominalDamage { get; set; }
    public float damage { get; set; }
    public float attackRange { get; set; }
    public float baseDamage { get; set; }
    
    // Constructor
    public Character(int avatarID, string userName, float health, Vector3 position, int styleID)
    {
        AvatarID = avatarID;
        UserName = userName;
        Health = health;
        Position = position;
        StyleID = styleID;
        Rank = 0;
        IsMeditating = false;
        IsDefeated = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Create a new thread to handle the update loop
        new Thread(() =>
        {
            RpcKeepPortOpen();
            while (true)
            {

                // Update the ServerSyncClock with the curernt sync time
                ServerSyncClock += 0.5;

                // Get the last 450 ms of movements
                var movements = GetLastMovements(450);
                
                // Convert the movements to JSON
                string json = JsonUtility.ToJson(movements);

                // Send the JSON to the server
                NetworkManager.singleton.client.Send(MsgType.Update, new StringMessage(json));

                // Update the character's traits globally
                RpcUpdateTraits();

                // Wait for the next update
                Thread.Sleep(450);

            }
        }).Start();
    }

    // Method to keep port 2024 open for transmitting character information
    [ClientRpc]
    private void RpcKeepPortOpen()
    {
        NetworkManager.singleton.networkPort = 2024;
        NetworkManager.singleton.StartClient();
    }

    // Method to update the character's traits globally
    [ClientRpc]
    private void RpcUpdateTraits()
    {
        // Get the GameController instance
        GameController gameController = GameController.Instance;
        // Get the GameController instance
        GameController gameController = GameController.Instance;

        // Check if character has been defeated in the last 450 ms
        if (gameController.GetLastMovements(450).Any(movement => movement.IsDefeat))
        {
            // Update defeat count
            gameController.UpdateDefeatCount();
        }
        
        // Get the last 450 ms of movements from the GameController
        List<Movement> movements = gameController.GetLastMovements(450);

        // Return the movements
        return movements;
    }

    // Apply damage to the character
    public void ApplyDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            IsDefeated = true;
        }
    }

    // Start meditating
    public void StartMeditating()
    {
        IsMeditating = true;
    }

    // Stop meditating
    public void StopMeditating()
    {
        IsMeditating = false;
    }

    // Check if the character is defeated
    public bool CheckDefeat()
    {
        return IsDefeated;
    }

    // Reset the character for the next round
    public void ResetCharacter()
    {
        Health = 100; // Reset health to 100
        IsDefeated = false;
        IsMeditating = false;
    }

    // Other properties and methods...

    public void Attack(Avatar target)
    {
        // Attack logic...
        // Turn towards closest position of target from Avatar class on the map
        Vector3 directionToTarget = target.Position - this.Position;
        Quaternion toTarget = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, toTarget, Time.deltaTime * 5);

        // Swing rigidbody random arm from damage class towards 'target'
        Rigidbody arm = AttackWithRandomLimb(); // AttackWithRandomLimb() is a method that returns a random arm's Rigidbody
        arm.AddForce(directionToTarget.normalized * swingForce); // swingForce is a predefined force for the swing

        // If 'target' is in sphere, 'target' receives damage unless button 'b' is held
        float distanceToTarget = Vector3.Distance(this.Position, target.Position);
        if (distanceToTarget <= attackRange) // attackRange is the radius of the sphere in which the target can be attacked
        {
            if (Input.GetKey(KeyCode.B))
            {
                // If 'b' is held damage is nominal and 'target' gets to strike immediately
                target.ApplyDamage(nominalDamage); // nominalDamage is a predefined nominal damage value
                this.ReceiveStrike(); // Assuming ReceiveStrike() is a method that makes the character receive a strike immediately
            }
            else
            {
                target.ApplyDamage(damage); // damage is a predefined damage value
            }
        }
        
    }
    // Method to generate a random seed based on the global in-game clock
    public int GetRandomSeed()
    {
        // Get the current in-game time
        float inGameTime = Time.time;

        // Convert the in-game time to an integer
        int inGameTimeInt = Mathf.FloorToInt(inGameTime);

        // Use the in-game time as a seed for the random number generator
        Random.InitState(inGameTimeInt);

        // Generate a random number
        int randomSeed = Random.Range(0, 4); // there are 4 limbs: 0 - left arm, 1 - right arm, 2 - left leg, 3 - right leg

        // Return the random seed
        return randomSeed;
    }

    // Method to attack with a limb based on the random seed
    public void AttackWithRandomLimb(Avatar target)
    {
        // Get a random seed
        int randomSeed = GetRandomSeed();

        // Decide which limb to attack with based on the random seed
        Rigidbody limb;
        switch (randomSeed)
        {
            case 0:
                limb = leftArm; // leftArm is a Rigidbody representing the left arm
                break;
            case 1:
                limb = rightArm; // rightArm is a Rigidbody representing the right arm
                break;
            case 2:
                limb = leftLeg; // leftLeg is a Rigidbody representing the left leg
                break;
            case 3:
                limb = rightLeg; // rightLeg is a Rigidbody representing the right leg
                break;
            default:
                limb = null;
                break;
        }

        // If a limb was selected, attack with it
        if (limb != null)
        {
            // Swing the limb towards the target
            Vector3 directionToTarget = target.Position - this.Position;
            limb.AddForce(directionToTarget.normalized * swingForce); // swingForce is a predefined force for the swing

            // If the target is in range, apply damage
            float distanceToTarget = Vector3.Distance(this.Position, target.Position);
            if (distanceToTarget <= attackRange) // attackRange is the radius of the sphere in which the target can be attacked
            {
                // Calculate the damage based on the offset from the rhythmic meta
                float offsetFromRhythmicMeta = Mathf.Abs(inGameTime - Mathf.Round(inGameTime));
                float damage = baseDamage * (1 - offsetFromRhythmicMeta); // baseDamage is the base damage value

                // Apply the damage to the target
                target.ApplyDamage(damage);
            }
        }
    }

    public void Block()
    {
        // Block logic...
        {
            // Block logic...
            // Use the same logic as AttackWithRandomLimb() but raise the opposite limb vertically
            Rigidbody oppositeLimb;
            switch (randomSeed)
            {
                case 0:
                    oppositeLimb = rightArm; // rightArm is a Rigidbody representing the right arm
                    break;
                case 1:
                    oppositeLimb = leftArm; // leftArm is a Rigidbody representing the left arm
                    break;
                case 2:
                    oppositeLimb = rightLeg; // rightLeg is a Rigidbody representing the right leg
                    break;
                case 3:
                    oppositeLimb = leftLeg; // leftLeg is a Rigidbody representing the left leg
                    break;
                default:
                    oppositeLimb = null;
                    break;
            }

            // If an opposite limb was selected, raise it vertically
            if (oppositeLimb != null)
            {
                // Raise the opposite limb vertically
                oppositeLimb.AddForce(Vector3.up * swingForce); // swingForce is a predefined force for the swing

                // If the target is in range, apply damage with decreasing synchronicity to the clock
                float distanceToTarget = Vector3.Distance(this.Position, target.Position);
                if (distanceToTarget <= attackRange) // attackRange is the radius of the sphere in which the target can be attacked
                {
                    // Calculate the damage based on the offset from the rhythmic meta
                    float offsetFromRhythmicMeta = Mathf.Abs(inGameTime - Mathf.Round(inGameTime));
                    float damage = baseDamage * (1 - offsetFromRhythmicMeta); // baseDamage is the base damage value

                    // Apply the damage to the target with decreasing synchronicity to the clock
                    target.ApplyDamage(damage * (1 - offsetFromRhythmicMeta));
                }
            }
        }
        
    }
}
```

