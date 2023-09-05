using System;
using Pong.Settings;
using UnityEngine;

namespace Pong.Input
{
    public class InputFactory : MonoBehaviour
    {
        [SerializeField] private MouseInput _mouseInput;
        [SerializeField] private KeyBoardInput _keyBoardInput;

        private BasePlayerInput _input;

        public IReversibleInputService GetInput(InputSettings inputSettings)
        {
            _input = inputSettings.InputType switch
            {
                InputType.Keyboard => _input is KeyBoardInput ? _input : Instantiate(_keyBoardInput),
                InputType.Mouse => _input is MouseInput ? _input : Instantiate(_mouseInput),
                _ => throw new ArgumentOutOfRangeException()
            };
            _input.Sensitivity = inputSettings.Sensitivity;
            _input.IsReverse = false;
            return _input;
        }
    }
}