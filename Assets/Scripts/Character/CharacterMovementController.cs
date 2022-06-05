using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class CharacterMovementController : ElementOf<Application>
{
    [SerializeField] private float speed;
    [SerializeField] private float damping = 1;
    [SerializeField] private float accelerationMultiplier;

    [SerializeField] private MinMaxValues horizontalClampValues;
    [SerializeField] private MinMaxValues translateClampValues;
    [SerializeField] private MinMaxValues rotationAngles;


    private float _translateValue;
    private float _acceleration;

    private bool _canMovable;

    private void OnValidate()
    {
        horizontalClampValues.ClampValues();
        translateClampValues.ClampValues();
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

        var movement = ((InputModel) payload[0]).movement;
        var position = transform.position;
        var horizontal = position.x + movement;
        horizontal = Mathf.Clamp(horizontal, horizontalClampValues.minValue, horizontalClampValues.maxValue);
        _translateValue += horizontal - position.x;
    }

    private void FixedUpdate()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();
    }

    private void HandleVerticalMovement()
    {
        var canMovable = _canMovable & Input.GetKey(KeyCode.Mouse0);
        var targetAcceleration = canMovable ? 1f : 0f;
        _acceleration = Mathf.Lerp(_acceleration, targetAcceleration, Time.fixedDeltaTime * accelerationMultiplier);
        transform.Translate(0, 0, _acceleration * speed);
        Master.Notify(CharacterNotification._VerticalMovement, new CharacterMovementModel(_acceleration));
    }

    private void HandleHorizontalMovement()
    {
        if (!_canMovable) return;
        _translateValue = Mathf.Clamp(_translateValue, translateClampValues.minValue, translateClampValues.maxValue);
        RotateCharacterTowardDirection();
        if (_translateValue == 0) return;
        transform.Translate(_translateValue, 0, 0);
        _translateValue = 0;
    }

    private void RotateCharacterTowardDirection()
    {
        float angle;
        if (_translateValue < 0)
        {
            var normalizedTranslate = _translateValue / translateClampValues.minValue;
            angle = Mathf.Lerp(0, rotationAngles.minValue, normalizedTranslate);
        }
        else
        {
            var normalizedTranslate = _translateValue / translateClampValues.maxValue;
            angle = Mathf.Lerp(0, rotationAngles.maxValue, normalizedTranslate);
        }

        var desiredAngle = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredAngle, Time.fixedDeltaTime * damping);
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