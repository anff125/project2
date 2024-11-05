using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletRazorLeaf : Bullet
{
    public Transform player; // Reference to the player's transform

    private Vector3 direction;
    private float baseSpeed;

    private float elapsedTime = 0f;
    private int toggle;

    private Transform _target;
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    public void SetToggle(int val)
    {
        toggle = val;
    }
    void Start()
    {
        if (player == null)
        {
            player = Player.Instance.transform;
        }
        baseSpeed = Speed;

        _target = player;
        // Initial direction to be 45 degree away from the direction towards the player
        direction = (_target.position - transform.position).normalized;
        direction.y = 0;
        direction = Quaternion.Euler(0, 60 * toggle, 0) * direction;
    }

    protected override void Update()
    {
        _movingDirection = direction;
        elapsedTime += Time.deltaTime;
        // Calculate the target direction toward the _target
        Vector3 targetDirection;
        if (_target == null)
        {
            targetDirection = direction;
        }
        else
        {
            targetDirection = (_target.position - transform.position).normalized;
        }

        targetDirection.y = 0;

        // according to lifetime, increase the speed of the bullet
        float speedMultiplier = Mathf.Lerp(1f, 2f, elapsedTime * .75f);

        // Rotate toward the player
        direction = Vector3.RotateTowards(direction, targetDirection, (float)Math.Pow(speedMultiplier, 4) * Time.deltaTime, 0.0f);
        SetSpeed(baseSpeed * speedMultiplier);

        base.Update();
    }

}