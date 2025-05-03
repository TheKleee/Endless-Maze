using UnityEngine;
using UnityEngine.InputSystem;

public class MazeGoal : MonoBehaviour
{
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
    }
}
