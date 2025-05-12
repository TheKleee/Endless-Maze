using UnityEngine;
using MEC;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Resources;

public class CameraMovement : MonoBehaviour
{
    [Header("Target"), SerializeField]
    Transform target;

    Vector3 startPos;

    [Space, SerializeField]
    float speed = 10;

    private void Start() => startPos = transform.localPosition;

    private void FixedUpdate()
    {
        transform.LookAt(target.position);
    }

    private void OnCollisionExit(Collision collision)
    {
        Timing.RunCoroutine(_MoveCamBack().CancelWith(gameObject));
    }

    IEnumerator<float> _MoveCamBack()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        while(dist > 0.025f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, Time.deltaTime * speed);
            yield return Timing.WaitForSeconds(Time.deltaTime);
        }
        transform.localPosition = startPos;
    }
}
