using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects;

    private Transform[,] blockObj = new Transform[9, 9];

    [System.Serializable]
    public class allBlockObj
    {
        public Transform[,] blockTrs = new Transform[9, 9];

        public void SetBlcokTrs(Transform[,] _value)
        {
            blockTrs = _value;
        }
        public Transform[,] GetBlockTrs()
        {
            return blockTrs;
        }
    }
    private allBlockObj block;

    [SerializeField] private List<Transform> exDestroy = new List<Transform>();
    [SerializeField] private bool checkDestroy = false;

    private Camera camera;
    private Vector2 mousePos;

    [SerializeField] private bool swipeCheck = false;
    public bool dfjgnjkd = false;
    private void Awake()
    {
        block = new allBlockObj();
        int x = 0;
        int y = 0;

        for (int i = 0; i < 81; i++)
        {

            int objNum = Random.Range(0, 4);
            GameObject obj = Instantiate(spawnObjects[objNum], transform);
            obj.transform.position = new Vector3(x, y, 0);
            blockObj[x, y] = obj.transform;


            x++;

            if (x == 9)
            {
                y++;
                x = 0;
            }
        }

        block.SetBlcokTrs(blockObj);
        camera = GameObject.Find("Camera").GetComponent<Camera>();

    }

    private void Update()
    {
        blockSwipe();
        arrMove();//블록들의 이동
        arrAdd();//블록추가

        initArr();//배열좌표와 값을 똑같게 초기화시켜주는곳
        blockDestroyHorizontal();//가로 파괴할곳찾기

        blockDestroy();

        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Debug.Log($"blockObj[{i},{j}] = {blockObj[i, j]}");
                }
            }
        }

    }

    private void blockDestroy()
    {
        if (dfjgnjkd == false)
        {
            return;
        }
        if (exDestroy.Count > 0)
        {
            for (int i = exDestroy.Count - 1; i > 0; i--)
            {
                Destroy(exDestroy[i].gameObject);
                exDestroy.Remove(exDestroy[i]);

            }
        }
        else
        {
            dfjgnjkd = false;
        }

    }
    private void blockSwipe()
    {



    }



    private void initArr()
    {
        //row = 세로줄  column = 가로줄
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                if (blockObj[row, column] == null)
                {

                    if (column == 8)
                    {
                        break;
                    }
                    int columCount = 1;

                    for (int i = 1; i < 9; i++)
                    {
                        int y = column + i;

                        if (y >= 9)
                        {
                            break;
                        }

                        if (blockObj[row, y] == null)
                        {
                            columCount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (column + columCount > 8)
                    {
                        break;
                    }

                    blockObj[row, column] = blockObj[row, column + columCount];
                    blockObj[row, column + columCount] = null;
                    checkDestroy = true;
                }
            }
        }
    }

    private void arrMove()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                Vector3 targetPos = new Vector3(row, column, 0);
                if (blockObj[row, column] != null)
                {
                    if (blockObj[row, column].GetComponent<Move>().GetTargetPos() != targetPos)
                    {
                        blockObj[row, column].GetComponent<Move>().SetMovePos(targetPos);
                    }
                }
            }
        }
    }

    private void arrAdd()
    {
        for (int x = 0; x < 9; x++)
        {
            if (blockObj[x, 8] == null)
            {
                int count = 10;//소환될 y의 위치 
                for (int y = 0; y < 9; y++)
                {
                    if (blockObj[x, y] == null)
                    {
                        int objNum = Random.Range(0, 4);
                        GameObject obj = Instantiate(spawnObjects[objNum], transform);
                        obj.transform.position = new Vector3(x, count);
                        count++;
                        obj.GetComponent<Move>().SetMovePos(new Vector3(x, y, 0));
                        blockObj[x, y] = obj.transform;
                    }
                }
            }

        }
    }

    private void blockDestroyHorizontal()
    {
        if (swipeCheck)
        {
            for (int color = 0; color < 5; color++)
            {

                for (int destroyY = 0; destroyY < 9; destroyY++)
                {
                    int des = 0;
                    for (int destroyX = 0; destroyX < 9; destroyX++)
                    {


                        if (color == 0)
                        {
                            if (blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType() == Move.eType.Red)
                            {
                                des++;
                            }
                            else
                            {
                                des = 0;
                            }
                        }
                        else if (color == 1)
                        {
                            if (blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType() == Move.eType.Blue)
                            {
                                des++;
                            }
                            else
                            {
                                des = 0;
                            }
                        }
                        else if (color == 2)
                        {
                            if (blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType() == Move.eType.Green)
                            {
                                des++;
                            }
                            else
                            {
                                des = 0;
                            }
                        }
                        else if (color == 3)
                        {
                            if (blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType() == Move.eType.Pink)
                            {
                                des++;
                            }
                            else
                            {
                                des = 0;
                            }
                        }
                        else if (color == 4)
                        {
                            if (blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType() == Move.eType.Black)
                            {
                                des++;
                            }
                            else
                            {
                                des = 0;
                            }
                        }

                        if (des == 3)
                        {
                            exDestroy.Add(blockObj[destroyX, destroyY]);
                            exDestroy.Add(blockObj[destroyX - 1, destroyY]);
                            exDestroy.Add(blockObj[destroyX - 2, destroyY]);
                        }
                        else if (des == 4)
                        {
                            exDestroy.Add(blockObj[destroyX, destroyY]);
                            //점수증가? 
                        }
                        else if (des == 5)
                        {
                            exDestroy.Add(blockObj[destroyX, destroyY]);
                            //폭탄? or 무언가 넣기
                        }

                    }
                }
            }
            swipeCheck = false;
        }

    }

    public Transform[,] GetBlockArr()
    {
        return block.GetBlockTrs();
    }

}
