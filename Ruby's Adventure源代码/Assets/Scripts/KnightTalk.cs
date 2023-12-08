using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightTalk : MonoBehaviour
{
    //对话
    public GameObject dialogBox;
    public float displayTime = 6.0f;
    private float timerDisplay;
    public Text dialogText;

    //初始设置为无
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }

    //消失
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
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
    }
}
