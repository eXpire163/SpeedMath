
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Manager;
using Random = UnityEngine.Random;

public class Rechnung : MonoBehaviour
{

    public enum State { Unsolved, OK, Fail, Dead }

    public Text rechnungsText;
    public InputField input;
    
    MathType mathType;
   public  State state = State.Unsolved;
    int number1;
    int number2;
    int result;
    string sign = "+";
    float speed = 0f;
    public float speedBalance = 20f;

    public AudioClip[] soundOK;
    public AudioClip[] soundFail;
    AudioSource audioSource;

    public int typeScroe { get { return (int)mathType; } }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    public void init(MathType mathType, float speed)
    {
        this.speed = speed;
        this.mathType = mathType;
        switch (mathType) {

            case MathType.PLUS:
                plus();
                break;
            case MathType.MINUS:
                minus();
                break;
            case MathType.MAL:
                mal();
                break;
            case MathType.GETEILT:
                geteilt();
                break;
            default:
                plus();
                Debug.Log("mathtype not implemented:  " + mathType);
                break;




        }
        setText();

        input.Select();
        input.ActivateInputField();

    }

   

    private void setText()
    {
        rechnungsText.text = number1 + " " + sign + " " + number2 + " " + " = ";
    }
    public string ToDebug() {
        return number1 + " " + sign + " " + number2 + " " + " = " + input.text;

    }

   
    private void plus()
    {
        int max = 9;
        if (Manager.score > 50)
            max = 20;
        else if (Manager.score > 100)
            max = 30;
        else if (Manager.score > 200)
            max = 50;
        number1 = Random.Range(0, max);
        number2 = Random.Range(0, max);
        result = number1 + number2;
        sign = "+";
    }

    private void minus()
    {
        int max = 20;
        if (Manager.score > 50)
            max = 40;
        else if (Manager.score > 100)
            max = 70;
        else if (Manager.score > 200)
            max = 100;
        number1 = Random.Range(1, max);
        number2 = Random.Range(0, number1);
        result = number1 - number2;
        sign = "-";
    }

    private void mal()
    {
        int max = 5;
        if (Manager.score > 100)
            max = 7;
        else if (Manager.score > 200)
            max = 10;

        number1 = Random.Range(0, max);
        number2 = Random.Range(0, max);
        result = number1 * number2;
        sign = "x";
    }

    private void geteilt()
    {
        int max = 5;
        if (Manager.score > 100)
            max = 7;
        else if (Manager.score > 200)
            max = 10;

        number2 = Random.Range(1, max);
        result = Random.Range(0, max);

        number1 = result * number2;

       sign = ":";
    }


   

    // Update is called once per frame
    void Update()
    {
      
      state = validateInput();
        updateColor();
        if (state == State.Unsolved) {
            moveDown();
        }
    }

    private void moveDown()
    {
        transform.position -=new Vector3 (0, speed * Time.deltaTime * speedBalance, 0);
    }

    private State validateInput()
    {
        if (state != State.Unsolved) {
            return state; ;
        }

        string intxt = input.text;

        if (result.ToString().Equals(intxt))
        {
            playSuccess();
            return State.OK;
        }

        if (string.IsNullOrEmpty(intxt) || result.ToString().StartsWith(intxt)) {
            return State.Unsolved;
        }

        if (!result.ToString().StartsWith(intxt)) {
            playFail();
            return State.Fail;
        }
      
        throw new Exception("Undefined state");
    }

    void updateColor() {
        switch (state) {
            case State.Unsolved:
                break;
            case State.OK:
                setColor(Color.green);
                break;
            case State.Fail:
                setColor(Color.red);
                break;
            default:
                break;
        }
    }

    internal void setDead()
    {
        input.text = "--";
    }

    private void setColor(Color color)
    {
        rechnungsText.color = color;        
    }

    private void playSuccess() {
        audioSource.clip = soundOK[Random.Range(0,soundOK.Length-1)];
        audioSource.Play();
       StartCoroutine( Kill());
    }
    private void playFail() {
        audioSource.clip = soundFail[Random.Range(0, soundFail.Length - 1)];
        audioSource.Play();
        Manager.lives--;
        StartCoroutine(Kill());
    }

    IEnumerator Kill() {
        input.DeactivateInputField();
        yield return new WaitForSeconds(0.8f);
        state = State.Dead;
    }
}
