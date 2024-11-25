using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BulletPatterns/CirclePattern")]
public class CirclePattern : BulletPatternBase
{
    public float rotationSpeed = 180f;
    public float radiusGrowthSpeed = 1f;

    public override void ModifyTransform(BulletState bulletState)
    {
        float newAngle = bulletState.CircleAngle + rotationSpeed * Time.deltaTime;
        newAngle %= 360f;
        float newRadius = bulletState.Radius + radiusGrowthSpeed * Time.deltaTime;
        Vector3 localOffset = new Vector3(Mathf.Cos(newAngle * Mathf.Deg2Rad), 0, Mathf.Sin(newAngle * Mathf.Deg2Rad)) * newRadius;
        Vector3 offset = bulletState.Rotation * localOffset;

        Vector3 newPosition = bulletState.Center + offset;
        bulletState.Position = newPosition;
        bulletState.CircleAngle = newAngle;
        bulletState.Radius = newRadius;
    }
}
