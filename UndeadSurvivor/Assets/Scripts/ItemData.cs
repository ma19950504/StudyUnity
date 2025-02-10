using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item")]
//直接可以右键创建Item类型的object
//Data文件夹中存放着

public class ItemData : ScriptableObject
{
    public enum ItemType {Melee,Range,Glove,Shoe,Heal};

    [Header("Main Info")]
    public ItemType itemType; 
    public int itmeId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;
    [Header("Level Info")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;
    [Header("Weapon")]
    public GameObject projectiles;
    public Sprite hand;
}
