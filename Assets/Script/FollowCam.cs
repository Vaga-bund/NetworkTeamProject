using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float dist = 10.0f;
    public float height = 3.0f;
    public float dampTrace = 20.0f;
    public Vector3 offset;
    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = target.position + offset;
        if (target == null)
            return;

        tr.position = Vector3.Lerp(tr.position,
                                    target.position - (target.forward * dist) + (Vector3.up * height),
                                    Time.deltaTime * dampTrace);
        tr.LookAt(target.position);
    }
}
