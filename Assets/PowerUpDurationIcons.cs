using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDurationIcons : MonoBehaviour
{
    [SerializeField]
    PowerUpManager powerUpManager;

    [Header("")]
    [SerializeField]
    GameObject GameObjectShieldDurationIcon;

    [SerializeField]
    GameObject GameObjectHighSpeedDurationIcon;

    [SerializeField]
    GameObject GameObjectLaserDurationIcon;

    [SerializeField]
    GameObject GameObjectSuperAmmoDurationIcon;


    
    void Start()
    {
        
    }

    void Update()
    {
        if (PowerUpManager.LaserActivated())
        {
            Debug.Log("Laser aktywowany!");
            Instantiate(GameObjectShieldDurationIcon, GameObjectShieldDurationIcon.transform.position, Quaternion.identity, this.transform);

        }
    }
}
