using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimProperties : MonoBehaviour
{
    [SerializeField] Transform seatPos;
    [SerializeField] Transform enterPos;

    public Transform AnimEnterPosition()
    {
        return enterPos;
    }
    public Transform AnimDrivePosition()
    {
        return seatPos;
    }
}
