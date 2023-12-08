using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //齿轮子弹的刚体
    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    //发射朝向
    public void Launch(Vector2 direction,float force)
    {
        rigidbody2d.AddForce(direction*force);
    }

    //消失
    private void Update()
    {
        if (transform.position.magnitude>100)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemyController = collision.gameObject.
            GetComponent<EnemyController>();
        if (enemyController!=null)
        {
            enemyController.Fix();
        }
        Destroy(gameObject);
    }
}
