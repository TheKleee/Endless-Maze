using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class DatabaseConnector : MonoBehaviour
{

    private string connectionString;

    public static string currentPlayer;

    public GameObject username_field;
    public GameObject password_field;

    public TextMeshProUGUI errorMsg;

    public TextMeshProUGUI usernameMsg;
    public TextMeshProUGUI passwordMsg;

    // Start is called before the first frame update
    void Start()
    {
        connectionString = "Server=localhost;Database=game_db;User ID=root;Pooling=false;";
        if (errorMsg != null) errorMsg.enabled = false;
        if (usernameMsg != null) usernameMsg.enabled = false;
        if (passwordMsg != null) passwordMsg.enabled = false;
    }

    public void LoginUser()
    {
        string username = username_field.GetComponent<TMP_InputField>().text;
        string password = password_field.GetComponent<TMP_InputField>().text;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM users WHERE username = @username"; // AND password = @password
                command.Parameters.AddWithValue("@username", username);
                var result = Convert.ToInt32(command.ExecuteScalar());

                if (result <= 0)
                {
                    usernameMsg.enabled = true;
                }
                else
                {
                    command.CommandText = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password ";
                    command.Parameters.AddWithValue("@password", password);
                    result = Convert.ToInt32(command.ExecuteScalar());
                    if (result <= 0)
                    {
                        usernameMsg.enabled = false;
                        passwordMsg.enabled = true;
                    }
                    else
                    {
                        currentPlayer = username;
                        SceneManager.LoadScene("Main Menu");
                    }
                }

                /*command.Parameters.AddWithValue("@password", password);

                var result = Convert.ToInt32(command.ExecuteScalar());
                if (result > 0)
                {
                    SceneManager.LoadScene("Main Menu");
                }
                else SceneManager.LoadScene("Login");*/
            }
        }
    }

    public void OnClickRegisterFromLogin()
    {
        SceneManager.LoadScene("Register");
    }

    public void RegisterUser()
    {

        string username = username_field.GetComponent<TMP_InputField>().text;
        string password = password_field.GetComponent<TMP_InputField>().text;

        if (username.Length < 3 || password.Length < 6)
        {
            errorMsg.enabled = true;
        }
        else
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO users (username, password) VALUES (@username, @password)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    try
                    {
                        command.ExecuteNonQuery();
                        SceneManager.LoadScene("Login");
                    }
                    catch
                    {
                        errorMsg.enabled = true;
                        /*SceneManager.LoadScene("Register");*/
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
