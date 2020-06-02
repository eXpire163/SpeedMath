
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Rechnung;

public class Manager : MonoBehaviour
{

    public enum MathType
    {
        PLUS = 1, MINUS = 2, MULTIPLY = 5, DIVIDE = 7,

        PLUSMINUS = 10,
        ALLES = 12,
        MALGETEILT = 11,

    }

    
    public Text scoreText;
    public Text livesText;
    public Text startText;
    public Image startPanal;


    public GameObject[] lanes;

    public GameObject rechnungsPrefab;
    public GameObject rechnung = null;
    public Rechnung rechClass = null;

    public Transform startPos;
    public Transform deadPos;

    public AudioClip[] songs;

    public static MathType mathType = MathType.PLUSMINUS;
    public static int score = 0;
    public static int lives = 5;
    public static float speed = 2f;
    public static float speed_start = 2f;
    public static float speed_increase = 0.03f;

    float startDelay = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        lives = 5;
        speed = speed_start;
        switch (mathType)
        {
            case MathType.PLUSMINUS:
                GetComponent<AudioSource>().clip = songs[0];
                break;
            case MathType.MALGETEILT:
                GetComponent<AudioSource>().clip = songs[1];
                break;
            default:
                GetComponent<AudioSource>().clip = songs[2];
                break;
        }
        GetComponent<AudioSource>().Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (startDelay >= 0)
        {
            udpateStartText();
            startDelay -= Time.deltaTime;
            return;
        }
        else {
        }
        startText.enabled = false;
        startPanal.enabled = false;
        if (lives <= 0)
        {
            SaveScore();
            SceneManager.LoadScene("Board");

        }
        else
        {

            if (rechnung == null)
            {

                Transform start = getStartPoint();
                rechnung = Instantiate(rechnungsPrefab);
                rechnung.transform.parent = null;
                rechnung.transform.position = startPos.position;
                rechClass = rechnung.GetComponent<Rechnung>();
                rechClass.init(nextMathType(), speed);

                speed += speed_increase;

            }
            if (rechClass.isAlive == false)
            {
                if (rechClass.state == State.OK)
                {
                    score += rechClass.typeScroe;
                }
                Debug.Log(rechClass.ToDebug());
                Destroy(rechnung);
                return;
            }

            if (rechnung.transform.position.y < deadPos.position.y)
            {
                rechClass.setDead();
            }

            updateUI();
        }
    }

    private void udpateStartText()
    {

        startText.enabled = true;
        startPanal.enabled = true;

        int restDelay =(int) Math.Floor(startDelay);

        if (restDelay > 0)
        {
            startText.text = "" + restDelay;
        }
        else {
            startText.text = "GO";
        }
        startText.color = ColorHelper.lightColors[restDelay];
        startPanal.color = ColorHelper.darkColors[restDelay];


    }

    private MathType nextMathType()
    {
        switch (mathType)
        {

            case MathType.PLUSMINUS:
                return UnityEngine.Random.Range(0f, 1f) > 0.5f ? MathType.PLUS : MathType.MINUS;
            case MathType.MALGETEILT:
                return UnityEngine.Random.Range(0f, 1f) > 0.5f ? MathType.MULTIPLY : MathType.DIVIDE;
            default:
                int range = UnityEngine.Random.Range(0, 3);
                switch (range)
                {
                    case 0:
                        return MathType.PLUS;
                    case 1:
                        return MathType.MINUS;
                    case 2:
                        return MathType.MULTIPLY;
                    default:
                        return MathType.DIVIDE;

                }

        }
    }

    private void SaveScore()
    {
        Debug.Log("your scroe: " + score);
        Debug.Log("last scroe: " + PlayerPrefs.GetInt("lastGame"));
        int highscore = PlayerPrefs.GetInt("highscore");
        Debug.Log("highscroe: " + highscore);

        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        PlayerPrefs.SetInt("lastGame", score);

        PlayerPrefs.Save();

    }

    private void updateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }

    private Transform getStartPoint()
    {

        int laneNumber = getAliveLane();
        startPos.position = new Vector3(lanes[laneNumber].transform.position.x, startPos.position.y, startPos.position.z);
        return startPos;
    }

    private int getAliveLane()
    {
        return UnityEngine.Random.Range(0, lanes.Length - 1);
    }
}