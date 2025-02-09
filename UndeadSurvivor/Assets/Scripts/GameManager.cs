using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  //实现单例模式 可以通过 GameManager.instance 全局访问这个实例

    [Header("GameObject")]
    public PoolManager pool;  
    public Player player; 
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleanner;

    
    public float gameTime; 
    public float maxGameTime = 2 * 10f; 
    public bool isLive;  //游戏是否进行  暂停
    public float maxhealth = 100;
    public int playerId; //选的第几个角色
    public float health;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = {3,10,30,60,100,150,210,280,360,450,600};


    public void Awake()
    {
        instance = this;
    
    }
    public void GameStart(int id)
    {
        playerId = id;
        health = maxhealth;
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        Resume();
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
        isLive = true;
    }
    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }
    IEnumerator GameOverCoroutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    } 
    public void GameWin()
    {
        StartCoroutine(GameWinCoroutine());
    }
    IEnumerator GameWinCoroutine()
    {
        isLive = false;
        enemyCleanner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }     
    public void Update()
    {
        if(!isLive) return;
        gameTime += Time.deltaTime;
        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;  //用于维持Spawner中的level的值
            Debug.Log("游戏结束win");
            GameWin();
        }
    }
    public void GetExp()
    {
        if(!isLive) return;
        exp ++;
        // if(exp==nextExp[level]){
        //     level++;
        //     exp = 0;
        //     uiLevelUp.Show();
        // }
        if(exp==nextExp[Mathf.Min(level,nextExp.Length-1)]){  //防止越界
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;

    }
}
