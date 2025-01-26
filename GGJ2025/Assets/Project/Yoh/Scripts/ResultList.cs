using Project.GGJ2025;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultList : MonoBehaviour
{
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

        //
        StartCoroutine(startResultList());

        //Player情報に変化があったときに受け取る設定→リザルトは不要か
        if (false)
        {
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
    }

    //
    public void touchNext()
    {
        //シーン遷移はナシに
        //SceneManager.LoadScene("GameScene");
    }

    //
    public IEnumerator startResultList()
    {
        //var charas
        //var players = new List<int>();//test

        //ソートしてランキング
        var playerInfos = DataStore.Instance.PlayerInfos;
        playerInfos.OrderByDescending(p => p.Score);

        //Object set
        int rank = 1;
        foreach (DataStore.PlayerInfo pf in playerInfos)
        {
            Debug.Log("rank:" + rank);
            yield return StartCoroutine(makeResultStatus(pf, rank));
            yield return new WaitForSeconds(1.2f);//test 0.2f
            rank++;
        }
    }

    //
    public IEnumerator makeResultStatus(DataStore.PlayerInfo pf, int rank)
    {
        var btn = getBtnResult(pf.PlayerId);

        //rank
        btn.transform.Find("Ranks/Txt").GetComponent<Text>().text = rank.ToString();

        //image
        var path = "test/test.png";
        btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

        //Score
        btn.transform.Find("Scores/Txt").GetComponent<Text>().text = pf.Score.ToString() + "点";

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
