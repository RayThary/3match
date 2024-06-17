using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameStart : MonoBehaviour
{
    [SerializeField] private int width;
    public int X { get { return width; } }
    [SerializeField] private int height;
    public int Y { get { return height; } }

    [SerializeField] private GameObject[] blocksObj;

    public GameObject[,] allObjs;

    [SerializeField] private List<GameObject> exDestroyObj = new List<GameObject>();

    private GameObject tempOneObj;
    public GameObject SetOneObj { set => tempOneObj = value; }
    public GameObject GetOneObj { get { return tempOneObj; } }

    private GameObject tempTwoObj;
    public GameObject SetTwoObj { set => tempTwoObj = value; }
    public GameObject GetTwoObj { get { return tempTwoObj; } }

    private bool destroyCheck = false;
    public bool DestroyCheck { set => destroyCheck = value; }

    private bool downCheck = false;
    private bool createObjCheck = false;

    //삭제할예정 부모에서 모든오브젝트의 움직임을 통제하기위한변수
    private bool fristTouchCheck = false;//첫번째 클릭 두번쨰 클릭 체크하기위한곳
    public bool setFristTouch { set => fristTouchCheck = value; }
    public bool getFristTouch { get { return fristTouchCheck; } }




    private bool movingCheck = false;




    //오브젝트 체인지
    #region
    //private void objChange()
    //{
    //    if (touchTwoPos.x == touchOnePos.x - 1 && touchOnePos.y == touchTwoPos.y)
    //    {

    //        tempOneObj = allObjs[oneX, oneY];
    //        tempTwoObj = allObjs[twoX, twoY];

    //        allObjs[oneX, oneY] = tempTwoObj;
    //        allObjs[twoX, twoY] = tempOneObj;

    //        tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
    //        tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);


    //        moveCheck = true;
    //    }
    //    else if (touchTwoPos.x == touchOnePos.x + 1 && touchOnePos.y == touchTwoPos.y)
    //    {
    //        tempOneObj = allObjs[oneX, oneY];
    //        tempTwoObj = allObjs[twoX, twoY];

    //        allObjs[oneX, oneY] = tempTwoObj;
    //        allObjs[twoX, twoY] = tempOneObj;

    //        tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
    //        tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);

    //        moveCheck = true;
    //    }
    //    else if (touchTwoPos.y == touchOnePos.y - 1 && touchOnePos.x == touchTwoPos.x)
    //    {
    //        tempOneObj = allObjs[oneX, oneY];
    //        tempTwoObj = allObjs[twoX, twoY];

    //        allObjs[oneX, oneY] = tempTwoObj;
    //        allObjs[twoX, twoY] = tempOneObj;

    //        tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
    //        tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);


    //        moveCheck = true;
    //    }
    //    else if (touchTwoPos.y == touchOnePos.y + 1 && touchOnePos.x == touchTwoPos.x)
    //    {
    //        tempOneObj = allObjs[oneX, oneY];
    //        tempTwoObj = allObjs[twoX, twoY];

    //        allObjs[oneX, oneY] = tempTwoObj;
    //        allObjs[twoX, twoY] = tempOneObj;

    //        tempOneObj.GetComponent<Obj>().SetXY(twoX, twoY);
    //        tempTwoObj.GetComponent<Obj>().SetXY(oneX, oneY);



    //        moveCheck = true;
    //    }
    //    else
    //    {
    //        Debug.Log("이동불가");

    //    }

    //    if (moveCheck)
    //    {

    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)
    //            {
    //                tempOneObj.GetComponent<Obj>().SetMoveCheck(true);
    //                tempTwoObj.GetComponent<Obj>().SetMoveCheck(true);

    //                allObjs[x, y].GetComponent<Obj>().SetMatchStart(true);

    //            }
    //        }


    //    }

    //    tempOneObj = null;
    //    tempTwoObj = null;


    //}
    #endregion
    private void Start()
    {
        allObjs = new GameObject[width, height];

        startCreate();
    }

    private void startCreate()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                int objNum = Random.Range(0, blocksObj.Length);

                Vector2 objPos = new Vector2(x, y);
                GameObject obj = Instantiate(blocksObj[objNum], transform);
                obj.transform.position = objPos;
                obj.name = "(" + x + "," + y + ")";
                allObjs[x, y] = obj;

            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                

                allObjs[x, y].GetComponent<Obj>().SetMatchStart(true);
                
            }
        }
    }
    private void Update()
    {
        objRemove();
        DownMove();
        createObj();

        if (Input.GetKeyDown(KeyCode.X))
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (allObjs[x, y] == null)
                    {
                        Debug.Log($"{x},{y}==null");
                    }
                }
            }
        }
    }

    private void objRemove()
    {
        if (movingCheck)
        {
            return;
        }
        if (destroyCheck)
        {
            Debug.Log("리무브중");
            //삭제할게없으면 넘어가기
            if (exDestroyObj.Count == 0)
            {
                destroyCheck = false;
                return;
            }
            //중복체크

            for (int i = exDestroyObj.Count - 1; i >= 0; i--)
            {
                if (exDestroyObj[i] == null)
                {
                    continue;
                }
                else
                {
                    Destroy(exDestroyObj[i]);

                }
            }
            exDestroyObj.Clear();

            downCheck = true;
            destroyCheck = false;
        }
    }

    private void DownMove()
    {
        if (downCheck)
        {
            movingCheck = true;
            StartCoroutine(down());
            downCheck = false;
        }
        //yield return new WaitForSeconds(0.4f);
    }

    IEnumerator down()
    {
        yield return new WaitForSeconds(0.5f);
        int nullCount = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allObjs[x, y] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    int objY = allObjs[x, y].GetComponent<Obj>().GetY;
                    allObjs[x, y].GetComponent<Obj>().SetMoveDown(x, objY - nullCount, false);

                    allObjs[x, objY - nullCount] = allObjs[x, y];
                    allObjs[x, y] = null;
                }
            }
            nullCount = 0;
        }
        createObjCheck = true;

    }

    private void createObj()
    {
        if (movingCheck)
        {
            Debug.Log("만드는중");
            return;
        }
        if (createObjCheck)
        {
            int startY = 10;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    if (allObjs[x, y] == null)
                    {
                        Debug.Log($"{x},{y}");
                        int objNum = Random.Range(0, blocksObj.Length);

                        Vector2 objPos = new Vector2(x, startY);
                        GameObject obj = Instantiate(blocksObj[objNum], transform);

                        obj.transform.position = objPos;
                        obj.GetComponent<Obj>().SetMoveDown(x, y, true);

                        obj.name = "(" + x + "," + y + ")";
                        allObjs[x, y] = obj;

                        startY++;
                    }
                }
                startY = 10;
            }

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    allObjs[x, y].GetComponent<Obj>().SetMatchStart(true);
                }
            }
            createObjCheck = false;
        }
    }

    public void AddDestrtoyObj(GameObject _gameobject)
    {
        exDestroyObj.Add(_gameobject);
    }


    public void SetMoving(bool _value)
    {
        movingCheck = _value;
    }

    public bool GetMoving()
    {
        return movingCheck;
    }

}
