using Project.GGJ2025;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;

public class ResultList : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //
    private void OnEnable()
    {
        foreach (Transform tr in gameObject.transform)
        {
            if (tr.gameObject.name == "Bg") continue;
            tr.gameObject.SetActive(false);
        }

        var wins = gameObject.transform.parent.Find("WinnerObjs").gameObject;
        foreach (Transform tr in wins.transform)
        {
            tr.gameObject.SetActive(false);
        }

        //
        StartCoroutine(startResultList());

        //
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
        //SceneManager.LoadScene("GameScene");
    }

    //
    public IEnumerator startResultList()
    {
        //var charas
        //var players = new List<int>();//test

        //order descend playerInfo
        var playerInfos = DataStore.Instance.PlayerInfos;

        if(playerInfos.Count < 1)
        {
            Debug.LogError("No playerInfos");
        }
        
        var playerInfosOrdered = playerInfos.OrderBy(p => p.Rank.Value).ToList();
        //playerInfos.Sort((a, b) => a.Rank - b.Rank);

        //応急
        var players = new List<DataStore.PlayerInfo>();
        foreach(var pod in playerInfosOrdered)
        {
            players.Add(pod);
        }

        //Object set
        int rank = 1;
        int topId = 0;
        yield return new WaitForSeconds(1.0f);//First Wait
        foreach (DataStore.PlayerInfo pf in players)
        {
            Debug.Log("rank:" + rank + " Score:" + pf.Score.Value);
            if (rank == 1) topId = pf.PlayerId;
            yield return StartCoroutine(makeResultStatus(pf, rank));
            yield return new WaitForSeconds(1.2f);//test 0.2f
            rank++;
        }
        //勝者表示
        var wins = gameObject.transform.parent.Find("WinnerObjs").gameObject;
        wins.transform.Find("WinnerObj0" + topId).gameObject.SetActive(true);

        SEManager.Instance.Play(SEPath.DON_PUFF);
        SEManager.Instance.Play(SEPath.CLAP00);
    }

    //
    public IEnumerator makeResultStatus(DataStore.PlayerInfo pf, int rank)
    {
        var btn = getBtnResult(pf.PlayerId);
        btn.SetActive(true);

        //rank
        btn.transform.Find("Ranks/Txt").GetComponent<Text>().text = rank.ToString();

        //image
        var path = "hat_00" + (pf.PlayerId + 1);// + ".png";
        Debug.Log("pid:" + pf.PlayerId + " path:" + path);
        if(true) btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

        //Score
        btn.transform.Find("Scores/Txt").GetComponent<Text>().text = (1000 - (rank * 200)).ToString();//pf.Score.Value.ToString() + "位";

        //item
        // gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);
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
