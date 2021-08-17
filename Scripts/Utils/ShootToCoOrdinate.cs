using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToCoOrdinate : MonoBehaviour
{
    public GameObject shellPrefab; // Add Shell game object in the Inspector.
    public GameObject shellSpawnPos; // Add Cube game object in the Inspector.
    public GameObject target; // Add Enemy game object in the Inspector.
    public GameObject parent; // Add Tank game object in the Inspector.
    float speed = 15;
    float turnSpeed = 2;

    const float gravity = 9.81f;
    public bool ShootLowAngle;

    void Fire()
    {
        GameObject shell = Instantiate(shellPrefab, shellSpawnPos.transform.position, shellSpawnPos.transform.rotation);
        shell.GetComponent<Rigidbody>().velocity = speed * this.transform.forward; // Use 'forward' because it's the Z axis you want to shoot along.
    }

    void Update()
    {
        Vector3 direction = (target.transform.position - parent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            float? angle = RotateTurret();

            if (angle != null && Vector3.Angle(direction, parent.transform.forward) < 10) // When the angle is less than 10 degrees...
                Fire();
        }
    }

    float? RotateTurret()
    {
        float? angle = CalculateAngle(); // Set to false for upper range of angles, true for lower range.

        if (angle != null) // If we did actually get an angle...
        {
            this.transform.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f); // ... rotate the turret using EulerAngles because they allow you to set angles around x, y & z.
        }
        return angle;
    }

    float? CalculateAngle()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        float y = targetDir.y;
        targetDir.y = 0f;
        float x = targetDir.magnitude;
        float sSqr = speed * speed;
        float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = sSqr + root;
            float lowAngle = sSqr - root;

            if (ShootLowAngle)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);

        }
        else
            return null;
    }
}
