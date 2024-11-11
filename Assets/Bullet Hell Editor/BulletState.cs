using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState
{
    public Vector3 Center { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Direction { get; set; }
    public float StraightSpeed { get; set; }
    public float CircleAngle { get; set; }
    public float SineAngle { get; set; }
    public float Radius { get; set; }

    // You can add other properties as needed
    public BulletState(Vector3 center, Vector3 position, Quaternion rotation, Vector3 direction,
                         float straightSpeed, float circleAngle, float sineAngle, float radius)
    {
        Center = center;
        Position = position;
        Rotation = rotation;
        Direction = direction;
        StraightSpeed = straightSpeed;
        CircleAngle = circleAngle;
        SineAngle = sineAngle;
        Radius = radius;
    }

    // Copy constructor
    public BulletState(BulletState other)
    {
        Center = other.Center;
        Position = other.Position;
        Rotation = other.Rotation;
        Direction = other.Direction;
        StraightSpeed = other.StraightSpeed;
        CircleAngle = other.CircleAngle;
        SineAngle = other.SineAngle;
        Radius = other.Radius;
    }

    public void SetState(Vector3 center, Vector3 position, Quaternion rotation, Vector3 direction,
                         float straightSpeed, float circleAngle, float sineAngle, float radius)
    {
        Center = center;
        Position = position;
        Rotation = rotation;
        Direction = direction;
        StraightSpeed = straightSpeed;
        CircleAngle = circleAngle;
        SineAngle = sineAngle;
        Radius = radius;
    }
}
