using TMPro;
using UnityEngine;

public class InputFieldGrabber : MonoBehaviour
{
    public TMP_InputField maxEnemiesInputField; // Reference to the UI InputField for Max Enemies
    public TMP_InputField spawnWindowInputField; // Reference to the UI InputField for Spawn Window
    public GameManager gameManager; // Reference to the GameManager

    void Start()
    {
        maxEnemiesInputField.onEndEdit.AddListener(OnMaxEnemiesInputChanged); // Listen for changes in Max Enemies InputField
        spawnWindowInputField.onEndEdit.AddListener(OnSpawnWindowInputChanged); // Listen for changes in Spawn Window InputField
    }

    private void OnMaxEnemiesInputChanged(string input)
    {
        // Convert the input string to an integer for max enemies
        if (int.TryParse(input, out int maxEnemies))
        {
            // Use the ChangeMaxEnemiesInScene method from GameManager to update max enemies
            gameManager.ChangeMaxEnemiesInScene(maxEnemies);
        }
        else
        {
            Debug.LogWarning("Invalid input for max enemies.");
        }
    }

    private void OnSpawnWindowInputChanged(string input)
    {
        // Convert the input string to a float for spawn window
        if (float.TryParse(input, out float spawnWindow))
        {
            // Directly update the spawnWindow in the GameManager
            gameManager.ChangeSpawnWindow(spawnWindow);
        }
        else
        {
            Debug.LogWarning("Invalid input for spawn window.");
        }
    }
}