using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private List<GameObject> GameObjects;

    private Transform[,] blockObj = new Transform[9, 9];

    private List<Transform> destroyObjs = new List<Transform>();
    int columCount;

    private bool destroyCheck = false;
    private bool objPosCheck = false;

    void Start()
    {
        objectCreate();
        StartCoroutine(blockDestroy());
    }
    private void objectCreate()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                int objNum = Random.Range(0, GameObjects.Count);

                GameObject obj = Instantiate(GameObjects[objNum], transform);

                obj.transform.position = new Vector3(x, y, 0);
                blockObj[x, y] = obj.transform;
            }
        }
    }

    void Update()
    {


        //initArr();//2차배열이 좌표와 포지션이맞게 정해줌
        //arrMove();

        //objDestroy();
    }

    private void initArr()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (blockObj[x, y] == null)
                {

                    columCount = 1;

                    if (blockObj[x, 8] == null || y == 8)
                    {
                        break;
                    }


                    for (int i = 1; i < 9; i++)
                    {
                        int ypos = y + i;

                        if (ypos > 8)
                        {
                            break;
                        }

                        if (blockObj[x, ypos] == null)
                        {
                            columCount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    blockObj[x, y] = blockObj[x, y + columCount];
                    blockObj[x + columCount, y] = null;
                    destroyCheck = true;
                }
            }
        }

        int posCheck = 0;

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (blockObj[x, y].position != new Vector3(x, y, 0))
                {
                    posCheck++;
                }
                else if (blockObj[x, y] == null)
                {
                    continue;
                }
            }
        }

        if (posCheck > 0)
        {
            objPosCheck = false;
        }
        else
        {
            objPosCheck = true;
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

    private void objDestroy()
    {
        if (destroyCheck && objPosCheck)
        {
            StartCoroutine(blockDestroy());
        }
    }

    IEnumerator blockDestroy()
    {
        yield return null;
        destroyOjbHrizontalAdd();
        destroyOjbVerticalAdd();
        //점수 스크립트

        if (destroyObjs.Count > 0)
        {
            for (int i = destroyObjs.Count - 1; i >= 0; i--)
            {
                Destroy(destroyObjs[i].gameObject);
            }
        }

    }

    private void destroyOjbHrizontalAdd()
    {
        for (int color = 0; color < GameObjects.Count; color++)
        {

            for (int destroyY = 0; destroyY < 9; destroyY++)
            {
                int des = 0;

                for (int destroyX = 0; destroyX < 9; destroyX++)
                {
                    if (blockObj[destroyX, destroyY] == null)
                    {
                        continue;
                    }


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
                        destroyObjs.Add(blockObj[destroyX, destroyY]);
                        destroyObjs.Add(blockObj[destroyX - 1, destroyY]);
                        destroyObjs.Add(blockObj[destroyX - 2, destroyY]);

                    }
                    else if (des == 4)
                    {
                        destroyObjs.Add(blockObj[destroyX, destroyY]);
                        //점수추가증가 넣어줄예정
                    }
                    else if (des >= 5)
                    {
                        destroyObjs.Add(blockObj[destroyX, destroyY]);
                        //폭탄? or 무언가 넣기
                    }

                }
            }
        }

    }

    private void destroyOjbVerticalAdd()
    {
        for (int color = 0; color < GameObjects.Count; color++)
        {

            for (int destroyX = 0; destroyX < 9; destroyX++)
            {
                int des = 0;

                for (int destroyY = 0; destroyY < 9; destroyY++)
                {
                    if (blockObj[destroyY, destroyX] == null)
                    {
                        continue;
                    }

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
                        destroyObjs.Add(blockObj[destroyX, destroyY]);
                        destroyObjs.Add(blockObj[destroyX, destroyY - 1]);
                        destroyObjs.Add(blockObj[destroyX, destroyY - 2]);
                    }
                    else if (des == 4)
                    {
                        destroyObjs.Add(blockObj[destroyY, destroyY]);
                        //점수추가증가 넣어줄예정
                    }
                    else if (des >= 5)
                    {
                        destroyObjs.Add(blockObj[destroyY, destroyY]);
                        //폭탄? or 무언가 넣기
                    }

                }
            }
        }
    }

    public Transform[,] GetArr()
    {
        return blockObj;
    }
}

