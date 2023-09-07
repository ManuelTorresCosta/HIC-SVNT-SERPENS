using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Text inputText;
    public float blinkSpeed = 1f;

    private float _timer = 0f;
    private bool _canBlink = false;


    private void Awake()
    {
        inputText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.anyKey)
            SceneManager.LoadScene(0);

        if (_canBlink)
        {
            if (_timer < 1)
                _timer += blinkSpeed * Time.deltaTime;
            else
            {
                inputText.gameObject.SetActive(!inputText.gameObject.activeSelf);
                _timer = 0;
            }
        }
    }

    
    private void Blink()
    {
        if (!_canBlink)
            _canBlink = true;
        
    }
}
