using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{

    public Text lastscore;
    public Text highscore;
    // Start is called before the first frame update
    void Start()
    {
        lastscore.text ="Score: " + PlayerPrefs.GetInt("lastGame");
        highscore.text ="HighScore: " + PlayerPrefs.GetInt("highscore");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void easyStart() {
        Manager.mathType = Manager.MathType.PLUSMINUS;
        SceneManager.LoadScene("Game");
    }
    public void mediumStart()
    {
        Manager.mathType = Manager.MathType.MALGETEILT;
        SceneManager.LoadScene("Game");
    }
    public void hardStart()
    {
        Manager.mathType = Manager.MathType.ALLES;
        SceneManager.LoadScene("Game");
    }

    public void showSettings()
    {
       
        SceneManager.LoadScene("Settings");
    }
}
