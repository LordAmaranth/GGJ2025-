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
    public bool isJump;

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
        if (isJump)
        {
            isJump = false;
            doJump(1f, 10);
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

    //下から上へ　どんどん上にあがっていく不具合がある…
    public void doJump(float mysec = 0f, int mypower = 0)
    {
        //if (sec < 0) return;
        var jumpSec = 2f;
        var jumpPower = 10f;
        var _baseLocalPos = transform.localPosition;
        var jumpCnt = 20;

        //数値の上書き
        if (mysec > 0f) jumpSec = mysec;
        if (mypower > 0) jumpPower = mypower;

        // ジャンプ前に毎回、基準座標へ戻しておく
        transform.localPosition = _baseLocalPos;

        var seq = DOTween.Sequence();
        transform.DOLocalJump(
            transform.localPosition,         // 移動終了地点
            jumpPower,                       // ジャンプする力 30くらいがベースか. 50-100でも可
            jumpCnt,                        // 移動終了までにジャンプする回数
            jumpSec                        // アニメーション時間
        )
        //// 基準座標をターゲットにしてジャンプさせる
        //// （上に飛んでまた同じ位置に戻ってくるようになる）
        //transform.DOLocalJump(
        //    _baseLocalPos,   // 終了後に着地する座標
        //    jumpPower,       // ジャンプの頂点の高さ
        //    numJumps,        // 何回バウンドするか
        //    duration         // 時間
        //)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            // 念のため、最終的に位置を合わせておきたいなら
            transform.localPosition = _baseLocalPos;
        });
    }

    //
    public void doCanvasAlpha(float a = 1f, float sec = 0.5f)
    {
        var cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            return;
        }
        DOTween.To(
            () => cg.alpha,          // 何を対象にするのか
            num => cg.alpha = num,   // 値の更新
            a,                  // 最終的な値
            sec                  // アニメーション時間
        );
    }

}
