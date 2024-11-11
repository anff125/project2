using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : MonoBehaviour
{
    public List<BulletPatternBase> patterns = new List<BulletPatternBase>();
    public List<BulletState> bulletStates = new List<BulletState>();

    public void setBulletProperty(
        Vector3 Center,
        Vector3 Position,
        Quaternion Rotation,
        Vector3 Direction,
        float StraightSpeed,
        float CircleAngle,
        float SineAngle,
        float Radius
    ){
        foreach (var pattern in patterns)
        {
            bulletStates.Add(new BulletState(Center, Position, Rotation, Direction, StraightSpeed, CircleAngle, SineAngle, Radius));
        }
        // bulletState = new BulletState(Center, Position, Rotation, Direction, StraightSpeed, CircleAngle, SineAngle, Radius);
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        ApplyPatterns();
    }

    private void ApplyPatterns()
    {
        // Apply pattern in sequence
        Vector3 center = bulletStates[0].Center;
        for (int i = 0; i < patterns.Count; ++i)
        {
            bulletStates[i].Center = center;
            patterns[i].ModifyTransform(bulletStates[i]);
            center = bulletStates[i].Center;
        }
        transform.position = bulletStates[bulletStates.Count-1].Position;
    }
}
