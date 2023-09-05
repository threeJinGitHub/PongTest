using System;
using Pong.Player.Ai;
using UnityEngine;

namespace Pong.Settings
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + FileName, fileName = FileName)]
    public sealed class GameSettings : ScriptableObject
    {
        private const string FileName = nameof(GameSettings);

        [Header("Game settings"), Space] [Header("Ball")]
        public BallSettings BallSettings;

        [Space] [Header("AI")] public AiSettings AiSettings;
        [Space] [Header("Boosters")] public BoostersSetting BoostersSetting;
        [Space] [Header("Match")] public MatchSettings MatchSettings;
        [Space] [Header("Boar")] public BoardSettings BoardSettings;
        [Space] [Header("Input")] public InputSettings InputSettings;

        public readonly GameFiledSettings GameFiledSettings = new();
    }

    [Serializable]
    public class InputSettings
    {
        [SerializeField] private InputType _inputType;
        [SerializeField, Range(0, 10)] private float _sensitivity;

        public InputType InputType => _inputType;
        public float Sensitivity => _sensitivity;
    }

    public enum InputType
    {
        Keyboard,
        Mouse
    }

    [Serializable]
    public class BallSettings
    {
        [SerializeField, Range(0, 180)] private float _maxAngleOfReboundFromBoard;
        [SerializeField, Range(0, 99)] private float _initialSpeed;
        [SerializeField, Range(0, 10)] private float _additionToSpeed;

        public float MaxAngleOfReboundFromBoard => _maxAngleOfReboundFromBoard;
        public float InitialSpeed => _initialSpeed;
        public float AdditionToSpeed => _additionToSpeed;
    }

    [Serializable]
    public class AiSettings
    {
        [SerializeField] private AILevel _aiLevel;

        public AILevel AiLevel => _aiLevel;

        public (int wallCollisionCheck, float boardPositionAccuracy, float dragSpeed, float randomness) GetAIParams() =>
            _aiLevel switch
            {
                AILevel.Easy => (1, 1.7f, .3f, .7f),
                AILevel.Medium => (2, .9f, .4f, .4f),
                AILevel.Hard => (-1, .1f, .5f, .1f),
                _ => throw new ArgumentOutOfRangeException()
            };
    }

    [Serializable]
    public class BoostersSetting
    {
        [SerializeField, Range(1, 99)] private float _spawnInterval;
        [SerializeField, Range(1f, 2)] private float _boardIncreaseSizeValue;
        [SerializeField, Range(.2f, 1)] private float _boardDecreaseSizeValue;

        public float SpawnInterval => _spawnInterval;
        public float BoardIncreaseSizeValue => _boardIncreaseSizeValue;
        public float BoardDecreaseSizeValue => _boardDecreaseSizeValue;
    }

    [Serializable]
    public class MatchSettings
    {
        [SerializeField, Min(0)] private int _pointsToWin;
        [SerializeField, Min(0)] private int _matchDurationInSec;

        public int PointsToWin => _pointsToWin;
        public int MatchDurationInSec => _matchDurationInSec;
    }

    [Serializable]
    public class BoardSettings
    {
        [SerializeField, Range(0, 99)] private float _maxSpeed;
        public float MaxSpeed => _maxSpeed;
    }

    [SerializeField]
    public class GameFiledSettings
    {
        public float UpperWallPositionY => 3.55f;
        public float RightWallPositionX => 5.9f;
    }
}