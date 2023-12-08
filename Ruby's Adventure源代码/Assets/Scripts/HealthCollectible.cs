using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip audioClip;
    public GameObject effectParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        //检测的游戏物体对象身上有否有RubyController脚本
        if (rubyController!=null)
        {
            //有RubyController脚本
            //Ruby是否满血
            if (rubyController.Health<rubyController.maxHealth)
            {
                //Ruby是不满血状态
                rubyController.ChangeHealth(1);
                rubyController.PlaySound(audioClip);
                Instantiate(effectParticle,transform.position,Quaternion.identity);
                Destroy(gameObject);
            }
         
        }
    }
}
