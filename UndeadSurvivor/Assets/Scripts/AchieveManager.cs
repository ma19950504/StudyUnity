using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    enum Achieve{
        UnlockPotato,
        UnlockBean
    }
    Achieve[] achieves;
    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        if(!PlayerPrefs.HasKey("MyData")){  //第一次游戏时才会初始化
            Init();
        }
        
    }
    void Init()
    {
        PlayerPrefs.SetInt("MyData",1);
        
        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(),0);
        }
        //PlayerPrefs.SetInt("UnlockPotato",0);
        //PlayerPrefs.SetInt("UnlockBean",0);
    }
    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
    void Start()
    {
        Debug.Log("初始化");
        UnlockCharacter();
    }
}
