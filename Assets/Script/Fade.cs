using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private bool isFade = false;
    private Image fadeImage;
    private Color color;
    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = Color.black;

    }
    private void Start()
    {
        color = fadeImage.color;
    }

    void Update()
    {
        if (isFade)
        {
            Board.Instance.SetPoint(0);
            if (Board.Instance.GetTouchChec() == true)
            {
                color.a -= Time.deltaTime;
                fadeImage.color = color;

                if (fadeImage.color.a <= 0)
                {
                    fadeImage.enabled = false;
                    Invoke("gameStart", 0.1f);
                    isFade = false;
                }
            }
        }
    }

    private void gameStart()
    {
        TimeLimit timer = FindAnyObjectByType<TimeLimit>();
        timer.SetTimerStart();
    }

    public void FadeStart()
    {
        isFade = true;
    }
}