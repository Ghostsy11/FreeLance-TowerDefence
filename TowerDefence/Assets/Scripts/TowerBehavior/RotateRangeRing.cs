using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRangeRing : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 25f;

    void Update()
    {

        gameObject.transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);

    }
}
