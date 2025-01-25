using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabCharaStatus : MonoBehaviour
{
    //public L
    public List<GameObject> charaObjs;


    //
    private void OnEnable()
    {

    }

    //
    private void startTabCharaStatus()
    {
        //var charas
        var players = new List<int>();//test

        //Object set
        foreach(int i in players)
        {
            int score = 100;
            var btn = gameObject.transform.Find("CharaStatus0" + i);

            //image
            var path = "test/test.png";
            btn.transform.Find("Face").GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;

            //score
            gameObject.transform.Find("Sores/Txt").GetComponent<Text>().text = score.ToString();

            //item
            gameObject.transform.Find("Items/Item0 + i").gameObject.SetActive(true);
            GetComponent<Image>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        }
    }

    //
    



    
}
