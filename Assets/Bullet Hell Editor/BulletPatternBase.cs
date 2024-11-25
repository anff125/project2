using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletPatternBase : ScriptableObject
{
    public abstract void ModifyTransform(BulletState bulletState);
}