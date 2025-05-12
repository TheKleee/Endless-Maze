using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement:")]
    [SerializeField] Transform R;
    [SerializeField] Transform L;
    [SerializeField] Transform F;
    [SerializeField] Transform B;

    [Header("Camera:"), SerializeField]
    Camera cam;

    [Header("Animator:"), SerializeField]
    Animator anim;

    public bool hasKey { get; private set; }
    bool startMoving;
    [SerializeField, Range(0.2f, 5.0f)] float startSpeed = 1f;
    [SerializeField] float maxSpeed = 15.0f;

    [Header("Speed:"), SerializeField]
    float curSpeed = .0f;

    [Header("Hand:"), SerializeField]
    Transform pickUpHand;

    void Awake()
    {
        curSpeed = startSpeed;
        cam = cam == null ? GetComponentInChildren<Camera>() : cam;
        anim = anim == null ? GetComponentInChildren<Animator>() : anim;
    }

    void FixedUpdate()
    {
        if (!startMoving)
        {
            if (Input.anyKey)
                startMoving = true;

            return;
        }

        Move();
        cam.fieldOfView = PercentageLock.instance.ReadPercentageLock(60, 90);
    }

    #region Movement
    void Move()
    {
        if (Input.GetAxis("Vertical") == 0)
        {
            DecreaseSpeed();
            PercentageLock.instance.SetPercentageLock(curSpeed, startSpeed, maxSpeed);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(F.localPosition * curSpeed * Time.fixedDeltaTime);
            PercentageLock.instance.SetPercentageLock(curSpeed, startSpeed, maxSpeed);
            if (curSpeed < maxSpeed)
            {
                curSpeed += 0.1f;
            }
            else
            {
                curSpeed = maxSpeed;
            }
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            transform.Translate(B.localPosition * startSpeed * Time.fixedDeltaTime);
            DecreaseSpeed();
            PercentageLock.instance.SetPercentageLock(curSpeed, startSpeed, maxSpeed);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            var lookPos = R.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, startSpeed * Time.deltaTime);
        }


        if (Input.GetAxis("Horizontal") < 0)
        {
            var lookPos = L.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, startSpeed * Time.deltaTime);
        }
    }

    void DecreaseSpeed()
    {
        if (curSpeed > startSpeed)
            curSpeed -= 0.1f;
        else
            curSpeed = startSpeed;
    }
    #endregion movement />

    #region Key
    public void CollectKey(Transform key)
    {
        hasKey = true;
        key.parent = pickUpHand;
        key.localPosition = Vector3.zero;
        anim.Play("PickUp");
    }
    #endregion key />
}