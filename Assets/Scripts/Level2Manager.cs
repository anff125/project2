using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] private List<Transform> room1Enemies;
    [SerializeField] private List<Transform> room2Enemies;
    [SerializeField] private List<Transform> doors;
    private bool room1_check = false;
    private bool room2_check = false;
    void Update()
    {
        //if roo1Enemies are all null open doors.
        if (room1Enemies.TrueForAll(enemy => enemy == null) && !room1_check)
        {
            room1_check = true;
            foreach (var door in doors)
            {
                OpenDoor(doors.IndexOf(door));
            }
        }

        //if roo2Enemies are all null open doors.
        if (room2Enemies.TrueForAll(enemy => enemy == null) && !room2_check)
        {
            room2_check = true;
            foreach (var door in doors)
            {
                OpenDoor(doors.IndexOf(door));
            }
        }

    }

    private void OpenDoor(int doorIndex)
    {
        StartCoroutine(OpenDoorGradually(doorIndex));
    }

    private IEnumerator OpenDoorGradually(int doorIndex)
    {
        float duration = 2f; // Duration in seconds
        float elapsedTime = 0f;
        Quaternion initialRotation = doors[doorIndex].rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(Vector3.up * 90f);

        while (elapsedTime < duration)
        {
            doors[doorIndex].rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doors[doorIndex].rotation = targetRotation; // Ensure the door reaches the final rotation
    }

    private void CloseDoor(int doorIndex)
    {
        StartCoroutine(CloseDoorGradually(doorIndex));
    }
    public void CloseAllDoors()
    {
        foreach (var door in doors)
        {
            CloseDoor(doors.IndexOf(door));
        }
    }

    public void ActivateRoom2Enemies()
    {
        // Activate room 2 enemies
        foreach (var enemy in room2Enemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }
    private IEnumerator CloseDoorGradually(int doorIndex)
    {
        float duration = 1f; // Duration in seconds
        float elapsedTime = 0f;
        Quaternion initialRotation = doors[doorIndex].rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(Vector3.up * -90f);

        while (elapsedTime < duration)
        {
            doors[doorIndex].rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doors[doorIndex].rotation = targetRotation; // Ensure the door reaches the final rotation
    }
}