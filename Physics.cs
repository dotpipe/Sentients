```csharp
using UnityEngine;

public class Physics : MonoBehaviour
{
    private const float GRAVITY = -9.81f;
    private const float TERMINAL_VELOCITY = -50f;

    private Character character;
    private Rigidbody rb;

    void Start()
    {
        character = GetComponent<Character>();
        rb = GetComponent<Rigidbody>();
    }



    void Update()
    {
        ApplyGravity();

        if (character.IsDefeated)
        {
            ApplyRagdollPhysics();
        }
    }

    private void ApplyGravity()
    {
        if (rb.velocity.y > TERMINAL_VELOCITY)
        {
            rb.AddForce(new Vector3(0, GRAVITY * rb.mass, 0));
        }
    }

    private void ApplyRagdollPhysics()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        character.IsDefeated = true;
    }

    public void ApplyStrikeForce(Vector3 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    public void ApplyFallDamage()
    {
        float fallSpeed = Mathf.Abs(rb.velocity.y);
        if (fallSpeed > TERMINAL_VELOCITY)
        {
            character.ApplyDamage(fallSpeed);
        }
    }

    public void ApplyMeditationForce()
    {
        // Make all characters who are meditating 35% less damaged than normal
        float damageMultiplier = character.IsMeditating ? 0.65f : 1f;

        // Use graphics to create fire around meditating players with 100% life
        if (character.Health == 100 && character.IsMeditating)
        {
            CreateFlamingBall();
        }
        else
        {
            ToggleFlamingBall(false);
        }
        // Check if any players are within the fire sphere
        List<Character> charactersInFireSphere = GetCharactersInFireSphere();
        foreach (Character characterInSphere in charactersInFireSphere)
        {
            // Any players who are within this sphere are instantly struck with 40% damage and pushed out of the sphere
            characterInSphere.ApplyDamage(characterInSphere.MaxHealth * 0.4f);
            PushCharacterOutOfSphere(character characterInSphere);
        }
    }

    public void CreateFlamingBall()
    {
        // Get the center position of the character
        Vector3 centerPosition = character.transform.position;

        // Generate a random radius for the flaming ball
        float radius = Random.Range(1f, 5f);

        // Create a sphere collider for the flaming ball
        GameObject flamingBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flamingBall.transform.position = centerPosition;
        flamingBall.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);

        // Add a particle system component to the flaming ball
        ParticleSystem particleSystem = flamingBall.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startColor = Color.red;
        mainModule.startSize = radius;

        // Set the particle system to loop and play
        particleSystem.loop = true;
        particleSystem.Play();
    }

    public void ToggleFlamingBall(bool enable)
    {
        if (enable)
        {
            CreateFlamingBall();
        }
        else
        {
            // Find and destroy the flaming ball object
            GameObject flamingBall = GameObject.FindWithTag("FlamingBall");
            if (flamingBall != null)
            {
                Destroy(flamingBall);
            }
        }
    }
    
    public void PushCharacterOutOfSphere(Character character, Character characterInSphere)
    {
        float distance = Vector3.Distance(character.transform.position, characterInSphere.transform.position);
        Vector3 direction = (characterInSphere.transform.position - character.transform.position).normalized;
        characterInSphere.transform.position += direction * distance;

    }
}
```

