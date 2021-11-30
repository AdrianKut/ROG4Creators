using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private GameObject player;
    void OnEnable() => player = GameObject.FindGameObjectWithTag("Player");
    void LateUpdate() => transform.position = new Vector3(transform.position.x, player.transform.position.y - 0.25f, transform.position.z); 
}
