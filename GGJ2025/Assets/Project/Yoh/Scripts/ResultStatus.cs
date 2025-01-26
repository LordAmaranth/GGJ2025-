using Project.GGJ2025;
using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStatus : MonoBehaviour
{
    //
    private void OnEnable()
    {

    }

    //
    private void OnStart()
    {
        foreach (Transform tr in gameObject.transform)
        {
            if (tr.gameObject.name == "Bg") continue;
            tr.gameObject.SetActive(false);
        }
    }

    //
    public void startResultStatus()
    {
        var players = new List<int>();//test

        //Object set        
        foreach (int i in players)
        {
            int score = 100;
            var btn = gameObject.transform.Find("CharaStatus0" + i).transform.gameObject;
            btn.SetActive(true);

            //image
            var path = "test/test.png";
            btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

            //score
            gameObject.transform.Find("Sores/Txt").GetComponent<Text>().text = score.ToString() + "“_";

            //item
            gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);
            GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
    }
}
