using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public Text inputText;

    private bool canBlink = false;
    private float timer = 0;
    public float blinkSpeed = 1f;


    private void Awake()
    {
        inputText.gameObject.SetActive(false);
    }
    private void Start()
    {
        StartCoroutine(StartTimer());
    }
    private void Update()
    {
        if (Input.anyKey)
            SceneManager.LoadScene(0);

        if (canBlink)
        {
            Blink();
        }
    }



    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(10f);
        inputText.gameObject.SetActive(true);
        canBlink = true;

    }
    private void Blink()
    {
        if (timer < 1)
            timer += blinkSpeed * Time.deltaTime;
        else
        {
            inputText.gameObject.SetActive(!inputText.gameObject.activeSelf);
            timer = 0;
        }
    }
}
