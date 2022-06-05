using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class CharacterMovementController : ElementOf<Application>
{
    [SerializeField] private float speed;
    [SerializeField] private float damping;
    [SerializeField] private float accelerationMultiplier;

    [SerializeField] private MinMaxValues horizontalClampValues;


    private float _translateValue;
    private float _acceleration;

    private bool _canMovable;
    private float _horizontal;

    private void OnValidate()
    {
        horizontalClampValues.ClampValues();
    }

    private void OnEnable()
    {
        Master.OnInputNotificationSent += HandleInputNotification;
        Master.OnGameStateNotificationSent += HandleGameStateNotification;
    }


    private void OnDisable()
    {
        Master.OnInputNotificationSent -= HandleInputNotification;
        Master.OnGameStateNotificationSent -= HandleGameStateNotification;
    }

    private void HandleGameStateNotification(string notificationString, Object[] payload)
    {
        _canMovable = notificationString == GameNotification._GamePlay;
    }

    private void HandleInputNotification(string notificationString, Object[] payload)
    {
        if (notificationString != InputNotification._MouseMove) return;

        _horizontal = ((InputModel) payload[0]).Movement;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        var canMovable = _canMovable & Input.GetKey(KeyCode.Mouse0);
        var targetAcceleration = canMovable ? 1f : 0f;
        _horizontal = canMovable ? _horizontal : 0;
        if (_horizontal > 0 && transform.position.x >= horizontalClampValues.maxValue
            || _horizontal < 0 && transform.position.x <= horizontalClampValues.minValue)
            _horizontal = 0;

        _acceleration = Mathf.Lerp(_acceleration, targetAcceleration, Time.fixedDeltaTime * accelerationMultiplier);
        var direction = new Vector3(_horizontal, 0, _acceleration);
        Master.Notify(CharacterNotification._VerticalMovement, new CharacterMovementModel(_acceleration));
        if (direction == Vector3.zero) return;

        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * damping);
        transform.position += transform.forward * _acceleration * Time.fixedDeltaTime * speed;
        transform.position =
            new Vector3(
                Mathf.Clamp(transform.position.x, horizontalClampValues.minValue, horizontalClampValues.maxValue),
                transform.position.y, transform.position.z);
    }


    [Serializable]
    private class MinMaxValues
    {
        public float minValue;
        public float maxValue;

        public void ClampValues()
        {
            minValue = Mathf.Min(minValue, maxValue);
            maxValue = Mathf.Max(minValue, maxValue);
        }
    }
}