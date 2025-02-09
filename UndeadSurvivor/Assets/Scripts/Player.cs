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
        speed *= Character.Speed;  //不同角色不同的速度
        //Debug.Log("PlayerId====="+GameManager.instance.playerId);
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];  //根据角色id变更animator的控制器
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
        if(!GameManager.instance.isLive) return;  //当前没有player实例就返回
        inputVector = inputVector.normalized; //归一化表示方向
        rb.MovePosition(rb.position + inputVector * speed * Time.fixedDeltaTime);
        //平稳地移动并且与其他物体正确交互，推荐使用 MovePosition。需要实现更复杂的速度控制或瞬时速度变化，可以直接设置 velocity。
    }
    void LateUpdate()
    {
        if(!GameManager.instance.isLive) return; //当前没有player实例就返回
        anim.SetFloat("Speed", inputVector.magnitude);  //计算并返回向量的长度（模） >0就运动
        //Debug.Log(inputVector.magnitude);
        //控制移动时角色面朝
        if(inputVector.x != 0) //在移动
        {
            //向左走，x<0,反转 true
            spriteRenderer.flipX = inputVector.x < 0;
        }
    }
    void OnCollisionStay2D(Collision2D collision)  //持续接触
    {
        if(!GameManager.instance.isLive) return;
        GameManager.instance.health -= Time.deltaTime *10;  //Time.deltaTime 表示自上一帧以来的时间（以秒为单位）  每秒减少10点生命 不用Time.deltaTime会每帧-10

        if(GameManager.instance.health<0){
            for(int i=2;i<transform.childCount;i++){ //GameManager.instance.player.transform.childCount
                //i =2 排除shadow和area
                transform.GetChild(i).gameObject.SetActive(false);  //武器和左右手  
            }
            anim.SetBool("Dead",true);
            GameManager.instance.GameOver();
        }
    }
}
