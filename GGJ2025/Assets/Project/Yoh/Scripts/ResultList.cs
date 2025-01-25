using Project.GGJ2025;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultList : MonoBehaviour
{
    //
    private void OnEnable()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //
    private void OnStart()
    {
        //
        foreach (Transform tr in gameObject.transform)
        {
            if (tr.gameObject.name == "Bg") continue;
            tr.gameObject.SetActive(false);
        }

        //DataStore.Instance.PlayerInfos

        //Playerî•ñ‚É•Ï‰»‚ª‚ ‚Á‚½‚Æ‚«‚ÉŽó‚¯Žæ‚éÝ’è
        DataStore.Instance.PlayerInfos.ForEach(x =>
        {
            x.Hp.Subscribe(hp =>
            {
                makeResultStatusHp(x.PlayerId, hp);
            })
            .AddTo(this);
            x.Score.Subscribe(score =>
               makeResultStatusScore(x.PlayerId, score)
            ) ;
        });
    }

    //
    public IEnumerator startResultList()
    {
        //var charas
        var players = new List<int>();//test

        //Object set
        foreach (int i in players)
        {
            yield return StartCoroutine(makeResultStatus(i));
            yield return new WaitForSeconds(0.2f);
        }
    }

    //
    public IEnumerator makeResultStatus(int i)
    {
        int score = 100;
        var btn = getBtnResult(i);

        //image
        var path = "test/test.png";
        btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

        //item
        gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);
        GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

        yield return null;
    }

    //
    public GameObject getBtnResult(int i)
    {
        var btn = gameObject.transform.Find("ResultStatus0" + i).gameObject;
        return btn;
    }

    //
    public void makeResultStatusHp(int i, int hp)
    {
        var btn = getBtnResult(i);
        btn.transform.Find("Hps/Txt").GetComponent<Text>().text = hp.ToString();
    }

    //
    public void makeResultStatusScore(int i,  int score)
    {
        var btn = getBtnResult(i);
        btn.transform.Find("Sores/Txt").GetComponent<Text>().text = score.ToString();
    }



}
