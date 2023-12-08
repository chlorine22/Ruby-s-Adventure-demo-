using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //速度
    public float speed=3;
    //刚体
    private Rigidbody2D rigidbody2d;
    //轴向控制
    public bool vertical;
    //方向控制
    private int direction = 1;
    //方向改变时间间隔
    public float changeTime = 3;
    //计时器
    private float timer;
    //动画
    private Animator animator;
    //故障
    private bool broken;
    //烟雾特效
    public ParticleSystem smokeEffect;
    //音效
    private AudioSource audioSource;
    public AudioClip fixedSound;
    public AudioClip[] hitSounds;
    //撞击
    public GameObject hitEffectParticle;

    //初始
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        PlayMoveAnimation();
        broken = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!broken)
        {
            return;
        }//已修好，不再移动

        timer -= Time.deltaTime;
        //变向
        if (timer<0)
        {
            direction = -direction;
            PlayMoveAnimation();
            timer = changeTime;
        }

        Vector2 position = rigidbody2d.position;
        //垂直轴向
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed*direction;
        }
        else//水平轴向
        {
            position.x = position.x + Time.deltaTime * speed*direction;
        }
    
        rigidbody2d.MovePosition(position); 
    }
    //碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController!=null)
        {
            rubyController.ChangeHealth(-1);
        }
    }
    //移动动画
    private void PlayMoveAnimation()
    {
        if (vertical)//垂直轴向动画
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else//水平轴向动画
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }
    //修复
    public void Fix()
    {
        Instantiate(hitEffectParticle,transform.position,Quaternion.identity);
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        int randomNum = Random.Range(0,2);
        audioSource.Stop();
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(hitSounds[randomNum]);
        Invoke("PlayFixedSound",1f);
        UIHealthBar.instance.fixedNum++;
    }
    //修复音效
    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedSound);
    }

}
