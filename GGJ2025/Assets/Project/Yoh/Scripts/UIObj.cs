using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class UIObj : MonoBehaviour
{
    //
    public bool isShake;
    public bool isShakePuru;
    public bool isWait;

    //wait
    public float size;//waitベースサイズ
    public float spdWait;//waitSpeed

    //
    public float baseScale;
    public float shakeBig;
    public float shakeSmall;
    public float shakeSec;


    //
    private void OnEnable()
    {
        size = 1f;
        spdWait = 1f;

        baseScale = 1f;
        shakeBig = 2f;
        shakeSmall = 0.8f;
        shakeSec = 1.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //isWait = true;
    }

    // Update is called once per frame
    void Update()
    {
        //test
        if (isShake)
        {
            isShake = false;
            doShake();
        }
        if (isShakePuru)
        {
            isShakePuru = false;
            doShakePuru();
        }
        if (isWait)
        {
            isWait = false;
            doWait();
        }
    }


    //動かす
    public void doShake()
    {
        var seq = DOTween.Sequence();
        //seq.Append(this.transform.DOScale(scaleSmall, 0.05f));

        float bs = baseScale;
        var bigSize = new Vector3(bs * shakeBig, bs * shakeBig);
        var smallSize = new Vector3(bs * shakeSmall, bs * shakeSmall);
        var normalSize = new Vector3(bs, bs);
        seq.Append(this.transform.DOScale(bigSize, shakeSec));
        seq.Append(this.transform.DOScale(smallSize, shakeSec / 2));
        seq.Append(this.transform.DOScale(normalSize, shakeSec / 2));
    }


    //ぷるぷる系(カレンダーなど)
    public void doShakePuru()
    {
        float bs = baseScale;
        float shakeSec = 0.4f;
        float scaleSmall = bs * 0.95f;//拡縮の値（小）
        float scaleBig = bs * 1.03f;//拡縮の値（大）

        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOScale(scaleSmall, 0.05f));
        seq.Append(this.transform.DOScale(scaleBig, 0.05f));
        seq.Append(this.transform.DOScale(bs, 0.1f));
        //seq.Append(this.transform.)

        if (true) seq.Join(this.transform.DOShakePosition(shakeSec, 1f));
    }


    //tweenでdoWait
    public void doWait()
    {
        spdWait = 1f;
        if (size < 0.01f) size = 1f;

        float x = size * 1.0f;
        float y = size * 0.8f;//0.96f;
        float yShadow = size * 0.38f;//0.38f
        float yShadow2 = size * 0.38f * 0.96f;

        var spr = this.gameObject;
        spr.transform.DOScale(new Vector3(size, size, 1f), 0f);

        var tween = spr.transform.DOScale(new Vector3(x, y, 1f), spdWait).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
