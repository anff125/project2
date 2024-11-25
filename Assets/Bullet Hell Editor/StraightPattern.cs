using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BulletPatterns/StraightPattern")]
public class StraightPattern : BulletPatternBase
{
    public override void ModifyTransform(BulletState bulletState)
    {
        Vector3 moveDir = bulletState.StraightSpeed * Time.deltaTime * bulletState.Direction;
        Vector3 newCenter = bulletState.Center + moveDir;
        Vector3 newPosition = newCenter;
        
        bulletState.Center = newCenter;
        bulletState.Position = newPosition;
    }
}
