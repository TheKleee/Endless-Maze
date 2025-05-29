using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MazeGoal : MonoBehaviour
{
    public TextMeshProUGUI timerUI;

    [Header("Door:"), SerializeField]
    MazeDoor mazeDoor;

    [Header("Key:"), SerializeField]
    MazeKey mazeKey;

    [Header("Game Over:"), SerializeField]
    GameObject gameOverMenu;

    [Header("Colours:"), SerializeField]
    Color[] cols = new Color[2];
    [SerializeField] TextMeshProUGUI winLoseTxt;
    [SerializeField] TextMeshProUGUI timeDescriptionTxt;

    public void SpawnGoal(MazeTile mt, bool key = false)
    {
        Transform goal = Instantiate(key ? mazeKey.transform : mazeDoor.transform);
        goal.name = $"Maze{(key ? "Key" : "Door")}";
        goal.parent = mt.transform;
        goal.localPosition = Vector3.zero;
        
        if(goal.GetComponent<MazeDoor>() != null)
        {
            var door = goal.GetComponent<MazeDoor>();
            door.timerUI = timerUI;
            door.goal = this;
        }
    }

    public void ShowGameOverMenu(float time)
    {
        timerUI.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        bool won = time > 0.0f;
        winLoseTxt.text = won ? "You Won" : "You Lost";
        winLoseTxt.color = won ? cols[0] : cols[1];
        int t = Mathf.CeilToInt(time);
        timeDescriptionTxt.text = won ? $"Time left: { (t > 9 ? t : "0"+t) } second{(t > 1 ? "s": "")}!" : "Time run out!";

        if (won)
            DBManager.instance.winner();
        else
            DBManager.instance.loser();
    }

    public void OkButton() => SceneManager.LoadSceneAsync(0);
}
