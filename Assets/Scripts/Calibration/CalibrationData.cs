﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class CalibrationData
{
    // public int ParticipationNumber;
    public String ParticipantUuid;
    public bool VRmode;
    public string SteeringInputDevice;
    public Vector3 EyeValidationError;
    public Vector3 SeatCalibrationOffset;

    public bool TrainingSuccessState;
    public int NumberOfTrainingTrials;
    
    public double ExperimentDuration;
    public double ApplicationDuration;

    public float AverageExperimentFPS;
}
