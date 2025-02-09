using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxhealth;
    public RuntimeAnimatorController[] animCon;
    public Animator anim;
    public Rigidbody2D target; //目标=，即player
    public Collider2D coll; 
    public bool isAlive;
    Rigidbody2D rb; //enemy
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if(!GameManager.instance.isLive) return;
        if(!isAlive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        Vector2 targetDir = (target.position - rb.position).normalized;  //归一化可以表示方向，不考虑距离
        //Debug.Log("targetDir:"+target.position+"////"+rb.position+"///"+targetDir);
        Vector2 nextDir  =  targetDir*speed*Time.deltaTime;  //下一帧要到的位置距离，position
        rb.MovePosition(rb.position + nextDir);
        rb.velocity = Vector2.zero;
    }
    void LateUpdate()
    {
        if(!GameManager.instance.isLive) return;
        spriteRenderer.flipX = target.position.x < rb.position.x;

    }
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        spriteRenderer.sortingOrder = 2 ;
        coll.enabled = true;
        rb.simulated = true;
        anim.SetBool("Dead",false);
        health = maxhealth;
    }
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxhealth = data.health;
        health = data.health;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet")||!isAlive) return;
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); 
        if(health>0){
            anim.SetTrigger("Hit");
        }else{
            isAlive = false;
            spriteRenderer.sortingOrder = 1;
            coll.enabled = false;
            rb.simulated = false;
            Debug.Log("进入dead");
            anim.SetBool("Dead",true);
            Debug.Log("进入dead动画"+anim.GetBool("Dead"));
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            //Dead();
        } 
    }
    IEnumerator KnockBack() //击退 
    {
        yield return wait;
        Vector3 playPos = GameManager.instance.player.transform.position;
        Vector3 dir = (transform.position-playPos);
        //Debug.Log("击退"+dir);
        rb.AddForce(dir.normalized * 20, ForceMode2D.Impulse);
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
