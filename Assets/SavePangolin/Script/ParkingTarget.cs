using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParkingColor { Red, Yellow, Blue, Green}
public class ParkingTarget : MonoBehaviour
{
    public ParkingColor parkingColor;

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
