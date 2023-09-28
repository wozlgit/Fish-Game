using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Update()
    {
        transform.position = player.transform.position;
        //transform.position = new(player.transform.position.x, 0, player.transform.position.z);
    }
}
