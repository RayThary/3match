using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GameS : MonoBehaviour
{
    private Move[,] move;

    private Transform[,] blocks;

    private List<Transform> destroyObjs = new List<Transform>();
    private void Start()
    {
        //blocks = Game.Instance.GetArr();
        StartCoroutine(blockDestroy());
    }
 
    private void Update()
    {

    }

    private bool find(int _typeNum , int x, int y)
    {

        if (_typeNum == (int)blocks[x, y].GetComponent<Move>().GetBlockType())
        {
            return true;
        }

        return false;
    }

    IEnumerator blockDestroy()
    {
        yield return null;

        destroyOjbHrizontalAdd();


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
        for (int color = 0; color < 5; color++)
        {

            for (int destroyY = 0; destroyY < 9; destroyY++)
            {
                int des = 0;

                for (int destroyX = 0; destroyX < 9; destroyX++)
                {
                    if (blocks[destroyX, destroyY] == null)
                    {
                        continue;
                    }

                    if (find(color, destroyX, destroyY))
                    {
                        des++;
                    }
                    else
                    {
                        des = 0;
                    }
           

                    if (des == 3)
                    {
                        destroyObjs.Add(blocks[destroyX, destroyY]);
                        destroyObjs.Add(blocks[destroyX - 1, destroyY]);
                        destroyObjs.Add(blocks[destroyX - 2, destroyY]);

                    }
                    else if (des == 4)
                    {
                        destroyObjs.Add(blocks[destroyX, destroyY]);
                        //점수추가증가 넣어줄예정
                    }
                    else if (des >= 5)
                    {
                        destroyObjs.Add(blocks[destroyX, destroyY]);
                        //폭탄? or 무언가 넣기
                    }
                }
            }
        }
    }

    private void destroyOjbVerticalAdd()
    {
        for (int color = 0; color < 5; color++)
        {

            for (int destroyX = 0; destroyX < 9; destroyX++)
            {
                int des = 0;

                for (int destroyY = 0; destroyY < 9; destroyY++)
                {
                    if (blocks[destroyY, destroyX] == null)
                    {
                        continue;
                    }


                    if (find(color, destroyX, destroyY))
                    {
                        des++;
                    }
                    else
                    {
                        des = 0;
                    }

                    if (des == 3)
                    {
                        destroyObjs.Add(blocks[destroyX, destroyY]);
                        destroyObjs.Add(blocks[destroyX, destroyY - 1]);
                        destroyObjs.Add(blocks[destroyX, destroyY - 2]);
                    }
                    else if (des == 4)
                    {
                        destroyObjs.Add(blocks[destroyY, destroyY]);
                        //점수추가증가 넣어줄예정
                    }
                    else if (des >= 5)
                    {
                        destroyObjs.Add(blocks[destroyY, destroyY]);
                        //폭탄? or 무언가 넣기
                    }

                }
            }
        }
    }

}
