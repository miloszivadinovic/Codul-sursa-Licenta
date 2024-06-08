using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitRotation : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(0f, 0f, 30 * Time.deltaTime, Space.Self);
    }
}
