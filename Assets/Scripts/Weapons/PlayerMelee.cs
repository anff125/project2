using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private float maxRadius = 5.5f; // Maximum distance from player
    private float minRadius = 1.5f;
    private float swingDuration = 0.5f; // Duration of the swing
    private Vector3 initialPosition;
    private float yPos = 1.4f;
    private bool isSwinging = false;
    public bool IsSwinging => isSwinging;

    void Awake()
    {
        initialPosition = transform.position;
    }

    public void Swing()
    {
        if (!isSwinging)
        {
            isSwinging = true;
            StartCoroutine(SwingCoroutine());
        }
    }

    private IEnumerator SwingCoroutine()
    {
        // Player.Instance.weaponHitbox.ActivateHitbox();

        float elapsedTime = 0f;
        float startAngle = 30f;   // Start at 45 degrees
        float endAngle = 135f;    // End at 225 degrees (180 degrees from start)

        while (elapsedTime < swingDuration)
        {
            // Calculate normalized progress from 0 to 1
            float progress = elapsedTime / swingDuration;

            // Calculate the current angle based on progress
            float angle = Mathf.Lerp(startAngle, endAngle, progress);

            // Calculate the radius (distance from player), peaking at 135 degrees (halfway through the swing)
            float radius = Mathf.Sin(progress * Mathf.PI) * maxRadius;

            // Calculate the position based on angle and radius
            Vector3 localOffset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
            localOffset = localOffset + new Vector3(0, yPos, 0);

            Vector3 offset = Player.Instance.transform.rotation * localOffset;

            // Set the weapon's position relative to the player
            transform.position = Player.Instance.transform.position + offset;

            // Rotate the weapon to face the player
            // transform.LookAt(player.transform.position);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the weapon's position if necessary (optional)
        Vector3 lOffset = new Vector3(Mathf.Cos(startAngle * Mathf.Deg2Rad) * minRadius, yPos, Mathf.Sin(startAngle * Mathf.Deg2Rad) * minRadius);
        transform.position = Player.Instance.transform.position + (Player.Instance.transform.rotation * lOffset);

        isSwinging = false;
        // Player.Instance.weaponHitbox.DeactivateHitbox();
    }
}
