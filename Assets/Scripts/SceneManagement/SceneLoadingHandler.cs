﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadingHandler : MonoBehaviour
{
    public static SceneLoadingHandler Instance { get; private set; }

    private GameObject _participantsCar;
    private GameObject _seatPosition;
    private bool _isLoadAdditiveModeRunning;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignParticipantsCarAndSeatPosition();

        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.SetObjectToFollow(_participantsCar);
            CameraManager.Instance.SetSeatPosition(_seatPosition);
            SavingManager.Instance.SetParticipantCar(_participantsCar);
        }
        
        /*if (_participantsCar !=null)
        {
            if (SceneManager.GetActiveScene().name != "SceneLoader")
                _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(0);
            else
                _participantsCar.GetComponent<CarWindows>().SetInsideWindowsAlphaChannel(1);
        }*/
    }

    private void Start()
    {
        AssignParticipantsCarAndSeatPosition();
        
        if (CameraManager.Instance != null)
        {
            CameraManager.Instance.SetObjectToFollow(_participantsCar);
            CameraManager.Instance.SetSeatPosition(_seatPosition);
        }
    }

    public void LoadExperimentScenes()
    {
        AssignParticipantsCarAndSeatPosition();

        CameraManager.Instance.FadeOut();
        StartCoroutine(LoadExperimentScenesAsyncAdditive());
    }
    
    public void SceneChange(string targetScene)
    {
        CameraManager.Instance.FadeOut();
        StartCoroutine(LoadScenesAsync(targetScene));
    }

    IEnumerator LoadScenesAsync(string targetScene)
    {
        Debug.Log(targetScene);
        yield return new WaitForSeconds(2);
        Debug.Log("Loading...");
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(operation.progress);

            if (progress >= .9f)
            {
                CameraManager.Instance.FadeOut();
            }
            
            yield return null;
        }
        
        AssignParticipantsCarAndSeatPosition();
        CameraManager.Instance.OnSceneLoaded(true);
    }
    
    public IEnumerator LoadExperimentScenesAsyncAdditive()
    {
        _isLoadAdditiveModeRunning = true;
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        
        // AsyncOperationHandle op1 = Addressables.LoadSceneAsync("MountainRoad");
        AsyncOperation opMountainRoad = SceneManager.LoadSceneAsync("MountainRoad");

        // opMountainRoad.allowSceneActivation = false;
        
        while (!opMountainRoad.isDone)
        {
            yield return null;
        }
        
        // opMountainRoad.allowSceneActivation = true;

        AsyncOperation op2 = SceneManager.LoadSceneAsync("Westbrueck", LoadSceneMode.Additive);

        while (!op2.isDone)
        {
            yield return null;
        }
        
        AsyncOperation op3 = SceneManager.LoadSceneAsync("CountryRoad", LoadSceneMode.Additive);
        
        while (!op3.isDone)
        {
            yield return null;
        }
        
        AsyncOperation op4 = SceneManager.LoadSceneAsync("Autobahn", LoadSceneMode.Additive);
        
        while (!op4.isDone)
        {
            yield return null;
        }

        _participantsCar = MountainRoadManager.Instance.GetParticipantsCar();
        _seatPosition = MountainRoadManager.Instance.GetSeatPosition();
        _isLoadAdditiveModeRunning = false;
        CameraManager.Instance.OnSceneLoaded(false);
    }
    
    private void AssignParticipantsCarAndSeatPosition()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "SceneLoader":
                _participantsCar = SceneLoadingSceneManager.Instance.GetParticipantsCar();
                break;
            case "SeatCalibrationScene":
                _participantsCar = SeatCalibrationManager.Instance.GetParticipantsCar();
                _seatPosition = SeatCalibrationManager.Instance.GetSeatPosition();
                break;
            case "TrainingScene":
                _participantsCar = TrainingHandler.Instance.testEventManager.GetParticipantCar();
                _seatPosition = TrainingHandler.Instance.GetSeatPosition();
                break;
            case "MountainRoad":
                _participantsCar = MountainRoadManager.Instance.GetParticipantsCar();
                _seatPosition = MountainRoadManager.Instance.GetSeatPosition();
                break;
            case "Westbrueck":
                _participantsCar = WestbrueckManager.Instance.GetParticipantsCar();
                _seatPosition = WestbrueckManager.Instance.GetSeatPosition();
                break;
            case "CountryRoad":
                _participantsCar = CountryRoadManager.Instance.GetParticipantsCar();
                _seatPosition = CountryRoadManager.Instance.GetSeatPosition();
                break;
            case "Autobahn":
                _participantsCar = AutobahnManager.Instance.GetParticipantsCar();
                _seatPosition = AutobahnManager.Instance.GetSeatPosition();
                break;
        }
    }

    public GameObject GetParticipantsCar()
    {
        AssignParticipantsCarAndSeatPosition();
        return _participantsCar;
    }
    
    public GameObject GetSeatPosition()
    {
        AssignParticipantsCarAndSeatPosition();
        return _seatPosition;
    }

    public bool GetAdditiveLoadingState()
    {
        return _isLoadAdditiveModeRunning;
    }
}


