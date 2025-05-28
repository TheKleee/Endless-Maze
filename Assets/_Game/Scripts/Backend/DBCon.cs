using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.MemoryProfiler;


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
    }

    public bool Login()
    {
        DBManager.instance.Error();
        string username = DBManager.instance.username;
        string password = BitConverter.ToString(new SHA256Managed()
            .ComputeHash(Encoding.UTF8.GetBytes(DBManager.instance.password))).Replace("-", "")
            .ToLower();

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        using MySqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Players WHERE username = @username";
        command.Parameters.AddWithValue("@username", username);
        var result = Convert.ToInt32(command.ExecuteScalar());

        if (result <= 0)
        {
            DBManager.instance.Error("Username not found");
            connection.Close();
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
                SceneManager.LoadScene("Main Menu");
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

        (bool check, string e) = DBManager.instance.CheckPassword();
        DBManager.instance.Error(check ? "" : e);
        
        string username = DBManager.instance.username;
        string password = BitConverter.ToString(new SHA256Managed()
            .ComputeHash(Encoding.UTF8.GetBytes(DBManager.instance.password))).Replace("-", "")
            .ToLower();

        DBManager.instance.Error(DBManager.instance.CheckUsername(IsUsernameTaken(username)));

        if (DBManager.instance.CheckError())
            return false;

        if (username.Length < 3 || password.Length < 6)
        {
            
        }
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
        catch (MySqlException err)
        {
            DBManager.instance.Error(err.Message);
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

        command.CommandText = "SELECT Count(*) FROM Players WHERE username=@username";
        command.Parameters.AddWithValue("@username", username);
        try
        {
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        finally
        {
            connection.Close();
        }
    }
}
