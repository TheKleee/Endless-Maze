using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Menues:"), SerializeField]
    GameObject[] menues = new GameObject[4]; //Login, Register, (Player, Score, Exit), Score
    [Space]
    [Header("Buttons")]
    [SerializeField] Button play;
    [Space, Header("Error:"), SerializeField]
    TextMeshProUGUI error;
    int prevMenuID = 0;
    

    private void Awake() => DisplayMenu();

    private void Start()
    {
        DBManager.instance.displayError += DisplayError;
        DBManager.instance.postLogin += Login;
        DBManager.instance.postRegister += Register;
    }
    #region Methods
    public bool Login()
    {
        DisplayMenu(2);
        return true;
    }

    public bool Register()
    {
        DisplayMenu(0);
        return true;
    }
    #endregion methods />

    #region Menu Navigation:
    public void Back() => DisplayMenu(prevMenuID);
    public void RegisterMenu() => DisplayMenu(1); //Register -> 1
    public void Leaderboard() => DisplayMenu(3); //Leaderboard -> 3
    #region Display
    public void DisplayMenu(int id = 0)
    {
        int counter = 0;
        foreach (var m in menues)
        {
            if (m.activeSelf)
                counter++;
            m.SetActive(false);
        }
        menues[id].SetActive(true);
        prevMenuID = counter;
    }
    #endregion display />
    #endregion menu navigation />

    #region Submit
    public void RegisterSubmit()
    {
        DBManager.instance.Register();
    }

    public void LoginSubmit()
    {
        DBManager.instance.Login();
    }
    #endregion

    #region Error:
    void DisplayError() => error.text = DBManager.instance.error;
    #endregion error />
}
