using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private GameObject sound;
    private Button btnStart;
    private Button btnOption;
    private Button btnExit;

    [SerializeField]private Button btnSoundExit;

    void Start()
    {
        btnStart = transform.GetChild(0).GetComponent<Button>();
        btnOption = transform.GetChild(1).GetComponent<Button>();
        btnExit = transform.GetChild(2).GetComponent<Button>();

        btnSoundExit= sound.GetComponentInChildren<Button>();

        btnStart.onClick.AddListener(startButton);
        btnOption.onClick.AddListener(optionButton);
        btnExit.onClick.AddListener(exitButton);
        btnSoundExit.onClick.AddListener(exitSoundButton);
    }

    private void startButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void optionButton()
    {
        sound.SetActive(true);
    }

    private void exitSoundButton()
    {
        sound.SetActive(false);
    }

    private void exitButton()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (sound.activeSelf == true)
        {
            btnStart.interactable = false;
            btnOption.interactable = false;
            btnExit.interactable = false;
        }
        else
        {
            btnStart.interactable = true;
            btnOption.interactable = true;
            btnExit.interactable = true;
        }
    }
}
