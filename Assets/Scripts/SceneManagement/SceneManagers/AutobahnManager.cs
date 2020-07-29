﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutobahnManager : MonoBehaviour
{
    public static AutobahnManager Instance { get; private set; }

    [SerializeField] private GameObject participantsCar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public GameObject GetParticipantsCar()
    {
        return participantsCar;
    }
}