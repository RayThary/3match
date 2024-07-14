using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private bool isLastStage = false;
    [SerializeField] private GameObject optionWindow;
    [SerializeField] private GameObject soundWindow;
    [SerializeField] private GameObject clearWindow;
    [SerializeField] private GameObject failWindow;

    private bool stop = false;

    private Button option_playing;
    private Button option_sound;
    private Button option_reStart;
    private Button option_exit;

    private Button option_soundExit;

    private Button clear_nextStage;
    private Button clear_exit;

    private Button fail_reStart;
    private Button fail_exit;

    void Start()
    {
        //버튼위치찾기

        option_playing = optionWindow.transform.GetChild(0).GetComponent<Button>();
        option_sound = optionWindow.transform.GetChild(1).GetComponent<Button>();
        option_reStart = optionWindow.transform.GetChild(2).GetComponent<Button>();
        option_exit = optionWindow.transform.GetChild(3).GetComponent<Button>();

        option_soundExit = soundWindow.transform.GetChild(4).GetComponent<Button>();
        if (isLastStage)
        {
            clear_exit = clearWindow.transform.GetChild(2).GetComponent<Button>();
        }
        else
        {

        clear_nextStage = clearWindow.transform.GetChild(1).GetComponent<Button>();
        clear_exit = clearWindow.transform.GetChild(2).GetComponent<Button>();
        }

        fail_reStart = failWindow.transform.GetChild(0).GetComponent<Button>();
        fail_exit = failWindow.transform.GetChild(1).GetComponent<Button>();

        //버튼기능넣기
        option_playing.onClick.AddListener(btnPlaying);
        option_reStart.onClick.AddListener(btnReStart);
        option_exit.onClick.AddListener(btnExit);

        option_sound.onClick.AddListener(btnSound);
        option_soundExit.onClick.AddListener(btnSoundExit);
        if (isLastStage)
        {
            clear_exit.onClick.AddListener(btnExit);
        }
        else
        {
            clear_nextStage.onClick.AddListener(btnnextStage);
            clear_exit.onClick.AddListener(btnExit);
        }

        fail_reStart.onClick.AddListener(btnReStart);
        fail_exit.onClick.AddListener(btnExit);
    }


    private void btnPlaying()
    {
        optionWindow.SetActive(false);
        stop = false;
        Time.timeScale = 1;
    }

    private void btnSound()
    {
        soundWindow.SetActive(true);
    }

    private void btnSoundExit()
    {
        soundWindow.SetActive(false);
    }

    private void btnReStart()
    {
        int NowSceneNum = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(NowSceneNum);
    }

    private void btnExit()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }

    private void btnnextStage()
    {
        int NowSceneNum = SceneManager.GetActiveScene().buildIndex;
        NowSceneNum++;
        if (NowSceneNum > 3)
        {
            Debug.Log("마지막스테이지 클리어");
        }
        else
        {
            Board.Instance.SetPoint(0);
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync(NowSceneNum);
        }


    }
    void Update()
    {
        if (stop == false)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                optionWindow.SetActive(true);
                stop = true;
                Time.timeScale = 0;

            }
        }

    }

    public void IsClear()
    {
        stop = true;
        clearWindow.SetActive(true);
    }

    public void IsFail()
    {
        stop = true;
        failWindow.SetActive(true);
    }
}
