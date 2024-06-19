using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private GameObject option;
    private bool stop = false;

    [SerializeField] private Button playing;
    [SerializeField] private Button reStart;
    [SerializeField] private Button exit;

    void Start()
    {

        playing = option.transform.GetChild(0).GetComponent<Button>();
        reStart = option.transform.GetChild(1).GetComponent<Button>();
        exit = option.transform.GetChild(2).GetComponent<Button>();

        playing.onClick.AddListener(btnPlaying);
        reStart.onClick.AddListener(btnReStart);
        exit.onClick.AddListener(btnExit);

    }


    private void btnPlaying()
    {
        option.SetActive(false);
        stop = false;
        Time.timeScale = 1;
    }

    private void btnReStart()
    {
        int NowSceneNum = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(NowSceneNum);
        SceneManager.LoadSceneAsync(NowSceneNum);
        Time.timeScale = 1;
    }

    private void btnExit()
    {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (stop == false)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                option.SetActive(true);
                stop = true;
                Time.timeScale = 0;

            }
        }

    }
}
