using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BulletPatterns/SinePattern Z-axis")]
public class SinePatternZ : BulletPatternBase
{
    public float amplitude = 4f;
    public float frequency = 30f;

    public override void ModifyTransform(BulletState bulletState)
    {
        float newAngle = bulletState.SineAngle + frequency * Time.deltaTime;
        newAngle %= 360f;
        float zOffset = Mathf.Sin(newAngle * Mathf.Deg2Rad) * amplitude;
        Vector3 offset = new Vector3(0, 0, zOffset);
        offset = bulletState.Rotation * offset;
        Vector3 newPosition = bulletState.Center + offset;
        
        bulletState.Position = newPosition;
        bulletState.SineAngle = newAngle;
    }
}
