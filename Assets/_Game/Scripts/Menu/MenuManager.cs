using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using System.Collections.Generic;

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
    [Space, Header("Credentials:")]
    [SerializeField] TMP_InputField[] username = new TMP_InputField[2];
    [SerializeField] TMP_InputField[] password = new TMP_InputField[2];
    [SerializeField] TMP_InputField confirm;

    private void Start()
    {
        DisplayMenu();
        DBManager.instance.displayError += DisplayError;
        DBManager.instance.postLogin += Login;
        DBManager.instance.postRegister += Register;
    }
    #region Methods

    public void SetUsername(int id = 0) => DBManager.instance.username = username[id].text;
    public void SetPassword(int id = 0) => DBManager.instance.password = password[id].text;
    public void SetConfirm() => DBManager.instance.confirm = confirm.text; //Confirm password

    public void Login() => DisplayMenu(2);
    public void Register() => DisplayMenu(0);
    #endregion methods />

    #region Menu Navigation:
    public void Back() => DisplayMenu(prevMenuID);
    public void RegisterMenu() => DisplayMenu(1); //Register -> 1
    public void Leaderboard() => DisplayMenu(3); //Leaderboard -> 3
    #region Display
    public void DisplayMenu(int id = 0)
    {
        DBManager.instance.Error();
        DisplayError();
        ClearFields();
        for (int i = 0; i < menues.Length; i++)
        {
            if (menues[i].activeSelf)
                prevMenuID = i;
            menues[i].SetActive(false);
        }
        //Debug.Log($"Previous menu:{prevMenuID}\nCurrent menu: {id}");
        menues[id].SetActive(true);
    }
    public void ClearFields()
    {
        List<TMP_InputField> fields = new List<TMP_InputField>();
        fields.AddRange(username);
        fields.AddRange(password);
        fields.Add(confirm);
        foreach (var f in fields)
            f.text = "";
        DBManager.instance.username = DBManager.instance.password = DBManager.instance.confirm = "";
    }
    #endregion display />
    #endregion menu navigation />

    #region Submit
    public void RegisterSubmit() => DBManager.instance.Register();
    public void LoginSubmit() => DBManager.instance.Login();
    #endregion

    #region Error:
    void DisplayError() => error.text = DBManager.instance.error;
    #endregion error />
}
