using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Kino;
using System;

public class MenuManager : MonoBehaviour
{
    private enum State
    {
        MENU,
        TRANSITION
    }
    private State _state;

    public Animator Animator { get; private set; }
    public EffectsManager Effects;

    // Glitch effect
    private float _fxTimer = 30;
    private float _fxMaxTimer = 30;
    

    
    // Unity Functions
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _state = State.MENU;
    }



    // Functions
    public void Run()
    {
        switch (_state)
        {
            case State.MENU:
                GlitchEffect();
                break;

            case State.TRANSITION:
                break;
        }
    }
    public void StartTransition()
    {
        if (_state == State.MENU)
        {
            Animator.SetTrigger("Transition");
            _state = State.TRANSITION;
        }
    }
    public void EndTransition()
    {
        gameObject.SetActive(false);
    }

    private void GlitchEffect()
    {
        if (_fxTimer < _fxMaxTimer)
            _fxTimer += Time.deltaTime;
        else
        {
            Effects.GlitchEffect();
            _fxTimer = UnityEngine.Random.Range(0, 15);
        }
    }
}
