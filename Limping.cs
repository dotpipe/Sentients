```csharp
using UnityEngine;

public class Limping : MonoBehaviour
{
    private Character character;
    private Animator animator;

    // Limb damage thresholds
    private const float LIMP_THRESHOLD = 30f;
    private const float CRAWL_THRESHOLD = 10f;

    void Start()
    {
        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (character.IsDefeated)
        {
            return;
        }

        // Check the character's health and apply the appropriate limping animation
        if (character.Health <= CRAWL_THRESHOLD)
        {
            animator.SetBool("IsCrawling", true);
            animator.SetBool("IsLimping", false);
        }
        else if (character.Health <= LIMP_THRESHOLD)
        {
            animator.SetBool("IsLimping", true);
            animator.SetBool("IsCrawling", false);
        }
        else
        {
            animator.SetBool("IsLimping", false);
            animator.SetBool("IsCrawling", false);
        }
    }
}
```

