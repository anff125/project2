using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    // public Animator animator = null;
    [SerializeField] private EnemyBoss2 enemyBoss;
    public bool isTesting;
    public bool isRandom;
    public float reverseRotation = 1;
    public List<Transform> bulletPrefabs = new List<Transform>();
    public int bulletIndex = 0;
    public float fireRate;
    public int numberOfBullets;
    public float fireRadius = 2;
    public float rotationSpeed = 0;
    public float straightSpeed = 0.5f;
    public float secondStraightSpeed = 1f;
    public int numberOfWave;
    public float minAngle = 0f;
    public float maxAngle = 360f;
    public float destroyTime = 5f;
        
    public float fireTimer = 0;
    public bool isFiring = false;
    public bool fixDirection = false;
    public bool lookAtPlayer = false;
    public bool shootAtPlayer0 = false;
    public bool shootAtPlayer1 = false;
    public Animator animator;
    private AnimationClip clip;
    private Coroutine animationCoroutine;
    public bool InAnimation => animationCoroutine != null;

    private AnimationClip GetAnimationClipByName(string animationName)
    {
        foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name == animationName)
            {
                return animationClip;
            }
        }
        return null; // Return null if no clip found with the name
    }
    public void PlayAnimationMultipleTimes(int index, int playCount, out float clipLength)
    {
        clip = GetAnimationClipByName("AnimationDanmoku" + index);

        if (clip != null && playCount > 0)
        {
            clipLength = (clip.length + 0.1f) * playCount;
            animationCoroutine = StartCoroutine(PlayAnimationRoutine("AnimationDanmoku" + index, playCount, clip.length));
        }
        else
        {
            clipLength = 0f;
            Debug.LogError("Animation clip not found or invalid play count.");
        }
    }
    public void PlayIdleAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animator.Play("IdleState", 0, 0f);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isFiring && !isTesting)  { return; }

        fireTimer += Time.deltaTime;
        
        if (fireRate != 0 && fireTimer >= 1f / fireRate)
        {
            FireBullet();
            fireTimer = 0f;
        }

        transform.Rotate(new Vector3(0, reverseRotation * rotationSpeed * Mathf.Deg2Rad, 0), Space.Self);

        if (lookAtPlayer)
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            transform.LookAt(new Vector3(playerPosition.x, transform.position.y, playerPosition.z));
        }
    }

    void FireBullet()
    {
        if (bulletIndex >= bulletPrefabs.Count)
        {
            return;
        }

        float angleStep = (maxAngle - minAngle) / numberOfBullets;  // Divide 360 degrees by the number of bullets
        float angle = minAngle;
        for (int i=0; i<numberOfBullets; ++i)
        {
            float bulletPosX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float bulletPosZ = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 worldOffset = new Vector3(bulletPosX, 0, bulletPosZ);
            Vector3 offset = transform.rotation * worldOffset;
            Vector3 fixOffset = enemyBoss.transform.rotation * worldOffset;
            Vector3 bulletPosition = transform.position + fireRadius * offset;

            Transform bulletTransform = Instantiate(bulletPrefabs[bulletIndex], bulletPosition, transform.rotation);
            BulletNew bullet = bulletTransform.GetComponent<BulletNew>();

            bullet.SetToShootPlayer(shootAtPlayer0, shootAtPlayer1);
            bullet.destroyTime = destroyTime;
            Vector3 direction = fixDirection ? fixOffset.normalized : offset.normalized;
            bullet.transform.LookAt(bulletPosition + direction);
            bullet.SetBulletProperty(bulletPosition, bulletPosition, bullet.transform.rotation, direction, straightSpeed, secondStraightSpeed, 270f, 0f, fireRadius);

            angle += angleStep;
        }
    }

    private IEnumerator PlayAnimationRoutine(string animationName, int playCount, float duration)
    {
        int currentPlayCount = 0;

        while (currentPlayCount < playCount)
        {
            Debug.Log("Play " + animationName + " " + currentPlayCount + " out of " + playCount + " time");
            animator.Play(animationName); // Play the animation
            currentPlayCount++;
            yield return new WaitForSeconds(duration + 0.1f); // Wait for animation to finish
        }
    }

    public void SetRotationIdentity()
    {
        transform.rotation = Quaternion.identity;
    }
}
