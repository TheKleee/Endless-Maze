using UnityEngine;

public class PercentageLock : MonoBehaviour
{
    #region Singleton
    public static PercentageLock instance;
    void Awake() => instance = this;
    #endregion singleton />
    [SerializeField]
    float curPercentage = 0.0f;

    public void SetPercentageLock(float curValue, float startValue = .0f, float endValue = 1.0f)
    {
        float range = endValue - startValue;
        curValue = curValue - startValue;
        curPercentage = curValue / range;
    }

    public float ReadPercentageLock(float startVlue = .0f, float endValue = 1.0f)
    {
        float range = endValue - startVlue;
        return startVlue + curPercentage * range;
    }
}
