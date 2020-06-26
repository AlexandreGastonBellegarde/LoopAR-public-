﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TrafficEventTrigger : MonoBehaviour
{
    /*[Space][Header("Start Event Delay")]
    [Tooltip("0 to 15 seconds")] [Range(0,15)] [SerializeField] */
    private float _startEventDelay = 0;

    [Space][Header("Event state")]
    [SerializeField] private bool activateEvent;
    
    private CriticalEventController _eventController;
    private GameObject _targetVehicle;
    private GameObject _currentTarget;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _currentTarget)
            return;

        _currentTarget = other.gameObject;
        
        
        if (other.gameObject == _targetVehicle)
        {
            if (activateEvent)
            {
                _startEventDelay = _eventController.GetEventStartDelay();
            }
            _targetVehicle.gameObject.GetComponentInChildren<HUD_Advance>().DriverAlert();
            // Debug.Log("Informed HUD " + Time.time);
            StartCoroutine(StartDelayedEvent(_startEventDelay));
        }
    }

    IEnumerator StartDelayedEvent(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        _eventController.Triggered(activateEvent);
        // Debug.Log("Event started " + Time.time);
    }

    public void TargetVehicle(GameObject vehicle)
    {
        _targetVehicle = vehicle;
    }

    public void SetController(CriticalEventController eventController)
    {
        _eventController = eventController;
    }
}
