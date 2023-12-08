using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        //检测对象是否存在RubyController脚本
        if (rubyController != null)
        {
            rubyController.ChangeHealth(-1);
        }
    }
}
