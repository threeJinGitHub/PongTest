using UnityEngine;

namespace Pong.Input
{
    using Input = UnityEngine.Input;
    public class MouseInput : BasePlayerInput
    {
        private float _lastMousePosY;
        private Camera _cameraMain;

        private void Start()
        {
            _lastMousePosY = Input.mousePosition.y;
            _cameraMain = Camera.main;
        }

        private float ScreenToWorldDistanceRatio => _cameraMain.orthographicSize * 2 / Screen.height;

        private void Update()
        {
            var curMousePos = Input.mousePosition.y;
            var direction = (_lastMousePosY - curMousePos) * Sensitivity * ScreenToWorldDistanceRatio;
            if (_lastMousePosY > curMousePos)
            {
                if (IsReverse)
                {
                    OnMoveUp.Execute(direction);
                }
                else
                {
                    OnMoveDown.Execute(direction);
                }
            }
            else
            {
                if (IsReverse)
                {
                    OnMoveDown.Execute(-direction);
                }
                else
                {
                    OnMoveUp.Execute(-direction);
                }
            }

            _lastMousePosY = curMousePos;
        }
    }
}
