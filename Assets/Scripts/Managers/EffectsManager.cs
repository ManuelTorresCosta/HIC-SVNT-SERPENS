using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using System;

public class EffectsManager : MonoBehaviour
{
    public Animator CamAnimation { get; private set; }
    public AnalogGlitch Analog { get; private set; }
    public DigitalGlitch Digital { get; private set; }
    public Transition Transition { get; private set; }

    // Glitch effect
    private float _fxTimer = 30;
    private float _fxMaxTimer = 30;



    private void Awake()
    {
        CamAnimation = Camera.main.GetComponent<Animator>();
        Analog = CamAnimation.GetComponent<AnalogGlitch>();
        Digital = CamAnimation.GetComponent<DigitalGlitch>();

        Transition = GetComponentInChildren<Transition>();
    }



    public void GlitchEffect()
    {
        if (_fxTimer < _fxMaxTimer)
            _fxTimer += Time.deltaTime;
        else
        {
            CamAnimation.SetTrigger("glitch");
            _fxTimer = UnityEngine.Random.Range(0, 15);
        }
    }
    public void ColorFadeEffect()
    {
        CamAnimation.SetTrigger("colorFade");
    }
    public void RunTransition(Action callback)
    {
        StartCoroutine(Transition.RunTransition(() =>
        {
            callback();
        }));
    }
}
