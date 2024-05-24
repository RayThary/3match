using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    public enum eType
    {
        Red,
        Blue,
        Green,
        Pink,
        Black,
    }
    [SerializeField] private eType type;

 
    [SerializeField] private float speed;

    private Game gamestart;
    private Transform[,] parentArr;

    [SerializeField] private Vector3 targetVec;

    private bool moving = false;

    void Start()
    {
        gamestart = GetComponentInParent<Game>();
        parentArr = gamestart.GetArr();

        targetVec = transform.position;

    }

    void Update()
    {
        move();
        hrizontalCheck();
        verticalCheck();
    }

    private void move()
    {
        if (targetVec != transform.position)
        {
            moving = true;
            transform.position = Vector2.MoveTowards(transform.position, targetVec, speed * Time.deltaTime);
        }
        else
        {
            moving = false;
        }
    }

    private void hrizontalCheck()
    {
        if (moving)
        {
            return;
        }
        int x = (int)targetVec.x;
        int y = (int)targetVec.y;

        if (parentArr[x, y] == null)
        {
            return;
        }


        if (parentArr[x, y].position == targetVec)
        {

            if (targetVec.x >= 8 || parentArr[x + 1, y] == null)
            {
                return;
            }

        }

    }

    private void verticalCheck()
    {
        if (moving)
        {
            return;
        }
        int x = (int)targetVec.x;
        int y = (int)targetVec.y;
        if (parentArr[x, y] == null)
        {
            return;
        }
        if (parentArr[x, y].position == targetVec)
        {
            if (targetVec.y >= 8 || parentArr[x, y + 1] == null)
            {
                return;
            }

        }
    }

    public eType GetBlockType()
    {
        return type;
    }

    public void SetMovePos(Vector3 _value)
    {
        targetVec = _value;
    }

    public Vector3 GetTargetPos()
    {
        return targetVec;
    }



}
