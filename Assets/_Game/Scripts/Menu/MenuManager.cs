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
    [Header("Back buttons:"), SerializeField]
    Button[] backButtons;

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

    public void Login()
    {
        DBManager.instance.loggedIn = true;
        DisplayMenu(2);
    }
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
            scoreChecked = true; //Ne moramo da cistamo score iz baze svaki put...
            DBManager.instance.PlayerDataInit();
        
            List<PlayerData> pd = DBManager.instance.data;
            List<PlayerScore> best = new List<PlayerScore>();
            if (pd.Count > 0)
                foreach (PlayerData p in pd) 
                {
                    PlayerScore ps = Instantiate(playerScore).GetComponent<PlayerScore>();
                    ps.transform.SetParent(scoreScroll, false);
                    ps.SetData(p);
                    ps.GetPercentage(p);
                    best.Add(ps);
                }

            //Sortiranje
            best.Sort((a, b) => b.winPercentage.CompareTo(a.winPercentage));
            for (int i = 0; i < best.Count; i++)
                best[i].transform.SetSiblingIndex(i);
        }
    }
    #region Display
    public void DisplayMenu(int id = 0)
    {
        DBManager.instance.Error();
        if (DBManager.instance.loggedIn)
            if (id == 0 || id == 1)
                id = 2;

        if (!DBManager.instance.loggedIn)
        {
            DisplayError();
            ClearFields();
        }
        for (int i = 0; i < menues.Length; i++)
        {
            if (menues[i].activeSelf)
                prevMenuID = i;
            menues[i].SetActive(false);
        }
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
            foreach (var btn in submitButtons)
                if (btn.gameObject.activeInHierarchy && btn.interactable)
                {
                    btn.onClick.Invoke();
                    break;
                }

        if (Input.GetKeyUp(KeyCode.Escape))
            foreach (var btn in backButtons)
                if (btn.gameObject.activeInHierarchy && btn.interactable)
                {
                    btn.onClick.Invoke();
                    break;
                }
    }
    #endregion menu controls />
}
