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



    private void Awake()
    {
        CamAnimation = Camera.main.GetComponent<Animator>();
        Analog = CamAnimation.GetComponent<AnalogGlitch>();
        Digital = CamAnimation.GetComponent<DigitalGlitch>();

        Transition = GetComponentInChildren<Transition>();
    }



    public void GlitchEffect()
    {
        CamAnimation.SetTrigger("glitch");
    }
    public void ColorFadeEffect()
    {
        CamAnimation.SetTrigger("colorFade");
    }
    public void RunTransition(Action callback)
    {
        StartCoroutine(Transition.RunTransition(callback));
    }
}
