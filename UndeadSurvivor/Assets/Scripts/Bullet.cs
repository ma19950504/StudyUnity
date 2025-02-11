using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    public float damage;
    public int per;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        if(per > -1){  //攻击次数
            rb.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!collider.CompareTag("Enemy") || per == -1) return;
        per--;
        if(per == -1){
            rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }

    }
}
