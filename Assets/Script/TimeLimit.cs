using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimit : MonoBehaviour
{
    private Image image;
    private float timer;
    private float maxTime;

    private bool gameStart;

    void Start()
    {
        image = GetComponent<Image>();
        timer = Board.Instance.GetTimer;
        maxTime = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                image.fillAmount = timer / maxTime;
            }
            else
            {
                Board.Instance.SetIsFail();
                gameStart = false;
            }
            
        }
    }

    public void SetTimerStart()
    {
        gameStart = true;
    }

    
}
