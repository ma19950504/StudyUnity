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
   public int prefabsId;
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
           case 0: 
               //speed = 200; 
               transform.Rotate(Vector3.back*speed*Time.deltaTime);
               break;
            default:
                timer += Time.deltaTime;
                if(timer>speed){
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
        if(id==0) Batch();
        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
   }
   public void Init(ItemData data)
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

   void Batch()
   {
       for(int i = 0; i < count; i++)
       {
//            Debug.Log("count:"+count+"i:"+i);
           // Debug.Log("transform.childCount:"+transform.childCount);
            
            //bullet在poolmanager中，bullet的parent为poolmanager
            //刚进入时未生成预制体，所以childcount=0
            Transform bullet;
            if(i<transform.childCount){  //刚进入时
                //Debug.Log("bullet:"+transform.GetChild(i).name);
                bullet = transform.GetChild(i);
            }else{
                //Debug.Log("bullet2:"+i);
                bullet= GameManager.instance.pool.Get(prefabsId).transform;
                bullet.parent = transform;
            }

            //Debug.Log("bullet:"+bullet.parent);
            //transform为挂载脚本的对象，即Weapon
            //Debug.Log("transform.position:"+transform.position);
            //bullet.parent = transform; //以Weapon为基准进行变动
            //bullet.position = transform.position;
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity; //确保子弹在生成时没有初始旋转
            //Debug.Log("bullet.position:"+bullet.position);
            Vector3 rotVec = Vector3.forward*360*i/count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f ,Space.World);
            bullet.GetComponent<Bullet>().Init(damage,-1,Vector3.zero); //-1为初始化无数次 
       }
   }
    void Fire()
    {
        if(!player.scanner.nearestTarget) return;
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = (targetPos - transform.position).normalized;
        Transform bullet= GameManager.instance.pool.Get(prefabsId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up,dir);
        bullet.GetComponent<Bullet>().Init(damage,count,dir); 

    }
   
}

 