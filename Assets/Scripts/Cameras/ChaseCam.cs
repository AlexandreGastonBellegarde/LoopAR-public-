﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ChaseCam : MonoBehaviour
{
    private GameObject _objectToFollow;
    [Range(0f, 10f)] public float damping;

    private void LateUpdate()
    {
        if (CameraManager.Instance.GetObjectToFollow() == null)
        {
            Debug.Log("<color=red>Error: </color>Object to follow not found!");
            return;
        }

        _objectToFollow = CameraManager.Instance.GetObjectToFollow();

        // todo remove
        this.transform.position = _objectToFollow.GetComponent<CarController>().GetSeatPosition().transform.position;
        
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _objectToFollow.transform.rotation,
            Time.deltaTime * damping);
    }
}