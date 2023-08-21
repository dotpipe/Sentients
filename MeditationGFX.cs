using UnityEngine;
using System.Collections.Generic;

public class LightningMeditation : Meditation
{
    public List<Vector3> chakraPositions; // Positions of the chakra points on the character

    // Start is called before the first frame update
    void Start()
    {
        // Initialize chakra positions
        InitializeChakraPositions();
    }

    // Update is called once per frame
    void Update()
    {
        // Call the base Update method from the Meditation class
        base.Update();

        // If the character is meditating, create lightning strikes between chakra points
        if (character.IsMeditating)
        {
            CreateLightningStrikes();
        }
    }

    // Initialize the positions of the chakra points on the character
    private void InitializeChakraPositions()
    {
        // Add the positions of the chakra points to the chakraPositions list
        // You can look up the specific positions of chakra points in the Hindu religion and assign them here
    }

    // Create lightning strikes between chakra points
    private void CreateLightningStrikes()
    {
        // Connect the chakra points with lightning strikes in random order
        for (int i = 0; i < chakraPositions.Count - 1; i++)
        {
            int randomIndex = Random.Range(i + 1, chakraPositions.Count);
            Vector3 startPoint = chakraPositions[i];
            Vector3 endPoint = chakraPositions[randomIndex];

            // Create a lightning strike between the start and end points
            CreateLightningStrike(startPoint, endPoint);
        }
    }

    private void CreateLightningStrike(Vector3 startPoint, Vector3 endPoint)
    {
        // Implement the logic to create a lightning strike between the start and end points
        // This can involve visual effects, particle systems, or other techniques to simulate lightning
        // Use distortion effects and emit sparks, gaining in particles until health hits 100
        // Create a particle system for the lightning strike
        GameObject lightningParticles = new GameObject("LightningParticles");
        ParticleSystem particleSystem = lightningParticles.AddComponent<ParticleSystem>();

        // Set the properties of the particle system
        ParticleSystem.MainModule mainModule = particleSystem.main;
        mainModule.startLifetime = 1f;
        mainModule.startSpeed = 10f;
        mainModule.startSize = 1f;
        mainModule.startColor = Color.white;

        // Set the emission properties of the particle system
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = 10f;

        // Set the shape properties of the particle system
        ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Cone;
        shapeModule.angle = 30f;
        shapeModule.radius = 1f;

        // Set the position and rotation of the particle system
        lightningParticles.transform.position = startPoint;
        lightningParticles.transform.LookAt(endPoint);

        lightningParticles.transform.LookAt(endPoint);

        // Calculate the distance between the start and end points
        float distance = Vector3.Distance(startPoint, endPoint);

        // Calculate the number of lightning strikes to create based on the distance
        int numStrikes = Mathf.RoundToInt(distance / 10f);

        // Create lightning strikes between the start and end points
        for (int i = 0; i < numStrikes; i++)
        {
            // Calculate the position of the current lightning strike along the line between the start and end points
            Vector3 strikePosition = Vector3.Lerp(startPoint, endPoint, (float)i / (float)(numStrikes - 1));

            // Create a lightning strike at the current position
            CreateLightningStrike(strikePosition);
        }

        // Play the particle system
        
        // Play the particle system
        particleSystem.Play();
    }
}