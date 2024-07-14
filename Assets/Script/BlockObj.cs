using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public enum BlockType
{
    Orange,
    Blue,
    Green,
    Pink,
    Crown,
    Purple,
    Cream,
    BombLR,
    BombUD,
}

public class BlockObj : MonoBehaviour
{
    public BlockType type;
    [SerializeField] private float boardDownSpeed = 4;
    [SerializeField] private float blockChangeSpeed = 4;

    [SerializeField] private int x;
    [SerializeField] private int y;//내가보기위한 x ,y 좌표 나중에 안보이게해주면됨

    private int blockNum = 0;
    public int GetBlockNum { get { return blockNum; } }

    private bool isLast = false;

    private Vector2 targetPos;//다운무브용 
    private Vector2 nowPos;

    private bool downMove = false;

    private bool swapCheck = false;
    private bool swapReturnCheck = false;

    private bool swapMove;

    private bool returnMove = false;

    [SerializeField] private bool matchCheck = false;
    public bool SetMatchCheck { set { matchCheck = value; } }
    public bool GetMatchCheck { get { return matchCheck; } }

    [SerializeField] private bool isBombObjLR = false;
    [SerializeField] private bool isBombObjUD = false;

    private SpriteRenderer spr;
    private Color sprA;

    private int blockPoint = 1;

    private int beforeX, beforeY;
    public void SetXY(int _x, int _y)
    {
        x = _x;
        y = _y;
    }


    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        sprA = spr.color;
        if (type == BlockType.BombLR)
        {
            blockPoint = Board.Instance.X;
        }
        else if (type == BlockType.BombUD)
        {
            blockPoint = Board.Instance.Y;
        }

    }

    // Update is called once per frame
    void Update()
    {
        dwonMoving();
        swapMoving();

        if (matchCheck == true)
        {

            sprA.a = 0.5f;
            spr.color = sprA;
        }
    }

    private void dwonMoving()
    {
        if (downMove)
        {

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(x, y), Time.deltaTime * boardDownSpeed);
            if (transform.position.y == y)
            {
                downMove = false;
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
                    returnMove = false;
                    swapCheck = false;
                    if (swapReturnCheck)
                    {
                        Board.Instance.SetBlockClick(false);
                        swapReturnCheck = false;
                    }

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
            Board.Instance.removeBoard();
        }
        else
        {
            second.GetComponent<BlockObj>().SetReturnXY();
            Board.Instance.blockArray[x, y] = second;
            x = beforeX;
            y = beforeY;
            Board.Instance.blockArray[x, y] = gameObject;   
            second.GetComponent<BlockObj>().SetReturnMove();
            returnMove = true;
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

    }





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

    public void SetBlockNum(int _BlockNumber)
    {
        blockNum = _BlockNumber;
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
        beforeX = (int)transform.position.x;
        beforeY = (int)transform.position.y;
        swapMove = true;
        swapCheck = _value;
    }

    public void SetReturnXY()
    {
        x = beforeX;
        y = beforeY;
    }

    public void SetReturnMove()
    {
        returnMove = true;
    }

    public void SetisLast(bool _value)
    {
        isLast = _value;
    }

    public void SetBlockPoint(int _value)
    {
        blockPoint = _value;
    }



    public void SetBombLRObj()
    {
        isBombObjLR = true;
    }
    public void SetBombUDObj()
    {
        isBombObjUD = true;
    }

    public bool GetBombLRObj()
    {
        return isBombObjLR;
    }

    public bool GetBombUDObj()
    {
        return isBombObjUD;
    }

    public bool CheckBlockPostion()
    {
        if (transform.position.x == x && transform.position.y == y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        Board.Instance.AddPoint(blockPoint);
    }

}

