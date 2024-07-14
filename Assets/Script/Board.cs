using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    public static Board Instance;
    [SerializeField] private int targetScore = 100;
    public int GetTargetScore { get { return targetScore; } }

    [SerializeField] private float timeLimit = 60;
    public float GetTimer { get { return timeLimit; } }

    private int point = 0;
    public int GetPoint { get { return point; } }

    [SerializeField] private int width;
    public int X { get { return width; } }
    [SerializeField] private int height;
    public int Y { get { return height; } }
    [SerializeField] private GameObject[] blocksObj;
    public GameObject[,] blockArray;
    [SerializeField] private GameObject bombBlockObjLR;
    [SerializeField] private GameObject bombBlockObjUD;

    private List<GameObject> removeBlock = new List<GameObject>();
    public List<GameObject> reblock { get { return removeBlock; } }

    private int beforeNum = -1;

    private bool downMoving = false;
    private bool newBlockCheck = false;//새 블록이 만들어졌는지 체크하고 내려주기위한불값
    private bool blockClick = false;//블록클릭가능한지여부

    private int beforeCreateBlockInt = -1;

    private bool swappingTouchCheck = false;//오브젝트가 안움직이고 바꾸기위한준비가되었는지 체크해주는부분

    private GameObject lastBlockObj;

    private bool firstClick = true;
    private GameObject firstBlock;
    private GameObject secondBlock;
    public GameObject GetSecondBlock { get { return secondBlock; } }

    private bool moveSwap = false;

    private int firstX = 0;
    private int firstY = 0;
    private int secondX = 0;
    private int secondY = 0;

    private Fade fade;
    private bool isFade = false;
    private float fadeTimer = 0;

    private bool isfail = false;

    private BoxCollider2D box2d;
    private Option option;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        option = GetComponent<Option>();
        box2d = GetComponent<BoxCollider2D>();
        box2d.size = new Vector2(width, height);
        box2d.offset = new Vector2((float)width/2, (float)height / 2);
        fade = FindObjectOfType<Fade>();
        fade.FadeStart();
        Time.timeScale = 1;
    }

    private void OnMouseDown()
    {
        if (fade.GetFade() == true || Time.timeScale == 0 || blockClick == true)
        {
            return;
        }

        if (swappingTouchCheck)
        {
            if (firstClick == true)
            {
                //첫번째클릭인지 체크

                boardInClickCheck();
            }
            else
            {
                blockClick = true;
                boardInClickCheck();
                blockSwap();
            }
        }
    }

    private void boardInClickCheck()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Floor(mousePos.x);
        mousePos.y = Mathf.Floor(mousePos.y);

        if (mousePos.x >= 0 && mousePos.y >= 0 && mousePos.x < width && mousePos.y < height)
        {
            if (firstClick)
            {
                firstBlock = blockArray[(int)mousePos.x, (int)mousePos.y];
                if (firstBlock.CompareTag("Bomb"))
                {
                    firstX = (int)mousePos.x;
                    firstY = (int)mousePos.y;
                    firstBlock.tag = "BombDestroy";
                    bombDestroy();
                }
                else
                {
                    firstX = (int)mousePos.x;
                    firstY = (int)mousePos.y;
                    firstBlock.GetComponent<BlockObj>().SetFirstBlockClick();
                    firstClick = false;
                }


            }
            else
            {
                secondBlock = blockArray[(int)mousePos.x, (int)mousePos.y];
                secondX = (int)mousePos.x;
                secondY = (int)mousePos.y;
                firstBlock.GetComponent<BlockObj>().SetSecondBlockClick();

                firstClick = true;
            }
        }
        else
        {
            if (firstClick == false)
            {
                secondBlock = null;
                firstBlock.GetComponent<BlockObj>().SetSecondBlockClick();
                firstClick = true;
            }
        }





    }

    private void blockSwap()
    {

        if (secondBlock == null)
        {
            Debug.Log("외부 클릭");
        }
        else if (secondBlock.transform.position.x == firstBlock.transform.position.x - 1 && firstBlock.transform.position.y == secondBlock.transform.position.y)
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
            blockClick = false;
            Debug.Log("이동불가");
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

                if (y > 1)
                {
                    int y1 = blockArray[x, y - 1].GetComponent<BlockObj>().GetBlockNum;
                    int y2 = blockArray[x, y - 2].GetComponent<BlockObj>().GetBlockNum;

                    if (objNum == y1 && y1 == y2)
                    {
                        while (objNum == y1)
                        {
                            objNum = Random.Range(0, blocksObj.Length);
                        }
                    }
                }

                beforeNum = objNum;

                if (x > 1)
                {
                    int x1 = blockArray[x - 1, y].GetComponent<BlockObj>().GetBlockNum;
                    int x2 = blockArray[x - 2, y].GetComponent<BlockObj>().GetBlockNum;
                    if (x1 == x2 && x1 == objNum)
                    {
                        while (objNum == x1 || objNum == beforeNum)
                        {
                            objNum = Random.Range(0, blocksObj.Length);
                        }
                    }
                }

                Vector2 objPos = new Vector2(x, y);
                GameObject obj = Instantiate(blocksObj[objNum], transform);

                if (x > 1)
                {
                    if (obj.tag == blockArray[x - 1, y].tag && blockArray[x - 1, y].tag == blockArray[x - 2, y].tag)
                    {
                        while (objNum == beforeNum)
                        {
                            if (y > 1)
                            {

                            }
                            else
                            {
                                objNum = Random.Range(0, blocksObj.Length);
                            }
                        }
                    }
                }

                obj.transform.position = objPos;
                obj.name = "(" + x + "," + y + ")";
                BlockObj block = obj.GetComponent<BlockObj>();
                block.SetBlockNum(objNum);
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
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (blockArray[x, y] != null)
                {
                    blockArray[x, y].GetComponent<BlockObj>().checkMatch();
                }
            }
        }
        removeBoard();
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

                if (matchNum == 4)
                {
                    if (blockArray[x, y].tag == blockArray[x, y - 1].tag && blockArray[x, y - 1].tag == blockArray[x, y - 2].tag && blockArray[x, y - 2].tag == blockArray[x, y - 3].tag)
                    {
                        blockArray[x, y].GetComponent<BlockObj>().SetBombUDObj();
                    }

                    blockArray[x, y].GetComponent<BlockObj>().SetBlockPoint(2);
                }
                else if (matchNum > 4)
                {
                    blockArray[x, y].GetComponent<BlockObj>().SetBlockPoint(2);
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


                if (matchNum == 4)
                {
                    if (blockArray[x, y].tag == blockArray[x - 1, y].tag && blockArray[x - 1, y].tag == blockArray[x - 2, y].tag && blockArray[x - 2, y].tag == blockArray[x - 3, y].tag)
                    {
                        blockArray[x, y].GetComponent<BlockObj>().SetBombLRObj();
                    }
                    blockArray[x, y].GetComponent<BlockObj>().SetBlockPoint(2);
                }
                else if (matchNum > 4)
                {
                    blockArray[x, y].GetComponent<BlockObj>().SetBlockPoint(2);
                }

            }
            matchNum = 0;
        }


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] != null)
                {
                    if (blockArray[x, y].GetComponent<BlockObj>().GetMatchCheck == true)
                    {
                        removeBlock.Add(blockArray[x, y]);
                        blockArray[x, y] = null;
                    }
                }
            }
        }


        //매치가 된부분이 생겼으면 삭제를해준다
        if (removeBlock.Count > 0)
        {
            Invoke("destroyObj", 0.2f);//삭제 딜레이
        }
        else
        {
            Invoke("blockCreate", 0.2f);
        }
    }

    private void destroyObj()
    {

        for (int count = removeBlock.Count - 1; count >= 0; count--)
        {
            if (removeBlock[count] != null)
            {
                if (removeBlock[count].GetComponent<BlockObj>().GetBombLRObj() == true)
                {
                    int bombX = (int)removeBlock[count].transform.position.x;
                    int bombY = (int)removeBlock[count].transform.position.y;

                    GameObject bomb = Instantiate(bombBlockObjLR, transform);

                    bomb.transform.position = new Vector3(bombX, bombY, 0);
                    blockArray[bombX, bombY] = bomb;
                }
                else if (removeBlock[count].GetComponent<BlockObj>().GetBombUDObj() == true)
                {
                    int bombX = (int)removeBlock[count].transform.position.x;
                    int bombY = (int)removeBlock[count].transform.position.y;

                    GameObject bomb = Instantiate(bombBlockObjUD, transform);

                    bomb.transform.position = new Vector3(bombX, bombY, 0);
                    blockArray[bombX, bombY] = bomb;
                }

                Destroy(removeBlock[count]);
            }

            if (count == removeBlock.Count - 1)
            {
                if (isFade == true)
                {
                    SoundManager.instance.SFXCreate(SoundManager.Clips.Block, 1, 0);
                }
            }
        }
        removeBlock.Clear();
        blockDown();
    }

    private void bombDestroy()
    {
        if (firstBlock.GetComponent<BlockObj>().type == BlockType.BombLR)
        {
            for (int i = 0; i < width; i++)
            {
                if (blockArray[i, firstY].tag != "Bomb")
                {
                    removeBlock.Add(blockArray[i, firstY]);
                    blockArray[i, firstY] = null;
                }
            }
        }
        else if (firstBlock.GetComponent<BlockObj>().type == BlockType.BombUD)
        {
            for (int i = 0; i < height; i++)
            {
                if (blockArray[firstX, i].tag != "Bomb")
                {
                    removeBlock.Add(blockArray[firstX, i]);
                    blockArray[firstX, i] = null;
                }
            }
        }

        destroyObj();

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
                    downobj.GetComponent<BlockObj>().SetXY(x, yPos);
                    blockArray[x, yPos] = downobj;
                    blockArray[x, yPos].name = "(" + x + ", " + yPos + ")";
                    blockArray[x, y] = null;
                }



                if (x == width - 1 && y == height - 1)
                {
                    downMoving = true;
                }
            }
            nullCount = 0;
        }


    }


    private void blockCreate()
    {
        bool endCheck = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (blockArray[x, y] == null)
                {
                    endCheck = false;
                    break;
                }
                else
                {
                    endCheck = true;
                }

                if (x == width - 1 && y == height - 1)
                {
                    blockClick = false;
                    return;
                }
            }
            if (endCheck == false)
            {
                break;
            }
        }

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
                            while (beforeCreateBlockInt == objNum)
                            {
                                objNum = Random.Range(0, blocksObj.Length);
                            }

                            beforeCreateBlockInt = objNum;
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
        Invoke("newBlockCheckInvkoke", 0.2f);


    }

    private void newBlockCheckInvkoke()
    {
        newBlockCheck = true;
    }
    void Update()
    {
        fadeCheck();
        allDownCheck();
        clickCheck();
        clearCheck();
        failCheck();

        if (Input.GetKeyDown(KeyCode.X))
        {

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (blockArray[x, y] == null)
                    {
                        Debug.Log($"{x},{y}==null");
                    }
                    else
                    {
                        Debug.Log($"{x},{y}=={blockArray[x, y]} tag = {blockArray[x, y].tag}");

                    }

                }
            }
        }


    }

    private void fadeCheck()
    {

        if (isFade == false && swappingTouchCheck == true)
        {
           
            fadeTimer += Time.deltaTime;
            if (fadeTimer >= 1)
            {
                isFade = true;
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
                            newBlockCheck = false;
                            CheckBoard();
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
                    swappingTouchCheck = false;
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
                            swappingTouchCheck = true;
                        }
                    }
                }
            }
        }

    }

    private void clearCheck()
    {
        if (point >= targetScore)
        {
            Time.timeScale = 0;
            option.IsClear();
        }
    }

    private void failCheck()
    {
        if (isfail)
        {
            Time.timeScale = 0;
            option.IsFail();
        }
    }

    public void SetBlockClick(bool _value)
    {
        blockClick = _value;
    }



    public bool GetFade()
    {
        return isFade;
    }

    public void SetPoint(int _point)
    {
        point = _point;
    }

    public void SetIsFail()
    {
        isfail = true;
    }

    public void AddPoint(int _point)
    {
        point += _point;
    }
}
