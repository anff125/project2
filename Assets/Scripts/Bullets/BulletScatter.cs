using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScatter : Bullet
{
    [SerializeField] private float initialDelay = 1f;        // Wait time before initial move
    [SerializeField] private float initialMoveSpeed = 4f;    // Speed of the initial move
    [SerializeField] private float initialMoveDuration = 2.5f; // Duration of the initial move
    [SerializeField] private float pauseDuration = 0.3f;     // Duration to pause after initial move
    [SerializeField] private float secondMoveSpeed = 7f;     // Speed of the second move
    [SerializeField] private float secondMoveDuration = 4f;  // Duration of the second move
    private float initialTimer;

    private Vector3 currentDirection;
    

    private void Start()
    {
        initialTimer = UnityEngine.Random.Range(0f, initialMoveDuration);
        initialDelay = initialTimer;
        SetDamage(10);
    }

    protected override void Update()
    {
        if (initialTimer > 0)
        {
            initialTimer -= Time.deltaTime;
            if (initialTimer <= 0)
            {
                StartCoroutine(BulletMovementSequence());
            }
        }
    }

    private IEnumerator BulletMovementSequence()
    {
        // 1. Wait for initial delay
        // yield return new WaitForSeconds(initialDelay);

        // 2. Choose a random initial angle and set direction
        float randomAngle = UnityEngine.Random.Range(0f, 360f);
        currentDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;

        // 3. Move forward in the initial direction
        float elapsedTime = 0f;
        while (elapsedTime < initialMoveDuration - initialDelay)
        {
            transform.position += currentDirection * initialMoveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 4. Pause for a short duration
        yield return new WaitForSeconds(pauseDuration);

        // 5. Randomly choose a new direction (forward, backward, left, or right)
        Vector3[] possibleDirections = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
        currentDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Length)];

        // 6. Move in the chosen direction
        elapsedTime = 0f;
        while (elapsedTime < secondMoveDuration)
        {
            transform.position += currentDirection * secondMoveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Optionally destroy the bullet after the sequence
        Destroy(gameObject);
    }
}
