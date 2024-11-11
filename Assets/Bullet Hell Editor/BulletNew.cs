using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : Bullet
{
    public List<BulletPatternBase> patterns0 = new List<BulletPatternBase>();
    public List<BulletPatternBase> patterns1 = new List<BulletPatternBase>();
    public List<BulletState> bulletStates = new List<BulletState>();
    [SerializeField] private int patternIndex = 0;
    public float switchTimer = 2f;
    public float destroyTime = 5f;
    private bool hasChangedPatterns = false;

    public void SetBulletProperty(
        Vector3 Center,
        Vector3 Position,
        Quaternion Rotation,
        Vector3 Direction,
        float StraightSpeed,
        float SecondStraightSpeed,
        float CircleAngle,
        float SineAngle,
        float Radius
    ){
        bulletStates.Clear();
        Vector3 lookAtPlayer = (Player.Instance.transform.position - transform.position);
        Vector3 towardPlayer = new Vector3(lookAtPlayer.x, transform.position.y, lookAtPlayer.z).normalized;

        if (patterns0.Count == 0)
        {
            for (int j = 0; j < patterns1.Count; ++j)
            {
                bulletStates.Add(new BulletState(Center, Position, Rotation, Direction, StraightSpeed, SecondStraightSpeed, CircleAngle, SineAngle, Radius));
            }
        }
        else if (hasChangedPatterns)
        {
            for (int j = 0; j < patterns0.Count; ++j)
            {
                bulletStates.Add(new BulletState(Center, Position, Rotation, towardPlayer, StraightSpeed, SecondStraightSpeed, CircleAngle, SineAngle, Radius));
            }
        }else{
            for (int j = 0; j < patterns0.Count; ++j)
            {
                bulletStates.Add(new BulletState(Center, Position, Rotation, Direction, StraightSpeed, SecondStraightSpeed, CircleAngle, SineAngle, Radius));
            }
        }

        
        
        // bulletState = new BulletState(Center, Position, Rotation, Direction, StraightSpeed, CircleAngle, SineAngle, Radius);
    }

    protected override void Start()
    {
        if (patterns0.Count == 0)
        {
            hasChangedPatterns = true;
        }
        Destroy(gameObject, destroyTime);
    }

    protected override void Update()
    {
        switchTimer -= Time.deltaTime;

        if (!hasChangedPatterns && switchTimer <= 0)
        {
            hasChangedPatterns = true;

            var state = bulletStates[0];
            SetBulletProperty(
                state.Center,
                transform.position,
                transform.rotation,
                state.Direction,
                state.StraightSpeed,
                state.SecondStraightSpeed,
                state.CircleAngle,
                state.SineAngle,
                state.Radius
            );
        }
        ApplyPatterns();
    }

    private void ApplyPatterns()
    {
        // Apply pattern in sequence
        Vector3 center = bulletStates[0].Center;
        if (hasChangedPatterns)
        {
            for (int i = 0; i < patterns1.Count; ++i)
            {
                bulletStates[i].Center = center;
                patterns1[i].ModifyTransform(bulletStates[i]);
                center = bulletStates[i].Center;
            }
        } else{
            for (int i = 0; i < patterns0.Count; ++i)
            {
                bulletStates[i].Center = center;
                patterns0[i].ModifyTransform(bulletStates[i]);
                center = bulletStates[i].Center;
            }
        }
        
        transform.position = bulletStates[bulletStates.Count-1].Position;
    }
}
