using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public SpawnData[] spawnDatas;
    float timer; //spawn计时器
    int level;
    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();  //当前对象及其子对象
    }
   void Update()
   {
        if(!GameManager.instance.isLive) return;
        timer += Time.deltaTime;
        level = Math.Min(Mathf.FloorToInt(GameManager.instance.gameTime/10f),spawnDatas.Length-1);
        //Debug.Log("level is "+level);
        if(timer > spawnDatas[level].spawnTime)
        {
            timer = 0;
            Spawn();
        }

    //    if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Debug.Log("进入Spawner");
    //         GameManager.instance.pool.Get(1);
    //     }
   }
   public void Spawn()
   {
        //GameObject enemy = GameManager.instance.pool.Get(Random.Range(0,2)); //获取两种敌人
        GameObject enemy = GameManager.instance.pool.Get(0); //获取两种敌人
        enemy.transform.position = spawnPoints[UnityEngine.Random.Range(1,spawnPoints.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnDatas[level]);
   }
}

[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public int health;
    public float speed;
    public float spawnTime;

}