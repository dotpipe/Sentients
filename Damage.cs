
using UnityEngine;

public class Damage : MonoBehaviour
{
    private Character character;
    private FightingStyle style;
    // Stick figure representation of the Avatar
    private GameObject head, torso, leftArm, rightArm, leftLeg, rightLeg;
    private float damagePerHit = 0.05f; // 15% health loss in 3 hits
    private int hitCount = 0;
    private int swingForce = 1;

    void Awake()
    {
        // Initialize the stick figure
        head = new GameObject("Head");
        torso = new GameObject("Torso");
        leftArm = new GameObject("LeftArm");
        rightArm = new GameObject("RightArm");
        leftLeg = new GameObject("LeftLeg");
        rightLeg = new GameObject("RightLeg");
    }

    void OnGUI()
    {
        // Detect 'a' key press for random movement
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
        {
            // Perform random movement
            Vector3 randomMovement = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            character.Position += randomMovement;

            // Check for other Avatars within the sphere of the figure
            Collider[] hitColliders = Physics.OverlapSphere(character.Position, 1f);
            foreach (var hitCollider in hitColliders)
            {
                Avatar hitAvatar = hitCollider.GetComponent<Avatar>();
                if (hitAvatar != null && hitAvatar != character)
                {
                    // Apply damage to the hit Avatar
                    hitAvatar.Health -= damagePerHit;
                    hitCount++;

                    // If 3 hits have been made, reset the hit count and increase the damage per hit
                    if (hitCount >= 3)
                    {
                        hitCount = 0;
                        damagePerHit *= 1.15f; // Increase damage by 15%
                    }
                }
            }
        }
    }

    void Start()
    {
        character = GetComponent<Character>();
    }

    void Update()
    {
        if (character.IsDefeated)
        {
            return;
        }

        // Check for collisions with other characters
        Collider[] hitColliders = Physics.OverlapSphere(character.Position, style.CalculateRange());
        foreach (var hitCollider in hitColliders)
        {
            Character hitCharacter = hitCollider.GetComponent<Character>();
            if (hitCharacter != null && hitCharacter != character)
            {
                // Calculate the damage based on the depth of the hit
                Vector3 hitDepth = hitCollider.ClosestPoint(character.Position) - character.Position;
                float damage = style.CalculateDamage() * hitDepth.magnitude;

                // Apply the damage to the hit character
                hitCharacter.ApplyDamage(damage);

                // Increase the character's skill in the current fighting style
                character.IncreaseSkill(style.StyleID, damage);
            }
        }
    }
}

