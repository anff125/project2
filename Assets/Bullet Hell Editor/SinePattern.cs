using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BulletPatterns/SinePattern")]
public class SinePattern : BulletPatternBase
{
    public float amplitude = 1f;
    public float frequency = 30f;

    public override void ModifyTransform(BulletState bulletState)
    {
        float newAngle = bulletState.SineAngle + frequency * Time.deltaTime;
        newAngle %= 360f;
        float xOffset = Mathf.Sin(newAngle * Mathf.Deg2Rad) * amplitude;
        Vector3 offset = new Vector3(xOffset, 0, 0);
        offset = bulletState.Rotation * offset;
        Vector3 newPosition = bulletState.Center + offset;
        
        bulletState.Position = newPosition;
        bulletState.SineAngle = newAngle;
    }
}
