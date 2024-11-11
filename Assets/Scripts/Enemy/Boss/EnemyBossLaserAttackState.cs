using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossLaserAttackState : EnemyState
{
    private EnemyBoss _bossEnemy;
    private readonly LineRenderer[] _laserLines = new LineRenderer[5]; // Array for 5 laser beams
    private readonly Vector3[] _laserDirections = new Vector3[5]; // Directions for each laser beam
    private float _laserPrepareTime = 0;
    private float _blinkTimer = 0;
    private bool _isBlinking = true;
    private float _laserDuration = 2f; // How long the laser will be active

    private const float BLINK_INTERVAL = 0.2f; // Blinking interval
    private const float LASER_PREPARE_TIME_MAX = .3f;
    private const float INITIAL_LASER_WIDTH = 0.1f; // Initial smaller width
    private const float FINAL_LASER_WIDTH = 1f; // Final enlarged width after blinking
    private const float LASER_LENGTH = 20f; // Length of each laser beam
    private const float MIN_ANGLE_DIFFERENCE = 20f; // Minimum angle difference between lasers

    public EnemyBossLaserAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _bossEnemy = EnemyStateMachine.Enemy as EnemyBoss;
        _laserPrepareTime = 0;
        _isBlinking = true;
        _blinkTimer = 0;
        _laserDuration = 3f;

        // Initialize lasers
        GenerateLaserDirections(); // Ensure lasers are spaced by at least 20 degrees

        for (int i = 0; i < 5; i++)
        {
            // Create a new GameObject for each laser
            GameObject laserObj = new GameObject("Laser" + i);
            _laserLines[i] = laserObj.AddComponent<LineRenderer>();

            // Customize LineRenderer (color, width, etc.)
            _laserLines[i].startWidth = INITIAL_LASER_WIDTH;
            _laserLines[i].endWidth = INITIAL_LASER_WIDTH;

            // Set color to black initially
            _laserLines[i].startColor = Color.black;
            _laserLines[i].endColor = Color.black;
            _laserLines[i].material = new Material(Shader.Find("Sprites/Default")); // Choose a suitable material
        }

        // Immediately fire the lasers (with smaller width)
        FireLasers();
    }

    public override void Update()
    {
        base.Update();

        _blinkTimer += Time.deltaTime;

        // Blink the lasers before firing with enlarged width
        if (_isBlinking)
        {
            if (_blinkTimer >= BLINK_INTERVAL)
            {
                ToggleLaserVisibility();
                _blinkTimer = 0;
            }

            // After some time, stop blinking and enlarge the lasers
            if (_laserPrepareTime >= LASER_PREPARE_TIME_MAX)
            {
                _isBlinking = false;
                EnlargeLasers();

            }
            else
            {
                _laserPrepareTime += Time.deltaTime; // Increase laser scale over time
            }
        }
        else
        {
            //check if the player is hit by the laser
            RaycastHit hit;
            for (int i = 0; i < 5; i++)
            {
                if (Physics.BoxCast(_bossEnemy.transform.position, new Vector3(0.5f, 0.5f, 0.5f), _laserDirections[i], out hit, Quaternion.identity, LASER_LENGTH))
                {
                    Debug.Log($"Laser {i} hit: {hit.collider.gameObject.name}");
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        IDamageable.Damage damage = new IDamageable.Damage(0.3f, ElementType.Physical, _bossEnemy.transform);
                        Player.Instance.TakeDamage(damage);
                    }
                }
            }
            
            // Lasers stay active for a duration
            _laserDuration -= Time.deltaTime;
            if (_laserDuration <= 0)
            {
                EnemyStateMachine.ChangeState(_bossEnemy.TrackPlayerState); // Return to tracking player after firing lasers
            }
        }
    }

    private void GenerateLaserDirections()
    {
        List<float> generatedAngles = new List<float>();
        int numLasers = 5; // Number of laser directions to generate

        // Randomly generate angles ensuring at least 20 degrees apart
        for (int i = 0; i < numLasers; i++)
        {
            float newAngle;
            bool validAngle;

            // Try generating a new angle and validate it
            do
            {
                validAngle = true;
                newAngle = Random.Range(0, 360f);

                // Check the difference between the new angle and all previously generated angles
                foreach (float existingAngle in generatedAngles)
                {
                    float angleDifference = Mathf.Abs(Mathf.DeltaAngle(existingAngle, newAngle));
                    if (angleDifference < MIN_ANGLE_DIFFERENCE)
                    {
                        validAngle = false;
                        break;
                    }
                }

            } while (!validAngle); // Repeat until a valid angle is found

            generatedAngles.Add(newAngle); // Store valid angle

            // Convert angle to direction on the XZ plane
            float radians = newAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));

            // Assign direction to the laser
            _laserDirections[i] = direction;
        }
    }


    private void ToggleLaserVisibility()
    {
        // Toggle the visibility of each laser during blinking phase
        foreach (var laser in _laserLines)
        {
            laser.enabled = !laser.enabled;
        }
    }
    private void ActivateLaserVisibility()
    {
        // Toggle the visibility of each laser during blinking phase
        foreach (var laser in _laserLines)
        {
            laser.enabled = true;
        }
    }
    private void FireLasers()
    {
        // Fire the lasers with initial smaller width
        ActivateLaserVisibility();
        for (int i = 0; i < 5; i++)
        {
            // Laser start position slightly above the boss's origin
            Vector3 laserStart = _bossEnemy.transform.position + new Vector3(0, 0.01f, 0);

            // Define laser length (10 units in the direction)
            Vector3 laserEnd = laserStart + _laserDirections[i] * LASER_LENGTH;

            // Set the positions of the LineRenderer
            _laserLines[i].SetPosition(0, laserStart);
            _laserLines[i].SetPosition(1, laserEnd);
        }
    }

    private void EnlargeLasers()
    {
        // Enlarge the width of the lasers after blinking is over
        ActivateLaserVisibility();
        foreach (var laser in _laserLines)
        {
            // Enlarge laser width
            laser.startWidth = FINAL_LASER_WIDTH;
            laser.endWidth = FINAL_LASER_WIDTH;
        }
    }

    public override void Exit()
    {
        base.Exit();
        // Cleanup or disable lasers when exiting
        foreach (var laser in _laserLines)
        {
            if (laser != null)
            {
                GameObject.Destroy(laser.gameObject);
            }
        }
    }
}