﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSavingTrigger : MonoBehaviour
{
    private string _oldScene;
    
    public enum Scenes
    {
        MountainRoad,
        Westbrueck, 
        CountryRoad, 
        Autobahn,
        TrainingScene
    }
    
    public Scenes sceneToSaveDataOf = Scenes.MountainRoad;
    
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        SavingManager.Instance.SaveDataAndStartRecordingAgain(sceneToSaveDataOf.ToString());
    }
}
