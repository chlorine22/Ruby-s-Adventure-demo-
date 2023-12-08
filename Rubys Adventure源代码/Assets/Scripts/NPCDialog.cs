using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    //对话
    public GameObject dialogBox;
    public float displayTime = 4.0f;
    private float timerDisplay;
    public Text dialogText;
    public AudioSource audioSource;
    public AudioClip completeTaskclip;
    private bool hasPlayed;

    //初始设置为无
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }

    //消失
    void Update()
    {
        if (timerDisplay>=0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay<0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    //显示
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        UIHealthBar.instance.hasTask = true;
        if (UIHealthBar.instance.fixedNum>=5)
        {
            dialogText.text = "哦，伟大的Ruby，谢谢你，你真的太棒了！";//任务完成后修改对话内容
            if (!hasPlayed)
            {
                audioSource.PlayOneShot(completeTaskclip);
                hasPlayed = true;
            }
            
        }
    }
}
