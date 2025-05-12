using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerMovement : MonoBehaviour
{
    [Header("Camera:"), SerializeField]
    Camera cam;

    [Header("Animator:"), SerializeField]
    Animator anim;

    Rigidbody rb;
    public bool hasKey { get; private set; }
    bool startMoving;
    [SerializeField, Range(0.2f, 5.0f)] float startSpeed = 1f;
    [SerializeField] float maxSpeed = 15.0f;
    float curSpeed = .0f;
    
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

        Move();
    }

    #region Movement
    void Move()
    {

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
