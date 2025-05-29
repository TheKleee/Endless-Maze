using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using System;
using EM_Hashing;

public class DBCon : MonoBehaviour
{
    private string connectionString;
    public static string player { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        connectionString = "Server=localhost;Database=EndlessMaze;User ID=root;Pooling=false;";
        DBManager.instance.registerPlayer += Register;
        DBManager.instance.loginPlayer += Login;
        DBManager.instance.readPlayerData += LeaderboardData;
        DBManager.instance.win += AddWin;
        DBManager.instance.lose += AddLoss;
    }

    public bool Login()
    {
        DBManager.instance.Error();

        string username = DBManager.instance.username;
        string password = DBManager.instance.password;

        if (username.Length < 1 || password.Length < 1)
            DBManager.instance.Error("Username and password fields cannot be empty");

        if (DBManager.instance.CheckError())
            return false;

        password = password.Hash();

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();
        
        command.CommandText = "SELECT COUNT(*) FROM Players WHERE username = @username";
        command.Parameters.AddWithValue("@username", username);
        var result = Convert.ToInt32(command.ExecuteScalar());

        if (result <= 0)
        {
            DBManager.instance.Error("Username not found");
            return false;
        }
        else
        {
            command.CommandText = "SELECT COUNT(*) FROM Players WHERE username = @username AND password = @password ";
            command.Parameters.AddWithValue("@password", password);
            result = Convert.ToInt32(command.ExecuteScalar());
            if (result <= 0)
            {
                DBManager.instance.Error("Incorrect password");
                connection.Close();
                return false;
            }
            else
            {
                player = username;
                connection.Close();
                return true;
            }
        }
    }

    public void OnClickRegisterFromLogin()
    {
        SceneManager.LoadScene("Register");
    }

    public bool Register()
    {
        DBManager.instance.Error();

        string username = DBManager.instance.username;
        string password = DBManager.instance.password;
        
        if (username.Length < 1 || password.Length < 1)
            DBManager.instance.Error("Username and password fields cannot be empty");
        
        (bool check, string e) = DBManager.instance.CheckPassword();
        DBManager.instance.Error(check ? "" : e);

        bool isUsernameTaken = IsUsernameTaken(username);
        DBManager.instance.Error(DBManager.instance.CheckUsername(isUsernameTaken));
        
        if (DBManager.instance.CheckError())
            return false;

        password = password.Hash();

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();

        command.CommandText = "INSERT INTO Players (username, password) VALUES (@username, @password)";
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", password);

        try
        {
            command.ExecuteNonQuery();
            return true;
        }
        catch (MySqlException ex)
        {
            DBManager.instance.Error(ex.Message);
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    bool IsUsernameTaken(string username)
    {
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();

        try
        {
            command.CommandText = "SELECT Count(*) FROM Players WHERE username=@username";
            command.Parameters.AddWithValue("@username", username);
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        catch (MySqlException ex)
        {
            DBManager.instance.Error(ex.Message);
            return true;
        }
        finally
        {
            connection.Close();
        }
    }

    public void LeaderboardData()
    {
        DBManager.instance.ClearPlayerData();
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();

        try
        {
            command.CommandText = "SELECT username, wins, losses, matchdate FROM Players";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PlayerData pd = new PlayerData()
                {
                    username = reader.GetString("username"),
                    wins = reader.GetInt32("wins"),
                    losses = reader.GetInt32("losses"),
                    lastMatch = reader.GetDateTime("matchdate"),
                };
                DBManager.instance.SetPlayerData(pd);
            }
        }
        finally
        {
            connection.Close();
        }
    }

    public void AddWin()
    {
        int id = GetPlayerID();

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();

        try
        {
            command.CommandText = "UPDATE Players SET wins = wins + 1 WHERE playerid = @id;";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    public void AddLoss()
    {
        int id = GetPlayerID();

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();

        try
        {
            command.CommandText = "UPDATE Players SET losses = losses + 1 WHERE playerid = @id;";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        finally
        {
            connection.Close();
        }
    }

    int GetPlayerID()
    {
        string username = DBManager.instance.username;
        Debug.Log(username);
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();

        try
        {
            command.CommandText = "SELECT playerid FROM Players WHERE username=@username";
            command.Parameters.AddWithValue("@username", username);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                Debug.Log(id);
                return id;
            }
            return -1;
        }
        finally
        {
            connection.Close();
        }
    }
}
