using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using System;

public class EffectsManager : MonoBehaviour
{
    public Animator Animator { get; private set; }
    public Camera Camera { get; private set; }
    public Animator CamAnimation { get; private set; }
    public AnalogGlitch Analog { get; private set; }
    public DigitalGlitch Digital { get; private set; }

    // Glitch effect
    private float _fxTimer = 30;
    private float _fxMaxTimer = 30;



    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Camera = Camera.main;
        CamAnimation = Camera.main.GetComponent<Animator>();
        Analog = CamAnimation.GetComponent<AnalogGlitch>();
        Digital = CamAnimation.GetComponent<DigitalGlitch>();
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

    public void RunGameOver()
    {
        Animator.SetTrigger("GameOver");
    }
    public bool IsGameOverFinished()
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("GameOver") && stateInfo.normalizedTime >= 1.0f;
    }
    
    public void FadeInOverlay()
    {
        Animator.SetTrigger("FadeIn");
    }
    public void FadeOutOverlay()
    {
        Animator.SetTrigger("FadeOut");
    }
}
