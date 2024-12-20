using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNew : MonoBehaviour
{
    [SerializeField] private LayerMask shooterLayerMask;
    public Transform crashEffectPrefab;
    public List<BulletPatternBase> patterns0 = new List<BulletPatternBase>();
    public List<BulletPatternBase> patterns1 = new List<BulletPatternBase>();
    public List<BulletState> bulletStates = new List<BulletState>();
    [SerializeField] private int patternIndex = 0;
    public float switchTimer = 2f;
    public float destroyTime = 5f;
    private bool hasChangedPatterns = false;
    private bool[] shootAtPlayer = new bool[2];
    private int _damage = 10;

    public void SetToShootPlayer(bool a, bool b)
    {
        shootAtPlayer[0] = a;
        shootAtPlayer[1] = b;
    }

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
        Vector3 lookAtPlayer = Player.Instance.transform.position - transform.position;
        Vector3 towardPlayer = new Vector3(lookAtPlayer.x, 0, lookAtPlayer.z).normalized;

        if (patterns0.Count == 0 || (hasChangedPatterns && patterns1.Count != 0))
        {
            hasChangedPatterns = true;
            
            if (shootAtPlayer[1])
            {
                Direction = towardPlayer;
            }
            for (int j = 0; j < patterns1.Count; ++j)
            {
                bulletStates.Add(new BulletState(Center, Position, Rotation, Direction, StraightSpeed, SecondStraightSpeed, CircleAngle, SineAngle, Radius));
            }
        }else{
            Debug.Log("Try to add Pattern0 !!!!!");
            if (shootAtPlayer[0])
            {
                Direction = towardPlayer;
            }
            for (int j = 0; j < patterns0.Count; ++j)
            {
                bulletStates.Add(new BulletState(Center, Position, Rotation, Direction, StraightSpeed, SecondStraightSpeed, CircleAngle, SineAngle, Radius));
            }
        }

        
        
        // bulletState = new BulletState(Center, Position, Rotation, Direction, StraightSpeed, CircleAngle, SineAngle, Radius);
    }

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        switchTimer -= Time.deltaTime;

        if (!hasChangedPatterns && switchTimer <= 0)
        {
            hasChangedPatterns = true;

            if (patterns0.Count != 0 && patterns1.Count != 0)
            {
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

    protected void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Bullet Hit: " + collision.name);
        // ignore the entity that shoots the bullet
        if (shooterLayerMask == (shooterLayerMask | (1 << collision.gameObject.layer)))
        {
            return;
        }
        
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) return;

        CrashEffect();
        
        IDamageable.Damage damage = new IDamageable.Damage(_damage, ElementType.Physical, transform);
        damageable.TakeDamage(damage);
        
        //destroy bullet
        Destroy(gameObject);
    }

    protected void CrashEffect()
    {
        if (crashEffectPrefab != null)
        {
            // Instantiate the crash effect at the bullet's position
            Instantiate(crashEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}
