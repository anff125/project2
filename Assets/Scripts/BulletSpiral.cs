using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpiral : Bullet
{
    private Vector3 _center;
    private float _angle;
    private float _rotationSpeed;
    private float _radius;
    private float _radiusGrowth;

    public void SetSpiralProperty(
        Vector3 center,
        float angle,
        float rotationSpeed = 270,
        float radius = 1,
        float radiusGrowth = 1
    )
    {
        _center = center;
        _angle = angle;
        _rotationSpeed = rotationSpeed;
        _radius = radius;
        _radiusGrowth = radiusGrowth;

    }
    protected override void Update()
    {
        _center += MovingDirection * (Time.deltaTime * Speed);
        _angle += _rotationSpeed * Time.deltaTime;
        _angle %= 360f;
        _radius += _radiusGrowth * Time.deltaTime;
        UpdateBulletPosition();
    }
    private void UpdateBulletPosition()
    {
        float x = _center.x + _radius * Mathf.Cos(_angle * Mathf.Deg2Rad);
        float z = _center.z + _radius * Mathf.Sin(_angle * Mathf.Deg2Rad);
        transform.position = new Vector3(x, _center.y, z);
    }
}
