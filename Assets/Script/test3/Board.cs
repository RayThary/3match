using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public static Board Instance;

    public SpriteRenderer btn;//임시 클릭가능한지 보여주기위해 잠시만들어준곳
    [SerializeField] private int point = 0;
    public int GetPoint { get { return point; } }

    [SerializeField] private int width;
    public int X { get { return width; } }
    [SerializeField] private int height;
    public int Y { get { return height; } }
    [SerializeField] private GameObject[] blocksObj;
    public GameObject[,] blockArray;

    [SerializeField] private List<GameObject> removeBlock = new List<GameObject>();

    private bool downMoving = false;

    private bool boardOutCheck = false;//오브젝트가 클릭가능한지 체크하기위한곳
    private bool newBlockCheck = false;
    private bool objClickCheck = false;

    private bool swappingToucchCheck = false;//오브젝트가 안움직이고 바꾸기위한준비가되었는지 체크해주는부분

    private GameObject lastBlockObj;

    private bool firstClick = true;
    private GameObject firstBlock;
    private GameObject secondBlock;
    public GameObject GetSecondBlock { get { return secondBlock; } }

    private bool moveSwap = false;

    private bool swapMatchCheck = false;

    private int firstX = 0;
    private int firstY = 0;
    private int secondX = 0;
    private int secondY = 0;

    private bool isFade = true;

    private void Awake()
    {
        Instance = this;
        isFade = true;
    }

    private void OnMouseDown()
    {
        if (swappingToucchCheck)
        {
            if (firstClick == true)
            {
                //첫번째클릭인지 체크
                boardInClickCheck();


                firstClick = false;
            }
            else
            {
                boardOutCheck = false;
                boardInClickCheck();
                blockSwap();

                firstClick = true;
            }
        }
    }

    private void boardInClickCheck()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Floor(mousePos.x);
        mousePos.y = Mathf.Floor(mousePos.y);

        if (firstClick)
        {
            firstBlock = blockArray[(int)mousePos.x, (int)mousePos.y];
            firstX = (int)mousePos.x;
            firstY = (int)mousePos.y;
            firstBlock.GetComponent<BlockObj>().SetFirstBlockClick();
        }
        else
        {
            secondBlock = blockArray[(int)mousePos.x, (int)mousePos.y];
            secondX = (int)mousePos.x;
            secondY = (int)mousePos.y;
            firstBlock.GetComponent<BlockObj>().SetSecondBlockClick();
        }

    }

    private void blockSwap()
    {
        if (secondBlock.transform.position.x == firstBlock.transform.position.x - 1 && firstBlock.transform.position.y == secondBlock.transform.position.y)
        {
            moveSwap = true;
        }
        else if (secondBlock.transform.position.x == firstBlock.transform.position.x + 1 && firstBlock.transform.position.y == secondBlock.transform.position.y)
        {
            moveSwap = true;
        }
        else if (secondBlock.transform.position.y == firstBlock.transform.position.y - 1 && firstBlock.transform.position.x == secondBlock.transform.position.x)
        {
            moveSwap = true;
        }
        else if (secondBlock.transform.position.y == firstBlock.transform.position.y + 1 && firstBlock.transform.position.x == secondBlock.transform.position.x)
        {
            moveSwap = true;
        }
        else
        {
            Debug.Log("이동불가");
            boardOutCheck = true;
        }

        if (moveSwap)
        {
            blockArray[firstX, firstY] = secondBlock;
            blockArray[secondX, secondY] = firstBlock;
            firstBlock.GetComponent<BlockObj>().SetXY(secondX, secondY);
            secondBlock.GetComponent<BlockObj>().SetXY(firstX, firstY);

            firstBlock.GetComponent<BlockObj>().SetSwapCheck(true);
            secondBlock.GetComponent<BlockObj>().SetSwapCheck(true);

            firstBlock.GetComponent<BlockObj>().SetReturnCheck(true);
            moveSwap = false;
        }
    }



   

    void Start()
    {
        startCreate();
    }

    private void startCreate()
    {
        blockArray = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int objNum = Random.Range(0, blocksObj.Length);

                Vector2 objPos = new Vector2(x, y);
                GameObject obj = Instantiate(blocksObj[objNum], transform);
                obj.transform.position = objPos;
                obj.name = "(" + x + "," + y + ")";
                BlockObj block = obj.GetComponent<BlockObj>();
                block.SetXY(x, y);
                blockArray[x, y] = obj;
            }
        }


        CheckBoard();

    }

    public void SwapCheckBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] != null)
                {
                    blockArray[x, y].GetComponent<BlockObj>().checkMatch();
                }
            }
        }
    }
    //이게 실행되면 모든오브젝트의 match를 체크를해준다
    public void CheckBoard()
    {
        bool oneCheck = false;
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = width - 1; x >= 0; x--)
            {
                if (oneCheck)
                {
                    break;
                }
                if (blockArray[x, y] != null)
                {
                    lastBlockObj = blockArray[x, y];
                    lastBlockObj.GetComponent<BlockObj>().SetisLast(true);
                    oneCheck = true;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] != null)
                {
                    blockArray[x, y].GetComponent<BlockObj>().checkMatch();
                }
            }
        }
    }

    //모든오브젝트가 체크를하고 가로 / 세로 줄이 어떻게 매치됬는지 체크를해주는부분

    public void removeBoard()
    {
        //세로줄 체크
        int matchNum = 0;



        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] == null)
                {
                    matchNum = 0;
                    continue;
                }

                if (blockArray[x, y].GetComponent<BlockObj>().GetMatchCheck == true)
                {
                    matchNum++;
                }
                else
                {
                    matchNum = 0;
                }


                if (matchNum == 3)
                {

                    point += 3;
                    removeBlock.Add(blockArray[x, y]);
                    removeBlock.Add(blockArray[x, y - 1]);
                    removeBlock.Add(blockArray[x, y - 2]);
                }
                else if (matchNum == 4)
                {
                    point += 2;
                    removeBlock.Add(blockArray[x, y]);
                }
                else if (matchNum >= 5)
                {
                    point += 2;
                    Debug.Log("폭탄추가");
                    removeBlock.Add(blockArray[x, y]);
                }



            }
            matchNum = 0;
        }

        //가로줄 체크
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (blockArray[x, y] == null)
                {
                    matchNum = 0;
                    continue;
                }

                if (blockArray[x, y].GetComponent<BlockObj>().GetMatchCheck == true)
                {
                    matchNum++;
                }
                else
                {


                    matchNum = 0;
                }


                if (matchNum == 3)
                {
                    point += 3;
                    removeBlock.Add(blockArray[x, y]);
                    removeBlock.Add(blockArray[x - 1, y]);
                    removeBlock.Add(blockArray[x - 2, y]);
                }
                else if (matchNum == 4)
                {
                    point += 2;
                    removeBlock.Add(blockArray[x, y]);
                }
                else if (matchNum >= 5)
                {
                    point += 2;
                    Debug.Log("폭탄추가");
                    removeBlock.Add(blockArray[x, y]);
                }
            }
            matchNum = 0;
        }

        if (removeBlock.Count > 0)
        {
            destroyObj();

        }

        //매치가 된부분이 생겼으면 삭제를해준다
        else
        {
            Invoke("blockCreate", 0.5f);
        }
    }

    private void destroyObj()
    {
        for (int count = removeBlock.Count - 1; count >= 0; count--)
        {
            if (removeBlock[count] != null)
            {
                Destroy(removeBlock[count]);
                removeBlock.RemoveAt(count);
            }
            if (count == 0)
            {
                Invoke("pointAdd", 0.1f);
            }
        }
        removeBlock.Clear();
        Invoke("blockDown", 0.1f);
    }

    private void blockDown()
    {
        int nullCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    int yPos = y - nullCount;
                    GameObject downobj = blockArray[x, y];
                    downobj.GetComponent<BlockObj>().downMovingCheck(true);
                    downobj.GetComponent<BlockObj>().SetXY(x, yPos);// SetTargetPos(new Vector2(x, y - nullCount));
                    blockArray[x, yPos] = downobj;
                    blockArray[x, yPos].name = "(" + x + ", " + yPos + ")";
                    blockArray[x, y] = null;
                }
            }
            nullCount = 0;
        }
        downMoving = true;
    }


    private void blockCreate()
    {
        bool createCheck = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] == null)
                {
                    createCheck = true;
                }
            }
        }

        if (createCheck)
        {
            for (int x = 0; x < width; x++)
            {
                int createNum = 0;
                for (int y = 0; y < height; y++)
                {
                    if (blockArray[x, y] == null)
                    {
                        createNum++;
                    }

                    if (y == height - 1)
                    {
                        if (createNum > 0)
                        {
                            int posY = height;
                            for (int i = 0; i < createNum; i++)
                            {
                                int objNum = Random.Range(0, blocksObj.Length);
                                int objY = (height - 1) - i;

                                Vector2 objPos = new Vector2(x, height + posY);
                                GameObject obj = Instantiate(blocksObj[objNum], transform);
                                obj.transform.position = objPos;
                                obj.name = "(" + x + "," + objY + ")";

                                obj.GetComponent<BlockObj>().SetXY(x, objY);
                                blockArray[x, objY] = obj;
                                obj.GetComponent<BlockObj>().downMovingCheck(true);
                                posY--;

                            }
                        }
                    }


                }
            }
            objClickCheck = false;
            Invoke("blockDown", 0.1f);
            Invoke("newBlockCheckInvkoke", 0.2f);
        }
        else
        {
            boardOutCheck = true;
        }
    }

    private void newBlockCheckInvkoke()
    {
        newBlockCheck = true;
    }
    void Update()
    {
        allDownCheck();
        clickCheck();
        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    Debug.Log($"{x},{y}=={blockArray[x, y]} tag = {blockArray[x, y].tag}");

                }
            }
        }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      

    }

    private void allDownCheck()
    {
        if (downMoving)
        {

            bool allMoveCheck = false;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (blockArray[x, y] != null)
                    {
                        if (blockArray[x, y].transform.position != new Vector3(x, y, 0))
                        {
                            allMoveCheck = true;
                        }
                    }


                    if (x == width - 1 && y == height - 1)
                    {
                        if (allMoveCheck)
                        {
                            allMoveCheck = false;
                        }
                        else
                        {
                            CheckBoard();
                            downMoving = false;

                        }
                    }
                }
            }
        }

        if (newBlockCheck)
        {
            bool allMoveCheck = false;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (blockArray[x, y] == null)
                    {

                        Debug.Log($"{x},{y} 가 null = {blockArray[x, y]}");
                        continue;
                    }
                    if (blockArray[x, y].transform.position != new Vector3(x, y, 0))
                    {
                        allMoveCheck = true;
                    }
                    if (x == width - 1 && y == height - 1)
                    {
                        if (allMoveCheck)
                        {
                            allMoveCheck = false;
                        }
                        else
                        {
                            objClickCheck = true;
                            newBlockCheck = false;
                        }
                    }
                }
            }
        }


    }

    private void clickCheck()
    {
        bool nullCheck = false;
        bool posCheck = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] == null)
                {
                    btn.color = Color.black;
                    swappingToucchCheck = false;
                    nullCheck = true;
                }
                if (x == width - 1 && y == height - 1)
                {
                    if (nullCheck == false)
                    {
                        posCheck = true;
                    }
                    else
                    {
                        posCheck = false;
                    }
                }
            }
        }

        if (posCheck)
        {
            bool clickCheck = false;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (blockArray[x, y].transform.position != new Vector3(x, y, 0))
                    {
                        clickCheck = true;
                    }

                    if (x == width - 1 && y == height - 1)
                    {
                        if (clickCheck == false)
                        {
                            btn.color = Color.white;
                            swappingToucchCheck = true;
                        }
                    }
                }
            }
        }

    }
    //보드체크를 시작하기위한부분
    public void SetBoardOutCheck()
    {
        boardOutCheck = true;
    }



}
