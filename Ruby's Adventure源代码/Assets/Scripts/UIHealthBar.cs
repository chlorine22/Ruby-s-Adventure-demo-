using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    //显示
    public Image mask;
    private float originalSize;

    public static UIHealthBar instance { get; private set; }

    public bool hasTask;
    public int fixedNum;

    private void Awake()
    {
        instance = this;
    }

 //初始
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    //血槽填充
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.
            Axis.Horizontal,originalSize*fillPercent);
    }
}
