using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    [Header("Data:")]
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI wins;
    [SerializeField] TextMeshProUGUI losses;
    [SerializeField] TextMeshProUGUI percentage; // wins / (wins+loses) * 100
    [SerializeField] TextMeshProUGUI lastMatch;
    public int winPercentage { get; set; }

    [SerializeField] Color[] perCol = new Color[2];
    [SerializeField] Image perColImg;

    #region Methods:
    void CalculatePercentage(float wins, int losses) => percentage.text = losses > 0 || wins > 0 ? ((int)(wins / (wins + losses) * 100)).ToString() : "N/A";
    public void SetData(PlayerData player)
    {
        username.text = player.username;
        wins.text = player.wins.ToString();
        losses.text = player.losses.ToString();
        CalculatePercentage(player.wins, player.losses);
        lastMatch.text = player.lastMatch.ToString("dd.MM.yyyy");
    }
    public void GetPercentage(PlayerData p)
    {
        winPercentage = 0;
        if (p.wins > 0 || p.losses > 0)
            winPercentage = (int)((float)p.wins / (p.wins + p.losses) * 100);
        SetPercentageColor();
    }
    void SetPercentageColor()
    {
        float colPer = (float)winPercentage / 100;
        perColImg.color = new Color()
        {
            r = Mathf.Lerp(perCol[1].r, perCol[0].r, colPer),
            g = Mathf.Lerp(perCol[1].g, perCol[0].g, colPer),
            b = Mathf.Lerp(perCol[1].b, perCol[0].b, colPer),
            a = 1
        };
    }
    #endregion methods />
}
