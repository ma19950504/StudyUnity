using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
   public ItemData.ItemType type;
   public Gear gear;
   public float rate;
   
   public void Init(ItemData data)
   {
        name = "Gear-"+data.itmeId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
   }

   public void Levelup(float rate)
   {
        this.rate = rate;
        ApplyGear();
   }
   void RateUp()
   {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id){
                case 0:
                    
                    weapon.speed = 150+(150*rate);
                    Debug.Log("rate up id=0"+weapon.name+"===="+weapon.speed);
                    break;
                default:
                    weapon.speed = 0.5f * (1f-rate);
                    break; 
            }
        } 
   }
   void SpeedUp()
   {
        float speed = 8f * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
   }
   void ApplyGear()
   {
        switch(type){
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;

        }
   }
}
