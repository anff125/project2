using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPlaygroundScene()
    {
        SceneManager.LoadScene("Anff125_PlayGroundScene"); // Replace "Playground" with the exact name of your scene
    }
    
    public void LoadBossScene()
    {
        SceneManager.LoadScene("Anff125_BossScene"); // Replace "Boss" with the exact name of your scene
    }
}