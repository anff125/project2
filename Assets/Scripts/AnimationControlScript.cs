using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControlScript : MonoBehaviour
{
    private Animator _animator;
    private float velocityX;
    private float velocityZ;

    [SerializeField] private float aimingLayerWeight = 1f; // Default weight for the aiming layer

    void Start()
    {
        _animator = GetComponent<Animator>();

        // Initialize the aiming layer weight
        SetAimingLayerWeight(aimingLayerWeight);
    }

    void Update()
    {
        Vector2 move = GameInput.Instance.GetMovementVector();

        Vector3 moveDir = new Vector3(move.x, 0, move.y);

        Vector3 playerForward = Player.Instance.transform.forward;
        Vector3 playerRight = Player.Instance.transform.right;

        // Calculate the velocities relative to the player's facing direction
        float velocityZ = Vector3.Dot(moveDir, playerForward); // Forward/backward velocity
        float velocityX = Vector3.Dot(moveDir, playerRight);   // Right/left velocity

        // Pass the calculated velocities to the animator
        _animator.SetFloat("VelocityX", velocityX);
        _animator.SetFloat("VelocityZ", velocityZ);
    }
    
    // Function to set the weight of the aiming layer
    public void SetAimingLayerWeight(float weight)
    {
        int aimingLayerIndex = _animator.GetLayerIndex("Aiming"); // Get the index of the "Aiming" layer
        if (aimingLayerIndex != -1)
        {
            _animator.SetLayerWeight(aimingLayerIndex, weight);
        }
        else
        {
            Debug.LogWarning("Aiming layer not found in the animator!");
        }
    }
}