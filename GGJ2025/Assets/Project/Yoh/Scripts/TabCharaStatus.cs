using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;
using Project.GGJ2025;


public class TabCharaStatus : MonoBehaviour
{
    //public L
    public List<GameObject> charaObjs;

    //
    private void OnEnable()
    {
        //

    }

    //
    private void OnStart()
    {
        //Playerî•ñ‚É•Ï‰»‚ª‚ ‚Á‚½‚Æ‚«‚ÉŽó‚¯Žæ‚é
        DataStore.Instance.PlayerInfos.ForEach(x =>
        {
            x.Hp.Subscribe(hp =>
            {
                setPlayserHp(x.PlayerId, hp);
            })
            .AddTo(this);
            x.Score.Subscribe(score =>
               setPlayerScore(x.PlayerId, score)
            );
        });
    }

    //
    private void startTabCharaStatus()
    {
        //var charas
        var players = new List<int>();//test

        //Object set
        foreach(int i in players)
        {
            var btn = getPlayerObj(i);

            //image
            var path = "test/test.png";
            btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

            //item
            gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);
            GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
    }

    //
    public GameObject getPlayerObj(int i)
    {
        var btn = gameObject.transform.Find("CharaStatus0" + i).gameObject;
        return btn;
    }

    //
    public void setPlayerScore(int id, int score)
    {
        var btn = getPlayerObj(id);
        btn.transform.Find("Sores/Txt").GetComponent<Text>().text = score.ToString();
    }
    

    //
    public void setPlayserHp(int id, int hp)
    {
        //HP
        var btn = getPlayerObj(id);
        btn.transform.Find("Hps/Txt").GetComponent<Text>().text = hp.ToString();
    }



    
}
