using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BoidSettings
{
    [SerializeField, Range(1, 50)] int cohesionRadius = 20, separationRadius = 20, speed = 5, maxDistance = 20;
    [SerializeField, Range(0, 100)] float cohesionCoeff = 1, separationCoeff = 1, alignementCoeff = 1;

    [SerializeField, Range(1, 100)] int maxCloseBoids = 20;
    [SerializeField] LayerMask boidLayer; //init struct ?

    [SerializeField, Header("Timer"), Range(0.1f, 5)] float timerStartRange = 0;
    [SerializeField, Header("Timer"), Range(0.1f, 5)] float timerUpdate = 0;

    public int CohesionRadius => cohesionRadius;
    public int SeparationRadius => separationRadius;
    public int Speed => speed;
    public int MaxDistance => maxDistance;
    public float CohesionCoeff => cohesionCoeff;
    public float SeparationCoeff => separationCoeff;
    public float AlignementCoeff => alignementCoeff;

    public int MaxCloseBoids => maxCloseBoids;
    public int BoidLayer => boidLayer;

    public float TimerStartRange => timerStartRange;
    public float TimerUpdate => timerUpdate;
}
