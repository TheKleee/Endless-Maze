using UnityEngine;

public class MazeDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null)
            return;

        var p = other.GetComponent<PlayerMovement>();
        if (p.hasKey)
        {
            DBManager.instance.winner();
        }
    }
}
