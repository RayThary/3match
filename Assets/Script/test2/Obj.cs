using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private int x;
    [SerializeField] private int y;

    private Vector2 touchOnePos;
    private Vector2 touchTwoPos;


    private int tempX;
    private int tempY;

    private GameStart gameStart;

    private bool returnMove = false;

    private bool moveCheck = false;
    private bool clickCheck = false;

    private bool firstTouchCheck = false;
    private int oneX;
    private int oneY;
    private int twoX;
    private int twoY;

    [SerializeField] private bool isFirst = false;
    [SerializeField] private bool isSecond = false;

    private SpriteRenderer spr;
    private Color color;

    [SerializeField] private bool matchCheck = false;

    private void OnMouseDown()
    {
        if (gameStart.GetMoving == true)
        {
            return;
        }

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        pos.x = Mathf.Floor(pos.x);
        pos.y = Mathf.Floor(pos.y);


        firstTouchCheck = gameStart.GetfirstTouch();

        if (firstTouchCheck)
        {
            gameStart.GetOneObj.GetComponent<Obj>().SetClickCheck(false);
            twoX = (int)pos.x;
            twoY = (int)pos.y;
            gameStart.SetTwoObj = transform.gameObject;

            isSecond = true;
            objChange();

            gameStart.SetfirstTouch(false);
        }
        else
        {
            clickCheck = true;

            oneX = (int)pos.x;
            oneY = (int)pos.y;
            isFirst = true;
            gameStart.SetOneObj = transform.gameObject;
            gameStart.SetfirstTouch(true);

        }




    }

    void Start()
    {
        gameStart = FindObjectOfType<GameStart>();
        spr = GetComponent<SpriteRenderer>();
        color = spr.color;
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        tempX = x;
        tempY = y;

        findMatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirst && isSecond)
        {
            Debug.Log("두개의 true");
        }
        //mouseDownCheck();
        if (matchCheck)
        {
            spr.color = new Color(1, 1, 1);
        }
        else
        {
            objClickChekc();
        }

        objMove();
    }

    private void objClickChekc()
    {
        if (clickCheck)
        {

            color.a = 0.5f;
            spr.color = color;
        }
        else
        {
            color.a = 1;
            spr.color = color;
        }
    }




    private void objMove()
    {
        if (moveCheck)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * moveSpeed);

            if (transform.position == new Vector3(x, y, 0))
            {
                if (isFirst || isSecond)
                {
                    gameStart.allObjs[x, y] = transform.gameObject;

                    findMatch();
                    if (isFirst)
                    {
                        if (gameStart.GetTwoObj.GetComponent<Obj>().GetMatchCheck() || matchCheck)
                        {
                            tempX = x;
                            tempY = y;
                        }
                        else
                        {
                            x = tempX;
                            y = tempY;
                            gameStart.allObjs[x, y] = transform.gameObject;
                        }
                    }
                    else if (isSecond)
                    {
                        if (gameStart.GetOneObj.GetComponent<Obj>().GetMatchCheck() || matchCheck)
                        {
                            tempX = x;
                            tempY = y;
                        }
                        else
                        {
                            x = tempX;
                            y = tempY;
                            gameStart.allObjs[x, y] = transform.gameObject;
                        }
                    }

                    isFirst = false;
                    isSecond = false;
                    gameStart.SetMoveCheck(false);
                }

                if (transform.position == new Vector3(x, y, 0))
                {
                    moveCheck = false;
                }



                #region
                //objReturnCheck();

                //if (returnMove && transform.position == new Vector3(x, y, 0))
                //{
                //    gameStart.allObjs[x, y] = transform.gameObject;

                //    gameStart.SetMoveCheck(false);
                //    returnMove = false;
                //    moveCheck = false;
                //}
                //game
                #endregion
            }


        }
    }

    private void objReturnCheck()
    {
        if (gameStart.allObjs[tempX, tempY].GetComponent<Obj>().GetMatchCheck() == true || matchCheck == true)
        {
            returnMove = false;
            tempX = x;
            tempY = y;

            transform.name = "(" + x + "," + y + ")";

            gameStart.SetMoveCheck(false);
            moveCheck = false;
        }
        else
        {
            returnMove = true;

            x = tempX;
            y = tempY;

        }
    }

    private void mouseDownCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameStart.GetMoving == true)
            {

                return;
            }

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            if (pos.x >= 0 && pos.x < gameStart.X && pos.y >= 0 && pos.y < gameStart.Y)
            {
                pos.x = Mathf.Floor(pos.x);
                pos.y = Mathf.Floor(pos.y);

                if (transform.position == new Vector3(pos.x, pos.y, 0))
                {
                    firstTouchCheck = gameStart.GetfirstTouch();


                    if (firstTouchCheck)
                    {
                        gameStart.GetOneObj.GetComponent<Obj>().SetClickCheck(false);
                        twoX = (int)pos.x;
                        twoY = (int)pos.y;
                        gameStart.SetTwoObj = transform.gameObject;

                        isSecond = true;
                        objChange();

                        gameStart.SetfirstTouch(false);
                    }
                    else
                    {
                        clickCheck = true;

                        oneX = (int)pos.x;
                        oneY = (int)pos.y;
                        isFirst = true;
                        gameStart.SetOneObj = transform.gameObject;
                        gameStart.SetfirstTouch(true);

                    }
                }


            }
        }
    }

    private void objChange()
    {

        touchOnePos = gameStart.GetOneObj.transform.position;
        touchTwoPos = transform.position;


        if (touchTwoPos.x == touchOnePos.x - 1 && touchOnePos.y == touchTwoPos.y)
        {
            gameStart.GetOneObj.GetComponent<Obj>().SetXY(x, y);
            x = (int)touchOnePos.x;
            y = (int)touchOnePos.y;

            gameStart.SetMoveCheck(true);
            gameStart.GetOneObj.GetComponent<Obj>().SetMoveCheck(true);
            moveCheck = true;
        }
        else if (touchTwoPos.x == touchOnePos.x + 1 && touchOnePos.y == touchTwoPos.y)
        {

            gameStart.GetOneObj.GetComponent<Obj>().SetXY(x, y);
            x = (int)touchOnePos.x;
            y = (int)touchOnePos.y;

            gameStart.SetMoveCheck(true);
            gameStart.GetOneObj.GetComponent<Obj>().SetMoveCheck(true);
            moveCheck = true;
        }
        else if (touchTwoPos.y == touchOnePos.y - 1 && touchOnePos.x == touchTwoPos.x)
        {

            gameStart.GetOneObj.GetComponent<Obj>().SetXY(x, y);
            x = (int)touchOnePos.x;
            y = (int)touchOnePos.y;

            gameStart.SetMoveCheck(true);
            gameStart.GetOneObj.GetComponent<Obj>().SetMoveCheck(true);
            moveCheck = true;
        }
        else if (touchTwoPos.y == touchOnePos.y + 1 && touchOnePos.x == touchTwoPos.x)
        {

            gameStart.GetOneObj.GetComponent<Obj>().SetXY(x, y);
            x = (int)touchOnePos.x;
            y = (int)touchOnePos.y;

            gameStart.SetMoveCheck(true);
            gameStart.GetOneObj.GetComponent<Obj>().SetMoveCheck(true);
            moveCheck = true;
        }
        else
        {
            gameStart.SetMoveCheck(false);
            isFirst = false;
            isSecond = false;

            moveCheck = false;
            Debug.Log("이동불가");
        }
    }

    private void findMatch()
    {

        if (x > 0 && x < gameStart.X - 1)
        {
            GameObject leftObj = gameStart.allObjs[x - 1, y];
            GameObject rightObj = gameStart.allObjs[x + 1, y];
            if (leftObj != transform && rightObj != transform && leftObj != rightObj)
            {

                if (leftObj.tag == this.gameObject.tag && rightObj.tag == this.gameObject.tag)
                {
                    leftObj.GetComponent<Obj>().IsMatch();
                    rightObj.GetComponent<Obj>().IsMatch();
                    matchCheck = true;

                    gameObject.tag = "tempDestroy";
                    leftObj.tag = "tempDestroy";
                    rightObj.tag = "tempDestroy";
                }
            }
        }

        if (y > 0 && y < gameStart.Y - 1)
        {
            GameObject downObj = gameStart.allObjs[x, y - 1];
            GameObject upObj = gameStart.allObjs[x, y + 1];
            if (downObj != transform && upObj != transform && downObj != upObj)
            {
                if (downObj.tag == this.gameObject.tag && upObj.tag == this.gameObject.tag)
                {
                    downObj.GetComponent<Obj>().IsMatch();
                    upObj.GetComponent<Obj>().IsMatch();
                    matchCheck = true;


                    gameObject.tag = "tempDestroy";
                    downObj.tag = "tempDestroy";
                    upObj.tag = "tempDestroy";
                }
            }
        }
    }

    public void SetXY(int _x, int _y)
    {
        x = _x;
        y = _y;
    }


    public bool GetMatchCheck()
    {
        return matchCheck;
    }

    public void SetMoveCheck(bool _value)
    {
        moveCheck = _value;
    }

    public void SetClickCheck(bool _value)
    {
        clickCheck = _value;
    }

    public void IsMatch()
    {
        matchCheck = true;
    }
}
