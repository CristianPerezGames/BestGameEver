using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        float lerpX = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime);
        transform.position = new Vector3(lerpX, transform.position.y, transform.position.z);
    }
}
