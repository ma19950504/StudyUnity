using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;  
    SpriteRenderer spriteRenderer;  
    Animator anim;
    public Scanner scanner;
    public Hands[] hands;
    public RuntimeAnimatorController[] animCon;

    public Vector2 inputVector;
    public float speed;

    public void Awake()
    {
        //初始化组件的引用
        rb = GetComponent<Rigidbody2D>();  //刚体
        spriteRenderer = GetComponent<SpriteRenderer>();  //精灵
        anim = GetComponent<Animator>();  //动画控制器
        scanner = GetComponent<Scanner>();  //检测范围内的怪
        hands = GetComponentsInChildren<Hands>(true);  //获取 当前游戏对象  及其  所有子对象  中指定类型
        //true：表示是否包括非激活状态的游戏对象。如果设置为 false，则只会获取激活状态的游戏对象中的组件。
        //这里会获取Hand Left 和 Hand Right
    }
    void OnEnable()
    {
        speed *= Character.Speed;  
        Debug.Log("PlayerId====="+GameManager.instance.playerId);
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }
    public void Update()
    {
        //系统默认方式
        //inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
    } 

    void OnMove(InputValue value)
    { 
        //inputSystem方式
        inputVector = value.Get<Vector2>();
    }
    public void FixedUpdate()
    {
        if(!GameManager.instance.isLive) return;
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * speed * Time.fixedDeltaTime);
    }
    void LateUpdate()
    {
        if(!GameManager.instance.isLive) return;
        anim.SetFloat("Speed", inputVector.magnitude);
        //Debug.Log(inputVector.magnitude);
        //控制移动时角色面朝
        if(inputVector.x != 0) //在移动
        {
            //向左走，x<0,反转 true
            spriteRenderer.flipX = inputVector.x < 0;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime *10;  //每帧变为每秒

        if(GameManager.instance.health<0){
            for(int i=2;i<transform.childCount;i++){ //GameManager.instance.player.transform.childCount
                //i =2 排除shadow和area
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetBool("Dead",true);
            GameManager.instance.GameOver();
        }
    }
}
