using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class EffectsManager : MonoBehaviour
{
    public Animator CamAnimation { get; private set; }
    public AnalogGlitch Analog { get; private set; }
    public DigitalGlitch Digital { get; private set; }



    private void Awake()
    {
        CamAnimation = Camera.main.GetComponent<Animator>();
        Analog = CamAnimation.GetComponent<AnalogGlitch>();
        Digital = CamAnimation.GetComponent<DigitalGlitch>();
    }



    public void GlitchEffect()
    {
        CamAnimation.SetTrigger("glitch");
    }

    public void ColorFadeEffect()
    {
        CamAnimation.SetTrigger("colorFade");
    }
}
