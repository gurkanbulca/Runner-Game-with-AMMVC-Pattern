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
            if (difference == 0) return;

            HandleMouseMovement(difference);
            _lastMouseHorizontalPosition = currentMouseHorizontalPosition;
        }
    }

    private void HandleMouseMovement(float difference)
    {
        Master.Notify(InputNotification._MouseMove, new InputModel(difference * sensivity));
    }
}