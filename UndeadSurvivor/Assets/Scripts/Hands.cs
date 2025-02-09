using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    //Vector3 leftPos = new Vector3(-0.15f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    SpriteRenderer player;
    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
        //Debug.Log(GetComponentsInParent<SpriteRenderer>().);
    }
    void LateUpdate()
    {
        bool isReverse = player.flipX;
        Debug.Log("isReverse====="+isReverse);
        if(isLeft){
            Debug.Log("isLeft====="+isLeft);

            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 20;
        }else{
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder  = isReverse ? 20 : 4;
        }
    }
}
