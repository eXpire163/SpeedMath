
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Rechnung;

public class Manager : MonoBehaviour
{

    public enum MathType
    {
        PLUS = 1, MINUS = 2, MAL = 5, GETEILT = 7,

        PLUSMINUS = 10,
        ALLES = 12,
        MALGETEILT = 11,

    }

    public static MathType mathType = MathType.PLUSMINUS;
    public Text scoreText;
    public Text livesText;


    public GameObject[] lanes;

    public GameObject rechnungsPrefab;
    public GameObject rechnung = null;
    public Rechnung rechClass = null;

    public Transform startPos;
    public Transform deadPos;

    public AudioClip[] songs;

    public static int score = 0;
    public static int lives = 5;
    public static float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        lives = 5;
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

                speed += 0.01f;

            }
            if (rechClass.state == State.Dead)
            {
                score += rechClass.typeScroe;
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

    private MathType nextMathType()
    {
        switch (mathType)
        {

            case MathType.PLUSMINUS:
                return UnityEngine.Random.Range(0f, 1f) > 0.5f ? MathType.PLUS : MathType.MINUS;
            case MathType.MALGETEILT:
                return UnityEngine.Random.Range(0f, 1f) > 0.5f ? MathType.MAL : MathType.GETEILT;
            default:
                int range = UnityEngine.Random.Range(0, 3);
                switch (range)
                {
                    case 0:
                        return MathType.PLUS;
                    case 1:
                        return MathType.MINUS;
                    case 2:
                        return MathType.MAL;
                    default:
                        return MathType.GETEILT;

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