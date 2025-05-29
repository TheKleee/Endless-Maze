using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public interface IUserManager
{
    public string username { get; set; }
    public string password { get; set; }
    public string confirm { get; set; }

    public (bool, string) CheckPassword();
}

public interface IDBError
{
    public string error { get; set; }
    public delegate void DisplayError();
    public event DisplayError displayError;
    public void Error(string e = "");
    public bool CheckError();
}

public interface IScoreManager
{
    public int wins { get; set; }
    public int losses { get; set; }

    public delegate void Win();
    public event Win win;
    public delegate void Lose();
    public event Lose lose;
}

public interface IRegisterManager
{
    public delegate bool RegisterPlayer();
    public event RegisterPlayer registerPlayer;
    public delegate void PostRegister();
    public event PostRegister postRegister;
    public void Register();
}

public interface ILoginManager
{
    public delegate bool LoginPlayer();
    public event LoginPlayer loginPlayer;
    public delegate void PostLogin();
    public event PostLogin postLogin;
    public void Login();
    public bool loggedIn { get; set; }
}

public interface IPlayerData
{
    public List<PlayerData> data { get; set; }
    public delegate void ReadPlayerData();
    public event ReadPlayerData readPlayerData;
    public void ClearPlayerData();
    public void SetPlayerData(PlayerData pd);
    public void PlayerDataInit();
}

public class DBManager : MonoBehaviour, IUserManager, IDBError, IScoreManager, IRegisterManager, ILoginManager, IPlayerData
{
    #region Singleton
    public static DBManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            username = password = confirm = "";
            data = new List<PlayerData>();
            return;
        }
        Destroy(gameObject);
    }
    #endregion singleton />

    public string username { get; set; }
    public string password { get; set; }
    public string confirm { get; set; }
    public int wins { get; set; }
    public int losses { get; set; }
    public string error { get; set; }
    public List<PlayerData> data { get; set; }
    public bool loggedIn { get; set; }

    public event IDBError.DisplayError displayError;
    public event IRegisterManager.RegisterPlayer registerPlayer;
    public event ILoginManager.LoginPlayer loginPlayer;
    public event IRegisterManager.PostRegister postRegister;
    public event ILoginManager.PostLogin postLogin;
    public event IPlayerData.ReadPlayerData readPlayerData;
    public event IScoreManager.Win win;
    public event IScoreManager.Lose lose;

    #region Error
    public void Error(string e)
    {
        error += e == "" ? "" : $"{e}\n";
        displayError?.Invoke();
    }
    public void Error()
    {
        error = "";
        displayError?.Invoke();
    }
    public bool CheckError() => error.Length > 0;
    #endregion error />

    #region Methods
    public (bool, string) CheckPassword() => (password == confirm, "Passwords do not match");
    public string CheckUsername(bool usernameTaken) => usernameTaken ? "Username already exists" : "";

    public void Register()
    {
        bool register = (bool)registerPlayer?.Invoke();
        if (register) postRegister?.Invoke();
            
    }
    public void Login()
    {
        if(!loggedIn)
        {
            bool login = (bool)loginPlayer?.Invoke();
            if (login) postLogin?.Invoke();
        }
    }
    public void ClearPlayerData() => data.Clear();
    public void SetPlayerData(PlayerData pd) => data.Add(pd);
    public void PlayerDataInit() => readPlayerData?.Invoke();
    #endregion methods />

    public void winner()
    {
        Debug.Log("Pobedio si!");
        //Win logic...
        win?.Invoke();
    }

    public void loser()
    {
        Debug.Log("Izgubio si!");
        //Lose logic
        lose?.Invoke();
    }

}

