using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;
    public void Lose()
    {
        Debug.Log("Lose");
        titles[0].SetActive(true);
        titles[2].SetActive(true);
    }
    public void Win()
    {
        Debug.Log("Win");
        titles[1].SetActive(true);
        titles[2].SetActive(true);
    }
}
