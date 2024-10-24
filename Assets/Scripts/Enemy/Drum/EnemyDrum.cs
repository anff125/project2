using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrum : Enemy
{
    #region States

//make track player state and shoot state
    public EnemyDrumTrackPlayerState TrackPlayerState { get; private set; }
    public EnemyDrumShootState ShootState { get; private set; }

    #endregion
    [SerializeField] public List<float> timeSpace; // Time intervals between each bullet shot, customizable in Unity Editor
    protected override void Awake()
    {
        base.Awake();
        EnemyStateMachine = new EnemyStateMachine(this);
        TrackPlayerState = new EnemyDrumTrackPlayerState(EnemyStateMachine);
        ShootState = new EnemyDrumShootState(EnemyStateMachine);
    }
    protected override void Start()
    {
        base.Start();
        EnemyStateMachine.Initialize(TrackPlayerState);
        WeaponIndex = 2;
    }
    
    public int totalBulletsShot = 0;
    public int reflectedBullets = 0;

    public void RegisterBullet(Bullet bullet) {
        bullet.SetShooter(this);  // Set the shooter reference in Bullet
    }

    public void IncrementReflectedCount() {
        reflectedBullets++;
        CheckIfDestroyed();
    }

    private void CheckIfDestroyed() {
        if (reflectedBullets == totalBulletsShot && totalBulletsShot > 0) {
            Destroy(gameObject);  // Destroy only this enemy
        }
    }


}