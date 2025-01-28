using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField] Transform player; // Reference to the player object
    [SerializeField] Animator bossAnimator; // Animator component for the boss
    [SerializeField] string glanceAnimationName = "Glance"; // Name of the glance animation
    [SerializeField] SpriteRenderer spriteRenderer;

    bool isPlayerCaught = false;
    bool isGlancing = false;

    void Update()
    {
        // Check if the boss is playing the "glance" animation
        if (IsPlayingAnimation(glanceAnimationName) && !isGlancing)
            StartGlance();
        else if (!IsPlayingAnimation(glanceAnimationName) && isGlancing)
            EndGlance();
    }

    void StartGlance()
    {
        isGlancing = true;
        Debug.Log("Boss is glancing at the player!");

        CheckForGame();

        // Check if the player is caught
        if (IsPlayerCaught())
        {
            RunFailSequence();
            Debug.Log("Player caught by the boss!");
            // Trigger failure state (e.g., end game, reduce score, etc.)
        }
    }

    void RunFailSequence()
    {

    }

    void CheckForGame()
    {
        if (spriteRenderer.enabled)
            isPlayerCaught= true;
    }

    void EndGlance()
    {
        isGlancing = false;
        Debug.Log("Boss stopped glancing.");
    }

    bool IsPlayerCaught() => isPlayerCaught;

    bool IsPlayingAnimation(string animationName)
    {
        if (bossAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            return true;
        }
        return false;
    }
}
