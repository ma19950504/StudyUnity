using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
        
    }
    public GameObject Get(int index)
    {
       
        GameObject select = null;
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        
        }
        if(!select) //如果select不存在，则生成以下
        {
           // Debug.Log("获取"+index+"进入了2");
            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
                
        }
        return select;
    }
}
