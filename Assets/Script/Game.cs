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
    int yPos;

    private bool destroyCheck = false;//파괴할지 체크하는곳

    private bool objPosCheck = false;//파괴를위한 오브젝트의 위치체크
    [SerializeField] private bool objPosCreateCheck = false; //만들기위한 오브젝트의 위치체크

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


        initArr();//2차배열이 좌표와 포지션이맞게 정해줌
        objMove();
        objPostionCheck();
        objCreate();
        objDestroy();

    }

    private void initArr()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (blockObj[x, y] == null)
                {

                    if (y == 8)
                    {
                        break;
                    }
                    int columCount = 1;

                    for (int i = 1; i < 9; i++)
                    {
                        int yPos = y + i;

                        if (yPos >= 9)
                        {
                            break;
                        }

                        if (blockObj[x, yPos] == null)
                        {
                            columCount++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (y + columCount > 8)
                    {
                        break;
                    }

                    blockObj[x, y] = blockObj[x, y + columCount];
                    blockObj[x, y + columCount] = null;
                    destroyCheck = true;
                }
            }
            #region
            //for (int y = 0; y < 9; y++)
            //{
            //    for (int x = 0; x < 9; x++)
            //    {
            //        if (blockObj[x, y] == null)
            //        {
            //            if (y == 8)
            //            {
            //                break;
            //            }

            //            columCount = 1;
            //            for (int i = 1; i < 9; i++)
            //            {
            //                int yPos = y + i;

            //                if (yPos >= 9)
            //                {
            //                    break;
            //                }

            //                if (blockObj[x, yPos] == null)
            //                {
            //                    columCount++;
            //                }
            //                else
            //                {
            //                    break;
            //                }

            //            }

            //            if (y + columCount > 8)
            //            {
            //                break;
            //            }

            //            blockObj[x, y] = blockObj[x, y + columCount];
            //            blockObj[x, y + columCount] = null;
            //            destroyCheck = true;

            //        }
            //    }
            //}
            #endregion
        }
    }

    private void objMove()
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

    private void objCreate()
    {
        if (objPosCreateCheck)
        {
            for (int x = 0; x < 9; x++)
            {
                if (blockObj[x, 8] == null)
                {
                    int count = 11;//소환될 y의 위치 
                    for (int y = 0; y < 9; y++)
                    {
                        if (blockObj[x, y] == null)
                        {
                            int objNum = Random.Range(0, 4);
                            GameObject obj = Instantiate(GameObjects[objNum], transform);
                            obj.transform.position = new Vector3(x, count);
                            count++;
                            obj.GetComponent<Move>().SetMovePos(new Vector3(x, y, 0));
                            blockObj[x, y] = obj.transform;
                        }
                    }
                }
            }
            objPosCreateCheck = false;
        }
    }

    private void objPostionCheck()
    {
        int posCheck = 0;

        int nullObjNum = 0;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (blockObj[x, y] != null && blockObj[x, y].position != new Vector3(x, y, 0))
                {
                    posCheck++;
                }

                if (blockObj[x, y] == null)
                {
                    nullObjNum++;
                }

                if (blockObj[8, 8])
                {
                    if (posCheck == 0)
                    {
                        if (nullObjNum == 0)
                        {

                            objPosCheck = true;
                        }
                        objPosCreateCheck = true;
                    }
                }
                else
                {
                    objPosCheck = false;
                }


            }
        }




    }

    private void objDestroy()
    {
        if (destroyCheck && objPosCheck)
        {
            StartCoroutine(blockDestroy());
            objPosCheck = false;
            destroyCheck = false;
        }
    }

    IEnumerator blockDestroy()
    {
        destroyOjbHrizontalAdd();
        destroyOjbVerticalAdd();
        //점수 스크립트 만들예정
        yield return new WaitForSeconds(0.5f);

        if (destroyObjs.Count > 0)
        {
            for (int i = destroyObjs.Count - 1; i >= 0; i--)
            {
                if (destroyObjs[i] == null)
                {
                    continue;
                }
                else
                {
                    Destroy(destroyObjs[i].gameObject);
                }

                if (i == 0)
                {
                    destroyObjs.Clear();
                }
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

                    if (color == (int)blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType())
                    {
                        des++;
                    }
                    else
                    {
                        des = 0;
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

                    if (color == (int)blockObj[destroyX, destroyY].GetComponent<Move>().GetBlockType())
                    {
                        des++;
                    }
                    else
                    {
                        des = 0;
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

