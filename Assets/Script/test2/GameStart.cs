using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameStart : MonoBehaviour
{
    [SerializeField] private int width;
    public int X { get { return width; } }
    [SerializeField] private int height;
    public int Y { get { return height; } }
    [SerializeField] private GameObject[] blocksObj;

    public GameObject[,] allObjs;

    private GameObject tempOneObj;
    public GameObject SetOneObj { set => tempOneObj = value; }
    public GameObject GetOneObj { get { return tempOneObj; } }

    private GameObject tempTwoObj;
    public GameObject SetTwoObj { set => tempTwoObj = value; }
    public GameObject GetTwoObj { get { return tempTwoObj; } }

    private bool firstTouch = false;

    private bool movingCheck = false;
    private bool SetMoving { set => moveCheck = value; }
    public bool GetMoving { get { return moveCheck; } }
    //삭제할예정 부모에서 모든오브젝트의 움직임을 통제하기위한변수
    private bool fristTouchCheck = false;//첫번째 클릭 두번쨰 클릭 체크하기위한곳
    private Vector2 touchOnePos;
    private Vector2 touchTwoPos;

    private int oneX;
    private int oneY;
    private int twoX;
    private int twoY;

    private bool oneMatch = false;
    private bool twoMatch = false;

    [SerializeField] private bool moveCheck = false;



    private void OnMouseDown()
    {
        if (moveCheck)
        {
            Debug.Log("오브젝트 이동중");
            return;
        }

        if (fristTouchCheck)
        {
            allObjs[oneX, oneY].GetComponent<Obj>().SetClickCheck(false);
            touchTwoPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchTwoPos.x = Mathf.Floor(touchTwoPos.x);
            touchTwoPos.y = Mathf.Floor(touchTwoPos.y);
            twoX = (int)touchTwoPos.x;
            twoY = (int)touchTwoPos.y;
            objChange();
            fristTouchCheck = false;
        }
        else
        {
            touchOnePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchOnePos.x = Mathf.Floor(touchOnePos.x);
            touchOnePos.y = Mathf.Floor(touchOnePos.y);
            oneX = (int)touchOnePos.x;
            oneY = (int)touchOnePos.y;
            Debug.Log($"{oneX},{oneY}");
            allObjs[oneX, oneY].GetComponent<Obj>().SetClickCheck(true);
            fristTouchCheck = true;
        }
    }


    private void objChange()
    {
        if (touchTwoPos.x == touchOnePos.x - 1 && touchOnePos.y == touchTwoPos.y)
        {

            tempOneObj = allObjs[oneX, oneY];
            tempTwoObj = allObjs[twoX, twoY];

            allObjs[oneX, oneY] = tempTwoObj;
            allObjs[twoX, twoY] = tempOneObj;

            tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
            tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);

            tempOneObj.GetComponent<Obj>().SetMoveCheck(true);
            tempTwoObj.GetComponent<Obj>().SetMoveCheck(true);

            moveCheck = true;
        }
        else if (touchTwoPos.x == touchOnePos.x + 1 && touchOnePos.y == touchTwoPos.y)
        {
            tempOneObj = allObjs[oneX, oneY];
            tempTwoObj = allObjs[twoX, twoY];

            allObjs[oneX, oneY] = tempTwoObj;
            allObjs[twoX, twoY] = tempOneObj;

            tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
            tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);

            tempOneObj.GetComponent<Obj>().SetMoveCheck(true);
            tempTwoObj.GetComponent<Obj>().SetMoveCheck(true);



            moveCheck = true;
        }
        else if (touchTwoPos.y == touchOnePos.y - 1 && touchOnePos.x == touchTwoPos.x)
        {
            tempOneObj = allObjs[oneX, oneY];
            tempTwoObj = allObjs[twoX, twoY];

            allObjs[oneX, oneY] = tempTwoObj;
            allObjs[twoX, twoY] = tempOneObj;

            tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
            tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);

            tempOneObj.GetComponent<Obj>().SetMoveCheck(true);
            tempTwoObj.GetComponent<Obj>().SetMoveCheck(true);

            moveCheck = true;
        }
        else if (touchTwoPos.y == touchOnePos.y + 1 && touchOnePos.x == touchTwoPos.x)
        {
            tempOneObj = allObjs[oneX, oneY];
            tempTwoObj = allObjs[twoX, twoY];

            allObjs[oneX, oneY] = tempTwoObj;
            allObjs[twoX, twoY] = tempOneObj;

            tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
            tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);

            tempOneObj.GetComponent<Obj>().SetMoveCheck(true);
            tempTwoObj.GetComponent<Obj>().SetMoveCheck(true);

            moveCheck = true;
        }
        else
        {
            Debug.Log("이동불가");
   
        }

       




        tempOneObj = null;
        tempTwoObj = null;


    }
    private void Start()
    {
        allObjs = new GameObject[width, height];

        startCreate();
    }

    private void startCreate()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int objNum = Random.Range(0, blocksObj.Length);

                Vector2 objPos = new Vector2(x, y);
                GameObject obj = Instantiate(blocksObj[objNum], transform);
                obj.transform.position = objPos;
                obj.name = "(" + x + "," + y + ")";
                allObjs[x, y] = obj;

            }
        }
    }
    private void Update()
    {
        
    }
    private void mouseDownCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height)
            {

                if (moveCheck)
                {
                    Debug.Log("오브젝트 이동중");
                    return;
                }






                if (fristTouchCheck)
                {
                    allObjs[oneX, oneY].GetComponent<Obj>().SetClickCheck(false);
                    touchTwoPos = pos;
                    touchTwoPos.x = Mathf.Floor(touchTwoPos.x);
                    touchTwoPos.y = Mathf.Floor(touchTwoPos.y);
                    twoX = (int)touchTwoPos.x;
                    twoY = (int)touchTwoPos.y;
                    objChange();
                    fristTouchCheck = false;
                }
                else
                {
                    touchOnePos = pos;
                    touchOnePos.x = Mathf.Floor(touchOnePos.x);
                    touchOnePos.y = Mathf.Floor(touchOnePos.y);
                    oneX = (int)touchOnePos.x;
                    oneY = (int)touchOnePos.y;
                    if (allObjs[oneX, oneY].tag == "tempDestroy")
                    {
                        return;
                    }

                    allObjs[oneX, oneY].GetComponent<Obj>().SetClickCheck(true);
                    fristTouchCheck = true;
                }
            }
        }
    }

    private IEnumerator checkMoveCo()
    {
        yield return new WaitForSeconds(0.5f);

    }


    public void SetMoveCheck(bool _value)
    {
        moveCheck = _value;
    }

    public void SetfirstTouch(bool _value)
    {
        firstTouch = _value;
    }
    public bool GetfirstTouch()
    {
        return firstTouch;
    }

}
