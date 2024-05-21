using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects;

    [SerializeField] private Transform[,] blockObj = new Transform[9, 9];

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

    public bool x = false;

    private void Awake()
    {
        block = new allBlockObj();
        int x = 0;
        int y = 0;

        for (int i = 0; i < 81; i++)
        {
            if (i == 5 || i == 14 || i == 23 || i == 23 + 9 + 9 || i == 23 + 27)
            {
                x++;
                continue;
            }
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
    }

    private void Update()
    {
        initArr();//배열좌표와 값을 똑같게 초기화시켜주는곳
        arrMove();//블록들의 이동

        if (x)
        {
            arrAdd();//블록추가
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(blockObj[5, 8]);
            Debug.Log(blockObj[5, 7]);
            Debug.Log(blockObj[5, 6]);
            Debug.Log(blockObj[5, 5]);
            Debug.Log(blockObj[5, 4]);
            Debug.Log(blockObj[5, 3]);
            Debug.Log(blockObj[5, 2]);
            Debug.Log(blockObj[5, 1]);
            Debug.Log(blockObj[5, 0]);
        }
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
                    if (blockObj[row, column].position != targetPos)
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
                for (int y = 0; y < 8; y++)
                {
                    if (blockObj[x, y] == null)
                    {

                    }
                }
            }
        }
    }

    public Transform[,] GetBlockArr()
    {
        return block.GetBlockTrs();
    }

}
