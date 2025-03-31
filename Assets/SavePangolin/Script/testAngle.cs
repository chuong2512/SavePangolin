using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAngle : MonoBehaviour
{
    public Transform targetObj;
    public float angle = 0;

    void Update()
    {
        //angle = (Vector2.Angle(targetObj.position - transform.position, transform.up));

        //Debug.LogError(Quaternion.LookRotation(targetObj.position - transform.position).eulerAngles);

        Vector3 targetDir = targetObj.position - transform.position;
        angle = Vector2.SignedAngle(targetDir, transform.up);
        //float angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);
        if (angle < -5.0F)
            print("turn left");
        else if (angle > 5.0F)
            print("turn right");
        else
            print("forward");
    }
}
