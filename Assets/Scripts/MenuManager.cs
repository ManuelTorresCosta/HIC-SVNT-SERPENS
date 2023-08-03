using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text pressPlayText;

    private float _timer = 0;
    private float _maxTimer = 1;
    public float blinkSpeed = 1;

    private bool startLerp = false;
    public Color blackColor;
    public Color greenColor;
    private Color _myColor;
    public float lerpSpeed = 1;


    
    private void Update()
    {
        if (_timer < _maxTimer)
            _timer += blinkSpeed * Time.deltaTime;
        else
        {
            Blink();
            _timer = 0;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            int currScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currScene + 1);
        }    

        else if (Input.GetKeyDown(KeyCode.F) && !startLerp)
        {
            startLerp = true;
            _myColor = blackColor;
        }

        if (startLerp)
        {
            _myColor = Color.Lerp(_myColor, greenColor, lerpSpeed * Time.deltaTime);
            Camera.main.backgroundColor = _myColor;

            //if (_myColor == greenColor)
            //    startLerp = false;
        }
    }



    private void Blink()
    {
        pressPlayText.enabled = pressPlayText.enabled ? false : true;
    }
}
