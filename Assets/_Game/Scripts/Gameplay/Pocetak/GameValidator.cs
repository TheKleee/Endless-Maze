using UnityEngine;

public class GameValidator : MonoBehaviour
{
    #region Singleton
    public static GameValidator instance;
    void Awake() => instance = this;
    #endregion singleton />

    public bool gameOver = false;
    public bool gameStarted = false;
}
