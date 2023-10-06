using System.Collections.Generic;
using UnityEngine;

public class Utility {
    public static float MoveTowardsTarget(GameObject thisObject, GameObject target, float movementSpeed, float rotationSpeed = 35) // 0.005f)
    {
        Vector3 dif;
        Quaternion targetRot = Quaternion.Euler(0, 0, GetRotationFromObject1To2(thisObject, target, out dif));
        //thisObject.transform.rotation = Quaternion.Slerp(thisObject.transform.rotation, targetRot, rotationSpeed);
        float step = rotationSpeed * Time.deltaTime;
        thisObject.transform.rotation = Quaternion.RotateTowards(thisObject.transform.rotation, targetRot, step);
        thisObject.transform.rotation = Quaternion.Euler(
            0,
            0,
            thisObject.transform.rotation.eulerAngles.z
        );

        Rigidbody2D rigidbody = thisObject.GetComponent<Rigidbody2D>();
        rigidbody.velocity = thisObject.transform.right * movementSpeed;
        return dif.magnitude;
    }

    public static float GetRotationFromObject1To2(GameObject thisObject, GameObject target, out Vector3 dif)
    {
        dif = target.transform.position - thisObject.transform.position;
        Vector3 direction = dif.normalized;
        Quaternion targetRot = Quaternion.FromToRotation(Vector3.right, direction);
        return targetRot.eulerAngles.z;
    }
}