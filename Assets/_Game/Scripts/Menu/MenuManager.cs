using UnityEditor.Search;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    [Space, Header("Prefabs:"), SerializeField]
    GameObject playerScore;
    [SerializeField] Transform scoreScroll;
    bool scoreChecked = false;
    List<TMP_InputField> activeFields = new List<TMP_InputField>();
    TMP_InputField[] fields;
    [Space]
    [Header("Submit buttons:"), SerializeField]
    Button[] submitButtons;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void Start()
    {
        fields = new TMP_InputField[] { username[0], password[0], username[1], password[1], confirm };
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
    public void LeaderboardMenu()
    {
        DisplayMenu(3); //Leaderboard -> 3
        if (!scoreChecked)
        {
            DBManager.instance.PlayerDataInit();
            scoreChecked = true; //Ne moramo da cistamo score iz baze svaki put...
        }
        List<PlayerData> pd = DBManager.instance.data;
        if (pd.Count > 0)
            foreach (PlayerData p in pd) 
            {
                PlayerScore ps = Instantiate(playerScore).GetComponent<PlayerScore>();
                ps.transform.SetParent(scoreScroll);
                ps.SetData(p);
            }
    }
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


    #region Menu Controls:
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            activeFields.Clear();
            foreach (var field in fields)
            {
                if (field.gameObject.activeInHierarchy && field.interactable)
                    activeFields.Add(field);
            }

            if (activeFields.Count == 0) return;

            activeFields.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

            GameObject current = EventSystem.current.currentSelectedGameObject;
            int currentIndex = -1;

            for (int i = 0; i < activeFields.Count; i++)
            {
                if (activeFields[i].gameObject == current)
                {
                    currentIndex = i;
                    break;
                }
            }

            int nextIndex = (currentIndex + 1) % activeFields.Count;

            EventSystem.current.SetSelectedGameObject(activeFields[nextIndex].gameObject);
            activeFields[nextIndex].ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            foreach (var btn in submitButtons)
                if (btn.gameObject.activeInHierarchy && btn.interactable)
                {
                    btn.onClick.Invoke();
                    break;
                }
        }
    }
    #endregion menu controls />
}
