using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Scanner : MonoBehaviour
{
   public float scanRange;
   public LayerMask targetLayer;
   public RaycastHit2D[] targets; //射线命中,检测范围内怪物成功时返回命中的object
   public Transform nearestTarget;

   void FixedUpdate()
   {
        // 当前对象的位置，射线范围，射线方向（全方向），最大距离（0为整个半径），检测图层
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
   }

   Transform GetNearest()
   {    
        Transform result = null;
        float diff = 100;  //检测距离
        foreach(RaycastHit2D target in targets){  //遍历更新最近的目标
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos,targetPos);
            if(curDiff<diff){   //
                diff = curDiff;
                result = target.transform;
            } 
        }
        //Debug.Log("调用了");
        return result;
   }
}
  