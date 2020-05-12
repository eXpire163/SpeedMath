using Amazon.DynamoDBv2.DataModel;
using System;
using static Manager;

[DynamoDBTable("speedmath_scores")]
public class ScoreEntry
{
    [DynamoDBHashKey]   // Hash key.
    public string id { get; set; }
    [DynamoDBProperty]
    public string User { get; set; }
    [DynamoDBProperty]
    public int score { get; set; }
    [DynamoDBProperty]
    public DateTime date { get; set; }
    [DynamoDBProperty]
    public MathType mathType { get; set; }
    // [DynamoDBProperty("Authors")]    // Multi-valued (set type) attribute.
    // public List<string> BookAuthors { get; set; }
}
