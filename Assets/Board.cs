using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;

public class Board : MonoBehaviour
{
    public string IDENTITY_POOL_ID = "eu-central-1:2d73d069-7e64-459d-8c60-d7361236cff0";
    DynamoDBContext Context;


    public Text resultText;
    public InputField usernameText;


    public Text lastscore;
    public Text highscore;
    // Start is called before the first frame update
    void Start()
    {
        
        lastscore.text ="Score: " + PlayerPrefs.GetInt("lastGame");
        highscore.text ="HighScore: " + PlayerPrefs.GetInt("highscore");
        usernameText.text = PlayerPrefs.GetString("user","nobody");
        initAWS();
        if (Manager.lives <= 0)
        {
            ScoreEntry scoreEntry = new ScoreEntry
            {
                id = Guid.NewGuid().ToString("N"),
                User = usernameText.text,
                score = PlayerPrefs.GetInt("lastGame"),
                date = DateTime.Now,
                mathType = Manager.mathType
            };
            addScore(scoreEntry);
        }

    }

    public void updateUsername(string user) {
        if (string.IsNullOrEmpty(usernameText.text))
            return;
        PlayerPrefs.SetString("user",user);
    }

    private void addScore(ScoreEntry scoreEntry)
    {
       

        // Save the book.
        Context.SaveAsync(scoreEntry, (result) => {
            if (result.Exception == null)
                resultText.text += @"score saved";
            else
                resultText.text += result.Exception.Message;
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initAWS()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;

        var credentials = new CognitoAWSCredentials(IDENTITY_POOL_ID, RegionEndpoint.EUCentral1);
        // Initialize the Amazon Cognito credentials provider


        AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, RegionEndpoint.EUCentral1);
        Context = new DynamoDBContext(client);

       // GetTableDetails(client);
    }

    private void GetTableDetails(AmazonDynamoDBClient client)
    {
        resultText.text += ("\n*** Retrieving table information ***\n");
        var request = new DescribeTableRequest
        {
            TableName = @"speedmath_scores"
        };
        client.DescribeTableAsync(request, (result) =>
        {
            if (result.Exception != null)
            {
                resultText.text += result.Exception.Message;
                Debug.Log(result.Exception);
                return;
            }
            var response = result.Response;
            TableDescription description = response.Table;
            resultText.text += ("Name: " + description.TableName + "\n");
            resultText.text += ("# of items: " + description.ItemCount + "\n");
            resultText.text += ("Provision Throughput (reads/sec): " +
                description.ProvisionedThroughput.ReadCapacityUnits + "\n");
            resultText.text += ("Provision Throughput (reads/sec): " +
                description.ProvisionedThroughput.WriteCapacityUnits + "\n");

        }, null);
    }

    public void easyStart() {

        if (string.IsNullOrEmpty(usernameText.text))
            return;
        
        Manager.mathType = Manager.MathType.PLUSMINUS;
        SceneManager.LoadScene("Game");
    }
    public void mediumStart()
    {
        if (string.IsNullOrEmpty(usernameText.text))
            return;
        Manager.mathType = Manager.MathType.MALGETEILT;
        SceneManager.LoadScene("Game");
    }
    public void hardStart()
    {

        if (string.IsNullOrEmpty(usernameText.text))
            return;
        Manager.mathType = Manager.MathType.ALLES;
        SceneManager.LoadScene("Game");
    }

    public void showSettings()
    {
       
        SceneManager.LoadScene("Settings");
    }
}
