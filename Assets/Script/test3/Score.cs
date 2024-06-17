using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

    private TextMeshProUGUI score;
    private int point;

    void Start()
    {
        score = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        point = Board.Instance.GetPoint;
        score.text = point.ToString();

    }
}
