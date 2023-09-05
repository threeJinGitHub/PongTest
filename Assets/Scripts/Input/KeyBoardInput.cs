using UnityEngine;

namespace Pong.Input
{
    using Input = UnityEngine.Input;
    public class KeyBoardInput : BasePlayerInput
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (IsReverse)
                {
                    OnMoveDown.Execute(Sensitivity * Time.deltaTime);
                }
                else
                {
                    OnMoveUp.Execute(Sensitivity * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (IsReverse)
                {
                    OnMoveUp.Execute(Sensitivity * Time.deltaTime);
                }
                else
                {
                    OnMoveDown.Execute(Sensitivity * Time.deltaTime);
                }
            }
        }
    }
}