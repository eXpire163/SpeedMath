using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHelper : MonoBehaviour
{

    public GUISkin skin;
    // Start is called before the first frame update
    void OnGUI()
    {

        GUI.skin = skin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
