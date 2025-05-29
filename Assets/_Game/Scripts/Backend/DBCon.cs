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
            command.CommandText = "SELECT username, wins, losses FROM Players";
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                PlayerData pd = new PlayerData()
                {
                    username = reader.GetString("username"),
                    wins = reader.GetInt32("wins"),
                    losses = reader.GetInt32("losses")
                };
                DBManager.instance.SetPlayerData(pd);
            }
        }
        finally
        {
            connection.Close();
        }
    }
}
