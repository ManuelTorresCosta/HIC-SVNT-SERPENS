using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Camera Camera { get; private set; }


    private void Awake()
    {
        Camera = Camera.main;
    }


    public void SetCameraBackgroundBlack()
    {
        Camera.backgroundColor = Color.black;
    }
    public void EndAnimation()
    {
        SceneManager.LoadScene(2);
    }
}
