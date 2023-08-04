using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Kino;

public class MenuManager : MonoBehaviour
{
    public Animator CameraFx { get; private set; }
    public Image background;

    // Press play text
    public Text descText;
    public Text titleText;
    private float _timer = 0;
    private float _maxTimer = 1;
    public float blinkSpeed = 1;

    // Glitch effect
    public DigitalGlitch glitchFX { get; private set; }
    private float _fxTimer = 30;
    private float _fxMaxTimer = 30;


    // Menu fade
    private bool startLerp = false;
    public float lerpSpeed = 1;
    public Color blackColor;
    public Color greenColor;
    private Color _color;
    private float _alpha = 1;




    private void Awake()
    {
        CameraFx = transform.parent.GetComponentInChildren<Animator>();
        glitchFX = Camera.main.GetComponent<DigitalGlitch>();
    }
    private void Update()
    {
        // Blink description text
        if (_timer < _maxTimer)
            _timer += blinkSpeed * Time.deltaTime;
        else
        {
            Blink();
            _timer = 0;
        }
        // ----------------------------------


        // Glitch effects
        if (_fxTimer < _fxMaxTimer)
            _fxTimer += Time.deltaTime;
        else
        {
            CameraFx.SetTrigger("glitch");
            _fxTimer = Random.Range(0, 15);
        }
        // ----------------------------------


        // Fade menu ------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space) && !startLerp)
        {
            startLerp = true;
            _color = blackColor;

            
        }

        if (startLerp)
        {
            _color = Color.Lerp(_color, greenColor, lerpSpeed * Time.deltaTime);            
            background.color = new Color(_color.r, _color.g, _color.b, _alpha);
            //if (_color.g > 0.76f)
            //{
                _alpha = Mathf.Lerp(_alpha, 0, (lerpSpeed / 2) * Time.deltaTime);
                Color rgba = new Color(titleText.color.r, titleText.color.g, titleText.color.b, _alpha);
                titleText.color = rgba;
                descText.color = rgba;

                if (rgba.a <= 0.1f)
                {
                    descText.gameObject.SetActive(false);
                    titleText.gameObject.SetActive(false);
                }
            //}
        }
        // ---------------------------------------------------
    }




    private void Blink()
    {
        descText.enabled = descText.enabled ? false : true;
    }
}
