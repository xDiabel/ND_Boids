using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Boid : MonoBehaviour
{
    event Action OnBehaviour = null;
    event Action OnUpdate = null;

    #region F/P

    [SerializeField] Vector3    cohesion = Vector3.zero,    separation = Vector3.zero,
                                alignement = Vector3.zero,  velocity = Vector3.zero;

    BoidSettings settings = null;
    Boid[] closeBoids = null;
    int nbCloseBoids = 0;
    int separationCount = 0;

    Transform anchor = null;

    public Vector3 Velocity => velocity;
    public Vector3 Separation { get => separation; set => separation = value; }
    public Vector3 Position => transform.position;

    bool IsValid => anchor != null && settings != null;

    #endregion

    #region UnityMethods

    private void Awake()
    {
        InitBehaviour();
    }

    private void Start()
    {
        LaunchBoid();
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void OnDestroy()
    {
        OnBehaviour = null;
        OnUpdate = null;
    }

    #endregion

    void InitBehaviour()
    {
        OnBehaviour += () =>
        {
            closeBoids = GetCloseBoids();
            ClampNbBoids();
            ResetBoid();

            if (nbCloseBoids <= 0) return;

            SetInitPosition();

            SetCohesion();
            SetSeparation();
            SetAlignement();

            SetVelocity();
        };

        OnUpdate += UpdatePosition;
    }

    void LaunchBoid()
    {
        InvokeRepeating("BoidBehaviour", Random.value * settings.TimerStartRange, settings.TimerUpdate);
    }

    #region Behaviour

    Boid[] GetCloseBoids()
    {
        return Physics.OverlapSphere(Position, settings.CohesionRadius).Select(b => b.GetComponent<Boid>()).ToArray();
    }

    void ClampNbBoids()
    {
        nbCloseBoids = (closeBoids == null) ? 0 : closeBoids.Length;
        nbCloseBoids = (nbCloseBoids > settings.MaxCloseBoids) ? settings.MaxCloseBoids : nbCloseBoids;
    }

    void ResetBoid()
    {
        cohesion = Vector3.zero;
        separation = Vector3.zero;
        alignement = Vector3.zero;
        velocity = Vector3.zero;
        separationCount = 0;
    }

    void SetInitPosition()
    {
        closeBoids.ToList().ForEach(b =>
       {
           cohesion += b.Position;
           alignement += b.Velocity;

           Vector3 _separationVector = Position - b.Position;
           float _svSqrMagnitude = _separationVector.sqrMagnitude;

           if (_svSqrMagnitude > 0 && _svSqrMagnitude < Mathf.Pow(settings.SeparationRadius, 2))
           {
               separation += _separationVector / _svSqrMagnitude;
               separationCount++;
           }
       });

    }

    void SetCohesion()
    {
        cohesion /= nbCloseBoids;
        cohesion = Vector3.ClampMagnitude(cohesion - Position, settings.Speed);
        cohesion *= settings.CohesionCoeff;
    }

    void SetSeparation()
    {
        if (separationCount <= 0) return;
        separation /= separationCount;
        separation = Vector3.ClampMagnitude(separation, settings.Speed);
        separation *= settings.SeparationCoeff;
    }

    void SetAlignement()
    {
        alignement /= nbCloseBoids;
        alignement = Vector3.ClampMagnitude(alignement, settings.Speed);
        alignement *= settings.AlignementCoeff;
    }

    void SetVelocity()
    {
        velocity = Vector3.ClampMagnitude(cohesion + separation + alignement, settings.Speed);
    }

    #endregion

    #region Update

    //Update position
    void UpdatePosition()
    {
        if (!IsValid) return;

        float _maxDistance = Vector3.Distance(Position, anchor.position);
        bool _needNormalizeVelocity = _maxDistance > settings.MaxDistance;
        if(_needNormalizeVelocity)
        {
            velocity += (anchor.position - Position) / settings.MaxDistance;
        }

        transform.position += velocity * Time.deltaTime;
    }

    void BoidBehaviour()
    {
        OnBehaviour?.Invoke();
    }

    #endregion

    public void SetSettings(BoidSettings _settings, Transform _anchor)
    {
        settings = _settings;

        anchor = _anchor;
    }

}
