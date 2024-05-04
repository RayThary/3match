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

    private bool horizontal = false;//if y+1의 type이 같으면 true
    private bool vertical = false;// if x+1의 type이 같으면 true

    [SerializeField] private float speed;

    private GameStart gamestart;
    private Transform[,] parentArr;


    [SerializeField] private GameObject target;
    private Vector3 targetVec;

    

    void Start()
    {
        gamestart = GetComponentInParent<GameStart>();
        parentArr = gamestart.GetBlockArr();

        targetVec = transform.position;

    }

    void Update()
    {
        move();
    }

    private void move()
    {
        if (targetVec != transform.position)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetVec, speed * Time.deltaTime);
        }
    }

    private void hrizontalCheck()
    {
        int x = (int)targetVec.x;
        int y = (int)targetVec.y;
        if (parentArr[x, y].position == transform.position)
        {

            if (parentArr[x + 1, y].GetComponent<Move>().GetBlockType() == type)
            {
                horizontal = true;
            }
            else
            {
                horizontal = false;
            }
        }
    }

    private void verticalCheck()
    {
        int x = (int)targetVec.x;
        int y = (int)targetVec.y;
        if (parentArr[x, y].position == transform.position)
        {

            if (parentArr[x, y + 1].GetComponent<Move>().GetBlockType() == type)
            {
                vertical = true;
            }
            else
            {
                vertical = false;
            }
        }
    }

    private eType GetBlockType()
    {
        return type;
    }

    public void SetMovePos(Vector3 _value)
    {
        targetVec = _value;
    }

    public bool GetHorizontal()
    {
        return horizontal;
    }

    public bool GetVertical()
    {
        return vertical;
    }

}
