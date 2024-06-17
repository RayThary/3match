using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private int x;
    [SerializeField] private int y;
    public int GetY { get { return y; } }

    private Vector2 touchOnePos;
    private Vector2 touchTwoPos;

    private GameObject oneObj;

    private int tempX;
    private int tempY;

    private GameStart gameStart;

    private bool moveChange = false;
    public bool SetMoveChange { set => moveChange = value; }

    private bool moveCheck = false;//좌우움직임
    private bool returnMove = false;//좌우움직임 이후 다시되돌아갈건지에관한내용

    private bool moveDownCheck = false;//아래로만움직임

    private bool clickCheck = false;//첫번째 두번째 클릭해줬는지 체크해주고 어딜클릭했었는지 알려주기위한곳

    private bool createCheck = false;//이오브젝트가 추가로생성된오브젝트인지 알려주는곳



    private SpriteRenderer spr;
    private Color color;

    [SerializeField] private bool matchCheck = false;
    private bool matchStart = false;

    private void OnMouseDown()
    {
        if (gameStart.GetMoving() == true)
        {
            return;
        }

        if (gameStart.getFristTouch == false)
        {
            //첫번째클릭인지 체크
            clickCheck = true;
            gameStart.SetOneObj = gameObject;
            gameStart.setFristTouch = true;

        }
        else
        {
            gameStart.setFristTouch = false;
            gameStart.SetTwoObj = gameObject;
            oneObj = gameStart.GetOneObj;
            //첫번째가 골라진상태에서 두번째 체크시 첫번째 오브젝트와 두번째 오브젝트의 위치를 받아주고 교환가능한지 체크를해준다
            touchOnePos = gameStart.GetOneObj.transform.position;
            touchTwoPos = transform.position;
            objChangeCheck();


        }
    }
    private void objChangeCheck()
    {
        if (touchTwoPos.x == touchOnePos.x - 1 && touchOnePos.y == touchTwoPos.y)
        {
            moveChange = true;
            oneObj.GetComponent<Obj>().SetMoveChange = true;
        }
        else if (touchTwoPos.x == touchOnePos.x + 1 && touchOnePos.y == touchTwoPos.y)
        {
            moveChange = true;
            oneObj.GetComponent<Obj>().SetMoveChange = true;
        }
        else if (touchTwoPos.y == touchOnePos.y - 1 && touchOnePos.x == touchTwoPos.x)
        {
            moveChange = true;
            oneObj.GetComponent<Obj>().SetMoveChange = true;
        }
        else if (touchTwoPos.y == touchOnePos.y + 1 && touchOnePos.x == touchTwoPos.x)
        {
            moveChange = true;
            oneObj.GetComponent<Obj>().SetMoveChange = true;
        }
        else
        {
            Debug.Log("이동불가");
        }

        if (moveChange)
        {
            gameStart.SetMoving(true);
            x = oneObj.GetComponent<Obj>().GetXY(true);
            y = oneObj.GetComponent<Obj>().GetXY(false);
            oneObj.GetComponent<Obj>().SetXY(tempX, tempY);

            gameStart.allObjs[x, y] = gameObject;
            gameStart.allObjs[tempX, tempY] = oneObj;
        }

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                gameStart.allObjs[x, y].GetComponent<Obj>().SetMatchStart(true);
            }
        }






    }

    void Start()
    {
        gameStart = FindObjectOfType<GameStart>();
        spr = GetComponent<SpriteRenderer>();
        color = spr.color;
        if (createCheck == false)
        {
            x = (int)transform.position.x;
            y = (int)transform.position.y;
            tempX = x;
            tempY = y;
        }

    }

    // Update is called once per frame
    void Update()
    {

        findMatch();
        if (matchCheck)
        {
            spr.color = new Color(1, 1, 1);
        }
        else
        {
            objClickChekc();
        }

        objChangeMove();
        objDownMove();
    }

    private void objClickChekc()
    {
        if (clickCheck)
        {
            color.a = 0.5f;
            spr.color = color;

            if (gameStart.getFristTouch == false)
            {
                clickCheck = false;
            }
        }
        else
        {
            color.a = 1;
            spr.color = color;
        }
    }




    private void objChangeMove()
    {
        if (moveChange)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * moveSpeed);
            if (transform.position == new Vector3(x, y, 0))
            {
                if (gameStart.GetOneObj.GetComponent<Obj>().GetMatchCheck() == false && gameStart.GetTwoObj.GetComponent<Obj>().GetMatchCheck() == false)
                {


                    gameStart.allObjs[x, y] = oneObj;
                    gameStart.allObjs[tempX, tempY] = gameObject;
                    x = tempX;
                    y = tempY;
                    returnMove = true;

                }
                else
                {
                    tempX = x;
                    tempY = y;
                    moveChange = false;
                    transform.name = "(" + x + "," + y + ")";
                    gameStart.SetMoving(false);
                }
            }
        }


        if (returnMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * moveSpeed * moveSpeed);
            if (transform.position == new Vector3(x, y, 0))
            {
                returnMove = true;
                gameStart.SetMoving(false);
            }
        }
    }


    private void objDownMove()
    {
        if (moveDownCheck)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * 5);
            if (transform.position == new Vector3(x, y, 0))
            {
                moveDownCheck = false;
                tempX = x;
                tempY = y;
                transform.name = "(" + x + "," + y + ")";
                gameStart.SetMoving(false);
            }
        }
    }


    private void findMatch()
    {
        if (matchStart)
        {
            if (moveDownCheck || moveChange)
            {
                return;
            }

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

                        gameStart.AddDestrtoyObj(leftObj);
                        gameStart.AddDestrtoyObj(rightObj);
                        gameStart.AddDestrtoyObj(gameObject);
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


                        gameStart.AddDestrtoyObj(downObj);
                        gameStart.AddDestrtoyObj(upObj);
                        gameStart.AddDestrtoyObj(gameObject);
                    }
                }
            }

            if (x == 8 && y == 8)//xy의마지막값
            {
                gameStart.DestroyCheck = true;
            }

            matchStart = false;
        }
    }

    public void SetXY(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    /// <summary>
    /// true일땐 x가 리턴 false 일땐 y가리턴
    /// </summary>
    /// <param name="_value">true일땐 x가 리턴 false 일땐 y가리턴</param>
    /// <returns></returns>
    public int GetXY(bool _value)
    {
        if (_value)
        {
            return x;
        }
        else
        {
            return y;
        }
    }

    public void SetChangeMove()
    {
        moveChange = true;
    }

    public bool GetMatchCheck()
    {
        return matchCheck;
    }

    public void IsMatch()
    {
        matchCheck = true;
    }

    public void SetMatchStart(bool _value)
    {
        matchStart = _value;
    }
    public void SetMoveDown(int _x, int _y, bool _value)
    {
        x = _x;
        y = _y;
        createCheck = _value;
        moveDownCheck = true;
    }

}
