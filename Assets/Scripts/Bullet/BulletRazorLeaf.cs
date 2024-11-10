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
    private Vector3 _targetPosition;

    //create enum type for the bullet following 
    public enum BulletType
    {
        FollowTarget,
        FollowUntilPosition
    }

    private BulletType _bulletType;

    public void SetBulletType(BulletType bulletType)
    {
        _bulletType = bulletType;
    }
    public void SetTarget(Transform target)
    {
        _target = target;
    }
    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetToggle(int val)
    {
        toggle = val;
    }
    protected override void Start()
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
        if (_bulletType == BulletType.FollowTarget)
        {
            FollowTarget();
        }
        else if (_bulletType == BulletType.FollowUntilPosition)
        {
            FollowUntilPosition();
        }
        base.Update();
    }
    
    private void FollowTarget()
    {
        _movingDirection = direction;
        elapsedTime += Time.deltaTime;
        // Calculate the target direction toward the _target
        Vector3 targetDirection = !_target ? direction : (_target.position - transform.position).normalized;
        targetDirection.y = 0;

        // according to lifetime, increase the speed of the bullet
        float speedMultiplier = Mathf.Lerp(1f, 2f, elapsedTime * .75f);

        // Rotate toward the player
        direction = Vector3.RotateTowards(direction, targetDirection, (float)Math.Pow(speedMultiplier, 4) * Time.deltaTime, 0.0f);
        SetSpeed(baseSpeed * speedMultiplier);
    }

    private bool hasReachedTargetPosition = false;
    [SerializeField] private float initialRotationSpeed = 5f;

    private void FollowUntilPosition()
    {
        
    }




}