using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [Header("Menues:"), SerializeField]
    GameObject[] menues = new GameObject[4]; //Login, Register, (Player, Score, Exit), Score
    [Space]
    [Header("Buttons")]
    [SerializeField] Button play;
    

    private void Awake() =>
        DisplayMenu();

    private void Start()
    {
        DBManager.instance.postLogin += Login;
        DBManager.instance.postRegister += Register;
    }
    public bool Login()
    {
        return true;
    }

    public bool Register()
    {
        return true;
    }

    #region Menu Controller
    public void DisplayMenu(int id = 0)
    {
        foreach (var m in menues)
            m.SetActive(false);
        menues[id].SetActive(true);
    }
    #endregion menu controller />
}
