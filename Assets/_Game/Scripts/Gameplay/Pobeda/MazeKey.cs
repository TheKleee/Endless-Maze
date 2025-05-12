using UnityEngine;

public class MazeKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() == null)
            return;

        var p = other.GetComponent<PlayerMovement>();
        if (!p.hasKey)
            p.CollectKey(transform.GetChild(0));
    }
}
    