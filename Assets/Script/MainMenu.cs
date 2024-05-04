using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Button btnStart;
    private Button btnEnd;

    void Start()
    {
        btnStart = transform.GetChild(0).GetComponent<Button>();
        btnEnd = transform.GetChild(1).GetComponent<Button>();

        btnStart.onClick.AddListener(() => ButtonStart());
        btnEnd.onClick.AddListener(() => ButtonEnd());
    }
    private void ButtonStart()
    {
        SceneManager.LoadScene(1);
    }
  
    private void ButtonEnd()
    {
        Application.Quit();
    }
}
