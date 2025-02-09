using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed  //静态只读属性
    {
        get {
            return GameManager.instance.playerId == 0 ? 1.1f : 1f;
        }
    }
    public static float WeaponSpeed  //静态只读属性
    {
        get {
            return GameManager.instance.playerId == 1 ? 1.1f : 1f;
        }
    }
    public static float WeaponRate  //静态只读属性
    {
        get {
            return GameManager.instance.playerId == 1 ? 0.9f : 1f;
        }
    }    public static float Damage  //静态只读属性
    {
        get {
            return GameManager.instance.playerId == 1 ? 1.2f : 1f;
        }
    }
}
