using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    private TextMeshProUGUI score;
    private int point;
    private int targetScore;

    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
        targetScore = Board.Instance.GetTargetScore;
    }

    // Update is called once per frame
    void Update()
    {
        point = Board.Instance.GetPoint;

        score.text = point + " / " + targetScore;

    }
}
