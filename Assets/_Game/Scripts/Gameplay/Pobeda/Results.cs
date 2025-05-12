using UnityEngine;

public class Results : MonoBehaviour
{
    public static Results instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int score;
    public int maxScore;
    public string username;
    //public string password;


    public void SetMaxScore()
    {
        if (score > maxScore)
        {
            maxScore = score;
        }
    }

    public void SaveData(UserData data)
    {
        //Sacuvaj u bazu sve izmene.
        username = data.username;
        score = data.score;
        maxScore = data.maxScore;
    }

    public void LoadData(UserData data)
    {
        //Ucitaj iz baze sve izmene.
        username = data.username;
        score = data.score;
        maxScore = data.maxScore;
    }
}


public class UserData
{
    public string username;
    public string password;
    public int score;
    public int maxScore;
    public UserData(string username, string password, int score, int maxScore)
    {
        this.username = username;
        this.password = password;
        this.score = score;
        this.maxScore = maxScore;
    }
}