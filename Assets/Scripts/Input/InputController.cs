using UnityEngine;

public class InputController : ElementOf<Application>
{
    [SerializeField] private float sensivity = 1;

    private float _lastMouseHorizontalPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _lastMouseHorizontalPosition = Input.mousePosition.x;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            var currentMouseHorizontalPosition = Input.mousePosition.x;
            var difference = currentMouseHorizontalPosition - _lastMouseHorizontalPosition;
            HandleMouseMovement(difference);
            _lastMouseHorizontalPosition = currentMouseHorizontalPosition;
        }
    }

    /// <summary>
    /// tracks touch drag.
    /// </summary>
    /// <param name="difference"></param>
    private void HandleMouseMovement(float difference)
    {
        Master.Notify(InputNotification._MouseMove, new InputModel(difference * sensivity));
    }
}