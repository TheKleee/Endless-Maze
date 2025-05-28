using UnityEngine;

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
    public int loses { get; set; }
}

public interface IRegisterManager
{
    public delegate bool RegisterPlayer();
    public event RegisterPlayer registerPlayer;
    public event RegisterPlayer postRegister;
    public void Register();
}

public interface ILoginManager
{
    public delegate bool LoginPlayer();
    public event LoginPlayer loginPlayer;
    public event LoginPlayer postLogin;
    public void Login();
}

public class DBManager : MonoBehaviour, IUserManager, IDBError, IScoreManager, IRegisterManager, ILoginManager
{
    #region Singleton
    public static DBManager instance;
    private void Awake()
    {
        instance ??= this;
        DontDestroyOnLoad(gameObject);
        username = password = confirm = "";
    }
    #endregion singleton />

    public string username { get; set; }
    public string password { get; set; }
    public string confirm { get; set; }
    public int wins { get; set; }
    public int loses { get; set; }
    public string error { get; set; }
    public event IDBError.DisplayError displayError;
    public event IRegisterManager.RegisterPlayer registerPlayer;
    public event ILoginManager.LoginPlayer loginPlayer;
    public event IRegisterManager.RegisterPlayer postRegister;
    public event ILoginManager.LoginPlayer postLogin;

    #region Error
    public void Error(string e = "clear")
    {
        error = e == "clear" ? "" : $"{error}{e}\n";
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
        bool login = (bool)loginPlayer?.Invoke();
        if (login) postLogin?.Invoke();
    }
    
    #endregion methods />

}

