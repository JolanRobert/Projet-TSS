using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class TriggerPattern : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            Boss.OnTriggerAttack?.Invoke(player);
        }
    }
}