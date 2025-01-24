using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobeAnimation : MonoBehaviour
{
    private Image image;
    private Material material;
    private Vector2 _MainTexOffset = new Vector2(0, 0);
    private Vector2 _MainTexOffset2 = new Vector2(0, 0);
    private Vector2 _MainTexOffset3 = new Vector2(0, 0);
    private Vector2 _HotlineTexOffset3 = new Vector2(0, 0);

    public Vector2 MainTexOffset = new Vector2(0, 0);
    public Vector2 MainTexOffset2 = new Vector2(0, 0);
    public Vector2 MainTexOffset3 = new Vector2(0, 0);
    public float HotlineTexOffset3;

    // Use this for initialization
    void Start ()
	{
	    image = GetComponent<Image>();
	    material = image.material;
    }
	
	// Update is called once per frame
	void Update ()
	{

        _MainTexOffset += MainTexOffset;
	    _MainTexOffset.x =_MainTexOffset.x % 1;
	    _MainTexOffset.y = _MainTexOffset.y % 1;
        _MainTexOffset2 += MainTexOffset2;
	    _MainTexOffset2.x = _MainTexOffset2.x % 1;
	    _MainTexOffset2.y = _MainTexOffset2.y % 1;
        _MainTexOffset3 += MainTexOffset3;
	    _MainTexOffset3.x = _MainTexOffset3.x % 1;
	    _MainTexOffset3.y = _MainTexOffset3.y % 1;
        _HotlineTexOffset3.x += HotlineTexOffset3;
	    _HotlineTexOffset3.x = _HotlineTexOffset3.x % 1;

        material.SetTextureOffset("_MainTex", _MainTexOffset);
	    material.SetTextureOffset("_MainTex2", _MainTexOffset2);
	    material.SetTextureOffset("_MainTex3", _MainTexOffset3);
	    material.SetTextureOffset("_HotlineTex", _HotlineTexOffset3);
    }
}
