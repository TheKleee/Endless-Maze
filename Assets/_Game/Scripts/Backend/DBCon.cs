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
        try
        {
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
                    SceneManager.LoadScene("Main Menu");
                    connection.Close();
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            DBManager.instance.Error(ex.Message);
            return false;
        }
        finally 
        { 
            connection.Close();
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

        if (DBManager.instance.CheckError())
            return false;

        DBManager.instance.Error(DBManager.instance.CheckUsername(IsUsernameTaken(username)));

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
        catch (Exception ex)
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


}
