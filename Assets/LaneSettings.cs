using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSettings : MonoBehaviour
{
    // Start is called before the first frame update

    
    public GameObject box;

    public int colorID = 9;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer srbox = box.GetComponent<SpriteRenderer>();


        sr.color = ColorHelper.lightColors[colorID];
        srbox.color = ColorHelper.darkColors[colorID];
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
