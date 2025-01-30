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
    [SerializeField] private PlayerConfig playerConfig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject cnvResult;
    public GameObject panel;

    //
    private void OnEnable()
    {
        //panel
        setCanvasAlpha(1f, 0.2f);
        setPanelAlpha(0.3f, 0f);

        //
        foreach (Transform tr in gameObject.transform)
        {
            if (tr.gameObject.name == "Bg") continue;
            tr.gameObject.SetActive(false);
            var face = tr.gameObject.transform.Find("Face").transform.gameObject;
            foreach (Transform trr in face.transform)
            {
                GameObject.Destroy(trr.gameObject);
            }
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
    public void touchTest()
    {
        //var parent = this.gameObject;//btn.transform.Find("Face");
        //Instantiate(
        //    playerConfig.PlayerVisuals[pf.PlayerId % playerConfig.PlayerVisuals.Length],
        //    parent);
    }

    //
    public void touchNext()
    {
        //SceneManager.LoadScene("GameScene");
    }

    //
    public IEnumerator startResultList()
    {
        //order descend playerInfo
        var playerInfos = DataStore.Instance.PlayerInfos;

        if(playerInfos.Count < 1)
        {
            Debug.LogError("No playerInfos");
        }
        
        //Rank 昇順。
        var playerInfosOrdered = playerInfos.OrderBy(p => p.Rank.Value).ToList();
        //playerInfos.Sort((a, b) => a.Rank - b.Rank);

        //Object set
        int rank = 1;//Rank.Valueが正しくなさげなので応急処置。0位,2位となってる？
        int topId = 0;
        yield return new WaitForSeconds(1.0f);//First Wait
        foreach (DataStore.PlayerInfo pf in playerInfosOrdered)
        {
            var pid = pf.PlayerId;
            Debug.Log("Rank:" + pf.Rank.Value + " PlayerId:" + pid);
            if (rank == 1) topId = pid;
            yield return StartCoroutine(makeResultStatus(pf, rank));
            yield return new WaitForSeconds(0.7f);//test 0.2f
            rank++;
        }
        //勝者表示
        var wins = gameObject.transform.parent.Find("WinnerObjs").gameObject;
        wins.transform.Find("WinnerObj0" + topId).gameObject.SetActive(true);

        SEManager.Instance.Play(SEPath.DON_PUFF);
        SEManager.Instance.Play(SEPath.CLAP00);

        //背景の色を無くす
        //yield return new WaitForSeconds(1.0f);
        setPanelAlpha(0f, 0.5f);
        yield return new WaitForSeconds(3.0f);
        setCanvasAlpha(0.4f, 1.0f);

        //ボタンの色を薄く
        foreach(Transform tr in gameObject.transform)
        {
            if(tr.gameObject.name == "Bg") continue;
            tr.Find("Bg").GetComponent<Image>().enabled = false;
        }

    }

    //
    public IEnumerator makeResultStatus(DataStore.PlayerInfo pf, int rank)
    {
        //var rank = pf.Rank.Value;
        var btn = getBtnResult(rank - 1);// pf.PlayerId);
        btn.SetActive(true);
        btn.transform.Find("Bg").GetComponent<Image>().enabled = true;

        //rank
        btn.transform.Find("Ranks/Txt").GetComponent<Text>().text = rank.ToString();

        //image
        if (true)
        {
            //var path = "hat_00" + (pf.PlayerId + 1);// + ".png";
            var path = "Chara0" + pf.PlayerId.ToString();
            //Debug.Log("pid:" + pf.PlayerId + " path:" + path);
            if(true) btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }

        //new
        if (false)
        {
            var parent = btn.transform.Find("Face");
            Instantiate(
                playerConfig.PlayerVisuals[pf.PlayerId % playerConfig.PlayerVisuals.Length],
                parent);
        }

        //Score
        btn.transform.Find("Scores/Txt").GetComponent<Text>().text = (1000 - (rank * 200)).ToString();//pf.Score.Value.ToString() + "位";

        //color TODO!!べた書き
        //btn.transform.Find("Face").GetComponent<Image>().color = c;

        //item
        // gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);
        //GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

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

    //
    public void setPanelAlpha(float a, float sec)
    {
        panel.GetComponent<UIObj>().setAlpha(a, sec);
        //Color color = panel.GetComponent<Image>().color;
        //color.a = a;
        //panel.GetComponent<Image>().color = color;        
    }

    //
    public void setCanvasAlpha(float a, float sec)
    {
        cnvResult.GetComponent<UIObj>().doCanvasAlpha(a, sec);
    }

}
