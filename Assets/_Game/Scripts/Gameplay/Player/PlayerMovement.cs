using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main:")]
    [SerializeField] Transform body;
    [SerializeField] Transform target;
    [Space]

    [Header("Camera:"), SerializeField]
    Camera cam;

    [Header("Animator:"), SerializeField]
    Animator anim;

    Rigidbody rb;
    public bool hasKey { get; private set; }
    bool startMoving;
    [SerializeField] float maxSpeed = 5.0f;
    float curSpeed;
    
    [Header("Hand:"), SerializeField]
    Transform pickUpHand;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        //Ideja: Trci non stop pravo, ali kada skrece usporava malo, ako naglo skrece, ili puno skreces usporava vise.
        Move();
    }

    #region Movement
    void Move()
    {
        //rb.AddForce(body.forward * maxSpeed * Time.deltaTime, ForceMode.Impulse);
        transform.Translate(body.forward * maxSpeed * Time.deltaTime);
        //Body gelad u Target!
        var lookPos = target.position - body.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        body.rotation = Quaternion.Slerp(body.rotation, rotation, maxSpeed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0)
            Timing.RunCoroutine(_Move().CancelWith(gameObject));
    }

    IEnumerator<float> _Move()
    {
        yield return Timing.WaitForSeconds(1);
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
