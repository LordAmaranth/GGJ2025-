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
    public float size;//wait�x�[�X�T�C�Y
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


    //������
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


    //�Ղ�Ղ�n(�J�����_�[�Ȃ�)
    public void doShakePuru()
    {
        float bs = baseScale;
        float shakeSec = 0.4f;
        float scaleSmall = bs * 0.95f;//�g�k�̒l�i���j
        float scaleBig = bs * 1.03f;//�g�k�̒l�i��j

        Sequence seq = DOTween.Sequence();
        seq.Append(this.transform.DOScale(scaleSmall, 0.05f));
        seq.Append(this.transform.DOScale(scaleBig, 0.05f));
        seq.Append(this.transform.DOScale(bs, 0.1f));
        //seq.Append(this.transform.)

        if (true) seq.Join(this.transform.DOShakePosition(shakeSec, 1f));
    }


    //tween��doWait
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

    //�������ց@�ǂ�ǂ��ɂ������Ă����s�������c
    public void doJump(float mysec = 0f, int mypower = 0)
    {
        //if (sec < 0) return;
        var jumpSec = 2f;
        var jumpPower = 10f;
        var _baseLocalPos = transform.localPosition;
        var jumpCnt = 20;

        //���l�̏㏑��
        if (mysec > 0f) jumpSec = mysec;
        if (mypower > 0) jumpPower = mypower;

        // �W�����v�O�ɖ���A����W�֖߂��Ă���
        transform.localPosition = _baseLocalPos;

        var seq = DOTween.Sequence();
        transform.DOLocalJump(
            transform.localPosition,         // �ړ��I���n�_
            jumpPower,                       // �W�����v����� 30���炢���x�[�X��. 50-100�ł���
            jumpCnt,                        // �ړ��I���܂łɃW�����v�����
            jumpSec                        // �A�j���[�V��������
        )
        //// ����W���^�[�Q�b�g�ɂ��ăW�����v������
        //// �i��ɔ��ł܂������ʒu�ɖ߂��Ă���悤�ɂȂ�j
        //transform.DOLocalJump(
        //    _baseLocalPos,   // �I����ɒ��n������W
        //    jumpPower,       // �W�����v�̒��_�̍���
        //    numJumps,        // ����o�E���h���邩
        //    duration         // ����
        //)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            // �O�̂��߁A�ŏI�I�Ɉʒu�����킹�Ă��������Ȃ�
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
            () => cg.alpha,          // ����Ώۂɂ���̂�
            num => cg.alpha = num,   // �l�̍X�V
            a,                  // �ŏI�I�Ȓl
            sec                  // �A�j���[�V��������
        );
    }

}
