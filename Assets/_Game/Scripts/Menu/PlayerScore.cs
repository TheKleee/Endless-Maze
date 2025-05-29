using UnityEngine;
using TMPro;
public class PlayerScore : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI wins;
    [SerializeField] TextMeshProUGUI losses;
    [SerializeField] TextMeshProUGUI percentage; // wins / (wins+loses) * 100

    #region Methods:
    void CalculatePercentage(float wins, int losses) => percentage.text = losses > 0 || wins > 0 ? ((int)(wins / (wins + losses) * 100)).ToString() : "N/A";
    public void SetData(PlayerData player)
    {
        username.text = player.username;
        wins.text = player.wins.ToString();
        losses.text = player.losses.ToString();
        CalculatePercentage(player.wins, player.losses);
    }
    #endregion methods />
}
