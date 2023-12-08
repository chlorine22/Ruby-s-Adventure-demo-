using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    //刚体
    private Rigidbody2D rigidbody2d;

    //速度
    public float speed;

    //生命值
    public int maxHealth = 5;//最大生命值
    private int currentHealth;//Ruby的当前生命值
    public int Health { get { return currentHealth; } }

    //无敌时间
    public float timeInvincible = 3.0f;//无敌时间常量
    public bool isInvincible;//无敌状态
    public float invincibleTimer;//计时器

    //动画
    private Vector2 lookDirection = new Vector2(1,0);
    private Animator animator;

    //子弹（齿轮）
    public GameObject projectilePrefab;

    //音效
    public AudioSource audioSource;
    public AudioSource walkAudioSource;
    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    //重生
    private Vector3 respawnPosition;

    //初始
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;
    }

    //主要脚本
    void FixedUpdate()
    {
        //监听玩家输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //移动相关
        Vector2 move = new Vector2(horizontal,vertical);
        if (!Mathf.Approximately(move.x,0)||!Mathf.Approximately(move.y,0))
        {
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }                      
        }
        else
        {
            walkAudioSource.Stop();
        }

        //动画的控制
        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed",move.magnitude);

        //移动
        Vector2 position = transform.position;
        position = position + speed * move * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        //无敌时间计算
        if (isInvincible)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer<=0)
            {
                isInvincible = false;
            }
        }
        //攻击(扔齿轮修理机器人)
        if (Input.GetKeyDown(KeyCode.J))
        {
            Launch();
        }

        //检测是否与NPC对话
        if (Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,lookDirection,1.5f,LayerMask.GetMask("NPC"));
            if (hit.collider!=null)
            {
                KnightTalk knightTalk = hit.collider.GetComponent<KnightTalk>();
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if (npcDialog!=null)
                {
                    npcDialog.DisplayDialog();
                }
                if(knightTalk!=null)
                {
                    knightTalk.DisplayDialog();
                }
            }
        }

    }

    //生命及受伤无敌帧
    public void ChangeHealth(int amount)
    {
        if (amount<0)
        {
            if (isInvincible)
            {
                return;
            }

            //受到伤害
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }
        currentHealth = Mathf.Clamp(currentHealth+amount,0,maxHealth);

        if (currentHealth<=0)
        {
            Respawn();
        }
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
    //捡道具无敌
    public void Invincible()
    {
        isInvincible = true;
        invincibleTimer = timeInvincible;
    }
    //发射子弹（齿轮）
    private void Launch()
    {
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }//有任务才能发射
        GameObject projectileObject = Instantiate(projectilePrefab,rigidbody2d.position+Vector2.up*0.5f,Quaternion.identity);
        Projectile projectile= projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection,300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    //按照条件播放一次的音效
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    //重生在地图初始点
    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }
}
