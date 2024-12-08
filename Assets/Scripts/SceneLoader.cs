using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Instance
    public static SceneLoader Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LoadPlaygroundScene()
    {
        SceneManager.LoadScene("Anff125_PlayGroundScene"); // Replace "Playground" with the exact name of your scene
    }
    
    public void LoadBossScene()
    {
        SceneManager.LoadScene("Anff125_BossScene"); // Replace "Boss" with the exact name of your scene
    }
    public void LoadLevel(String level)
    {
        SceneManager.LoadScene(level); // Replace "Boss" with the exact name of your scene
    }
    
    public void LoadBoss2Scene()
    {
        SceneManager.LoadScene("f(x)_Boss2Scene"); // Replace "Boss" with the exact name of your scene
    }
}