using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BlockType
{
    Orange,
    Blue,
    Green,
    Pink,
    Crown,
    Purple,
    Cream,
}

public class BlockObj : MonoBehaviour
{
    public BlockType type;
    [SerializeField] private float boardDownSpeed = 4;
    [SerializeField] private float blockChangeSpeed = 4;

    [SerializeField] private int x;
    [SerializeField] private int y;

    private bool isMatch = false;
    private bool isLast = false;

    private Vector2 changePos;
    private Vector2 targetPos;//다운무브용 


    private bool isMoving = false;
    private bool downMove = false;
    

    private bool swapCheck = false;
    private bool swapReturnCheck = false;

    private bool swapMove;

    private bool isBoardCheck = false;
    private bool returnMove = false;


    [SerializeField] private bool matchCheck = false;
    public bool SetMatchCheck { set { matchCheck = value; } }
    public bool GetMatchCheck { get { return matchCheck; } }

    private SpriteRenderer spr;
    private Color sprA;


    public void SetXY(int _x, int _y)
    {
        x = _x;
        y = _y;
    }


    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        sprA = spr.color;
    }

    // Update is called once per frame
    void Update()
    {
        dwonMoving();
        swapMoving();
    }

    private void dwonMoving()
    {
        if (downMove)
        {

            isMoving = true;

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * boardDownSpeed);
            if (transform.position.y == y)
            {
                downMove = false;
                isMoving = false;
            }
        }
 
    }

    private void swapMoving()
    {
        if (swapCheck)
        {
            if (swapMove)
            {

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * blockChangeSpeed);
                if (transform.position == new Vector3(x, y, 0))
                {
                    swapReturn();
                    swapMove = false;
                }
            }
            if (returnMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * blockChangeSpeed);

                if (transform.position == new Vector3(x, y, 0))
                {
                    if (swapReturnCheck)
                    {
                        Board.Instance.SetBoardOutCheck();

                    }
                    returnMove = false;
                    swapCheck = false;
                    swapReturnCheck = false;
                    
                }
            }

        }

    }

    private void swapReturn()
    {
        if (swapReturnCheck)
        {
            Board.Instance.SwapCheckBoard();
            swapMatchCheck();
        }
    }

    private void swapMatchCheck()
    {
        GameObject second = Board.Instance.GetSecondBlock;

        if (matchCheck == true || second.GetComponent<BlockObj>().GetMatchCheck == true)
        {
            transform.name = "(" + x + ", " + y + ")";
            second.name = "(" + (int)second.transform.position.x + ", " + (int)second.transform.position.y + ")";
            swapCheck = false;
            swapReturnCheck = false;
        }
        else
        {
            targetPos = second.transform.position;
            second.GetComponent<BlockObj>().SetXY(x, y);
            Board.Instance.blockArray[x, y] = second;
            x = (int)targetPos.x;
            y = (int)targetPos.y;
            Board.Instance.blockArray[x, y] = gameObject;


            returnMove = true;
            second.GetComponent<BlockObj>().SetReturnMove();
        }
    }

    public void startCheckMatch()
    {
        if (x > 0 && x < Board.Instance.X - 1)
        {
            GameObject leftObj = Board.Instance.blockArray[x - 1, y];
            GameObject rightObj = Board.Instance.blockArray[x + 1, y];
            if (leftObj != null && rightObj != null)
            {
                if (leftObj.GetComponent<BlockObj>().type == type && rightObj.GetComponent<BlockObj>().type == type)
                {
                    matchCheck = true;
                }
            }
        }
        if (y > 0 && y < Board.Instance.Y - 1)
        {
            GameObject downObj = Board.Instance.blockArray[x, y - 1];
            GameObject upObj = Board.Instance.blockArray[x, y + 1];

            if (downObj != null && upObj != null)
            {
                if (downObj.GetComponent<BlockObj>().type == type && upObj.GetComponent<BlockObj>().type == type)
                {
                    matchCheck = true;

                }
            }
        }
    }

    public void checkMatch()
    {

        if (x > 0 && x < Board.Instance.X - 1)
        {
            GameObject leftObj = Board.Instance.blockArray[x - 1, y];
            GameObject rightObj = Board.Instance.blockArray[x + 1, y];
            if (leftObj != null && rightObj != null)
            {
                if (leftObj.GetComponent<BlockObj>().type == type && rightObj.GetComponent<BlockObj>().type == type)
                {
                    leftObj.GetComponent<BlockObj>().SetMatchCheck = true;
                    rightObj.GetComponent<BlockObj>().SetMatchCheck = true;
                    matchCheck = true;
                }
            }
        }
        if (y > 0 && y < Board.Instance.Y - 1)
        {
            GameObject downObj = Board.Instance.blockArray[x, y - 1];
            GameObject upObj = Board.Instance.blockArray[x, y + 1];

            if (downObj != null && upObj != null)
            {
                if (downObj.GetComponent<BlockObj>().type == type && upObj.GetComponent<BlockObj>().type == type)
                {
                    downObj.GetComponent<BlockObj>().SetMatchCheck = true;
                    upObj.GetComponent<BlockObj>().SetMatchCheck = true;
                    matchCheck = true;

                }
            }
        }
        if (isBoardCheck == false)
        {
            if (isLast)
            {
                Board.Instance.removeBoard();
            }
        }
    }

    #region
    //private void returnMoveCheck()
    //{
    //    GameObject secondObj = Board.Instance.blockArray[x, y];
    //    if (matchCheck == true || Board.Instance.blockArray[x, y].GetComponent<BlockObj>().GetMatchCheck == true)
    //    {
    //        secondObj.GetComponent<BlockObj>().SetXY(x, y);
    //        secondObj.transform.name = "(" + x + ", " + y + ")";

    //        x = (int)changePos.x;
    //        y = (int)changePos.y;
    //        transform.name = "(" + x + ", " + y + ")";
    //        Board.Instance.removeBoard();

    //    }
    //    else
    //    {
    //        returnMove = true;
    //        secondObj.GetComponent<BlockObj>().SetReturnMove(true);

    //    }
    //}
    #endregion


    public void SetFirstBlockClick()
    {
        sprA.a = 0.6f;
        spr.color = sprA;
    }
    public void SetSecondBlockClick()
    {
        sprA.a = 1;
        spr.color = sprA;
    }


    public void downMovingCheck(bool _value)
    {
        downMove = _value;
    }


      
    public void SetReturnCheck(bool _value)
    {
        swapReturnCheck = _value;
    }

    public void SetSwapCheck(bool _value)
    {
        swapMove = true;
        swapCheck = _value;
    }

    public void SetReturnMove()
    {
        returnMove = true;
    }

    public void SetisLast(bool _value)
    {
        isLast = _value;
    }
}

