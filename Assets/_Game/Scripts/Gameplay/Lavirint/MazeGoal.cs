using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MazeGoal : MonoBehaviour
{

    public TextMeshProUGUI timerUI;

    [Header("Door:"), SerializeField]
    MazeDoor mazeDoor;

    [Header("Key:"), SerializeField]
    MazeKey mazeKey;

    public void SpawnGoal(MazeTile mt, bool key = false)
    {
        Transform goal = Instantiate(key ? mazeKey.transform : mazeDoor.transform);
        goal.name = $"Maze{(key ? "Key" : "Door")}";
        goal.parent = mt.transform;
        goal.localPosition = Vector3.zero;
        
        if(goal.GetComponent<MazeDoor>() != null)
        {
            goal.GetComponent<MazeDoor>().timerUI=timerUI;
        }
    }
}
