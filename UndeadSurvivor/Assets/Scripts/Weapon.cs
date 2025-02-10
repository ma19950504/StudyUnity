using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.XR;

public class Weapon : MonoBehaviour
{
   public int id; //第n种武器 
   public int prefabsId; //预制件id  0是enemy  
   public float damage;
   public int count;  //数量
   public float speed; //旋转速度，射速
   public float timer; //weapon1的间隔

   Player player;

   public void Awake()
   {
        player = GameManager.instance.player;
   }
//    public void Start()
//    {
//         Init();
//    }
   
   void Update()
   {
    if(!GameManager.instance.isLive) return;
    switch(id) 
       {
           case 0: //0号武器
               //speed = 200; 
               transform.Rotate(Vector3.back*speed*Time.deltaTime); //Vector3.back (0, 0, -1) 
               //旋转方向：在 Unity 中，默认的坐标系是左手坐标系。对于绕 Z 轴的旋转：
                //正值（例如 Vector3.forward 或正 Z 轴）通常会导致逆时针旋转。
                //负值（例如 Vector3.back 或负 Z 轴）通常会导致顺时针旋转。
               break;
            default:
                timer += Time.deltaTime;
                if(timer>speed){  //speed为发射间隔，射速
                    timer = 0f;
                    Fire();
                }
                break;
       }

    //    if(Input.GetKeyDown(KeyCode.Space)){
    //         LevelUp(2,1);
    //    }
    
   } 
   public void LevelUp(float damage,int count)
   {
        this.damage = damage;
        this.count += count;
        if(id==0) Batch();  //第0种武器 铁锹
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
   }
   public void Init(ItemData data)  //ItemData
   {
        name = "Weapon-"+data.itmeId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itmeId;
        //Debug.Log("id="+id);
        damage = data.baseDamage;
        count = data.baseCount;

        for(int i = 0;i<GameManager.instance.pool.prefabs.Length;i++){
            if(data.projectiles == GameManager.instance.pool.prefabs[i]){
                prefabsId = i;
                break;
            }
        }

       switch(id) 
       {
           case 0: 
               speed = 150;
                Debug.Log(name+"===="+speed);
               transform.Rotate(Vector3.back*speed*Time.deltaTime);
              // Debug.Log("Init:"+transform.childCount);
               Batch();
              // Debug.Log("batch:"+transform.childCount);
               break;
            default:
                speed = 0.3f;
                break;
       }
       Hands hand = player.hands[(int)data.itemType];
       hand.spriter.sprite = data.hand;
       hand.gameObject.SetActive(true);
       Debug.Log("进入进入");
       player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
   } 

   void Batch() //只有铁锹用到了  初始状态count是3
   {
       for(int i = 0; i < count; i++)
       {
            
            //bullet在poolmanager中，bullet的parent为poolmanager
            //刚进入时未生成预制体，所以childcount=0
            Transform bullet;
            if(i<transform.childCount){  //初始状态Weapon下会生成3个bullet
                bullet = transform.GetChild(i);  //如果i<3 则复用0-2的bullet(初始生成的)
            }else{
                //Debug.Log("bullet2:"+i);
                bullet= GameManager.instance.pool.Get(prefabsId).transform; //从pool里获取新的bullet预制体
                bullet.parent = transform; //新预制体的父对象变成weapon (就是在weapon下面加铁锹)
            }
            //transform为挂载脚本的对象，即Weapon
            //bullet.parent = transform; //以Weapon为基准进行变动
            //bullet.position = transform.position;
            bullet.localPosition = Vector3.zero;  //localPosition为相对位置，即以父物体为基准进行变动
            bullet.localRotation = Quaternion.identity; //确保子弹在生成时没有初始旋转 Quaternion.identity


            Vector3 rotVec = Vector3.forward*360*i/count; //Vector3.forward (0,0,1)
            //i=0,  第一个铁锹0度， i=1  120度  i=2  240度  按照如上度数分配
            bullet.Rotate(rotVec); //开始旋转 子弹将被均匀地分布在以父对象为中心的圆周上。
            bullet.Translate(bullet.up * 1.5f ,Space.World); //从原点向上移动1.5f单位，指定移动是在世界坐标系中进行的
            bullet.GetComponent<Bullet>().Init(damage,-1,Vector3.zero); //-1为初始化无数次 
       }
   }
    void Fire()
    {
        if(!player.scanner.nearestTarget) return; //是否有目标
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;
        Transform bullet= GameManager.instance.pool.Get(prefabsId).transform;
        //0是enmey  1是铁锹 2是子弹  在init中确认id
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up,dir);
        bullet.GetComponent<Bullet>().Init(damage,count,dir); 

    }
   
}

 