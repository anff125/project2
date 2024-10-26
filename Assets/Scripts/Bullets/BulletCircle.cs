using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCircle : Bullet
{
    private Vector3 _center; // The point around which the bullet revolves
    private float _initialRadius = 20f; // Initial distance from the center point
    private float _radiusDecayRate = 5f; // Rate at which the radius decreases
    private float _revolveSpeed = 120f; // Speed of revolution in degrees per second
    private float _initialAngle = 0f; // Starting angle in degrees

    private float currentRadius;
    private float currentAngle;

    public void SetCircleProperty(
        Vector3 center,
        float angle,
        float radius = 20
    )
    {
        _center = center;
        _initialAngle = angle;
        currentAngle = angle;

        _initialRadius = radius;
        currentRadius = radius;

        SetDamage(10);
    }

    protected override void Update()
    {
        // Reduce the radius over time
        currentRadius -= _radiusDecayRate * Time.deltaTime;
        if (currentRadius < 0)
        {
            Destroy(gameObject);
        }

        // Update the angle based on revolve speed
        currentAngle += _revolveSpeed * Time.deltaTime;

        // Convert polar coordinates (angle and radius) to Cartesian (x, y)
        float radianAngle = currentAngle * Mathf.Deg2Rad;
        float xOffset = Mathf.Cos(radianAngle) * currentRadius;
        float zOffset = Mathf.Sin(radianAngle) * currentRadius;

        // Set bullet position relative to the center point
        transform.position = _center + new Vector3(xOffset, 0, zOffset);
    }
}
