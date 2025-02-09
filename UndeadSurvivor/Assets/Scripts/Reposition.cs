using System;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
   void OnTriggerExit2D(Collider2D collision)
   {
       // Debug.Log("Player exited area"+collision.name);
       if(!collision.CompareTag("Area")) return;
       Vector3 playerPosition = GameManager.instance.player.transform.position;
       //Tilemap的位置
       //Debug.Log("======="+transform.name+"------"+transform.tag);
       Vector3 myPosition = transform.position;  //当前的ground的位置
       float diffX = Math.Abs(playerPosition.x-myPosition.x);
       float diffY = Math.Abs(playerPosition.y-myPosition.y);
       //Debug.Log("diffX:"+diffX+"diffY:"+diffY);

       Vector3 playerDir = GameManager.instance.player.inputVector;
       float dirX = playerDir.x>0 ? 1 : -1;
       float dirY = playerDir.y>0 ? 1 : -1;
       // Debug.Log("dirX:"+dirX+"dirY:"+dirY);
       switch (transform.tag)
       {
           case "Ground":
                if(diffX>diffY)
                {
                   // Debug.Log("diffX>diffY");
                   transform.Translate(Vector3.right * dirX * 40);
                }
                else if(diffX<diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);

                }
                // else if(diffX==diffY)
                // {

                // }
               break;
            case "Enemy":
                if(coll.enabled)
                {
                    //Debug.Log("Enemy");
                    transform.Translate(playerDir*20+ new Vector3(UnityEngine.Random.Range(-3f,3f),UnityEngine.Random.Range(-3f,3f),0f));
                }
               break;
       }
   }
}
