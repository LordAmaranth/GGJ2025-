using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using R3;
using Project.GGJ2025;
using System.Linq;

//
public class CanvasBattle : MonoBehaviour
{
    //public L
    public List<GameObject> charaObjs;

    //
    private void OnEnable()
    {
        //
        foreach (Transform tr in gameObject.transform)
        {
            if (tr.gameObject.name == "Bg") continue;
            tr.gameObject.SetActive(false);
        }

        //èââÒï\é¶
        startTabCharaStatus();

        //PlayerèÓïÒÇ…ïœâªÇ™Ç†Ç¡ÇΩÇ∆Ç´Ç…éÛÇØéÊÇÈ
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
        var players = DataStore.Instance.PlayerInfos;

        //Object set
        int i = 1;
        foreach (var pf in players)
        {
            var btn = getPlayerObj(pf.PlayerId);

            //image
            var path = "test/test.png";//TODO!Ç†Ç∆Ç≈ëfçﬁÇéwíË
            btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

            //color TODO!!Ç◊ÇΩèëÇ´
            var c = Color.red;
            if (pf.PlayerId == 2) c = Color.blue;
            if (pf.PlayerId == 3) c = Color.yellow;
            if (pf.PlayerId == 4) c = Color.green;
            if (pf.PlayerId == 5) c = Color.cyan;
            btn.transform.Find("Face").GetComponent<Image>().color = c;

            //item
            if(false) gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);

            //Image
            if (false)
            {
                GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            }
            i++;
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
        if (false)
        {
            btn.transform.Find("Sores/Txt").GetComponent<Text>().text = score.ToString();
        }
    }
    

    //
    public void setPlayserHp(int id, int hp)
    {
        //HP
        var btn = getPlayerObj(id);
        btn.transform.Find("Hps/Txt").GetComponent<Text>().text = hp.ToString();
    }



    
}
