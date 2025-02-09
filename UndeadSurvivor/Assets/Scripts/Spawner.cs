using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;  //生成点
    public SpawnData[] spawnDatas;  //生成数据
    float timer; //spawn计时器
    int level;  
    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();  //当前对象及其子对象   在地图选取10个点用于充当生成点
    }
   void Update()
   {
        if(!GameManager.instance.isLive) return;
        timer += Time.deltaTime;
        level = Math.Min(Mathf.FloorToInt(GameManager.instance.gameTime/10f),spawnDatas.Length-1);
        //FloorToInt 将float类型的数向下取整成int  
        //在游戏时间/10  和 怪的种类数中取最小
        //Debug.Log("level is "+level);
        if(timer > spawnDatas[level].spawnTime)
        //如果当前时间>第0中怪的出生时间，就出生一个，并归零计时器
        //如果当前时间10s level=1, 只生成第2种怪
        {
            timer = 0;
            Spawn();
        }


   }
   public void Spawn()
   {
        //GameObject enemy = GameManager.instance.pool.Get(Random.Range(0,2)); //获取两种敌人
        GameObject enemy = GameManager.instance.pool.Get(0); //获取两种敌人 0为prefab的id=0   
        enemy.transform.position = spawnPoints[UnityEngine.Random.Range(1,spawnPoints.Length)].position;
        //UnityEngine.Random.Range(1,spawnPoints.Length) 为[1,10)随机整数 生成点
        enemy.GetComponent<Enemy>().Init(spawnDatas[level]);  //初始化怪
   }
}

[System.Serializable]
//Inspector 窗口来编辑这些类的字段，并且它们可以在场景和预制件中保存
public class SpawnData   //怪的信息
{
    public int spriteType;
    public int health;
    public float speed;
    public float spawnTime;  

}