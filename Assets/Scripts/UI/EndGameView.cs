using Pong.Match;
using Pong.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Pong.View
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private GameObject _firstPlayerWinObj;
        [SerializeField] private GameObject _secondPlayerWinObj;
        [SerializeField] private GameObject _drawObj;

        private readonly CompositeDisposable _disposables = new();
        private IMatchControlService _matchControlService;
        private GameSettings _gameSettings;

        [Inject]
        private void Inject(IMatchControlService matchControlService)
        {
            _matchControlService = matchControlService;
        }

        private void Awake()
        {
            _matchControlService.OnMatchEnd.Subscribe(ShowEndGameView).AddTo(_disposables);
            _playButton.gameObject.SetActive(MatchLauncher.FirstStart);
            _playButton.onClick.AddListener(() => _playButton.gameObject.SetActive(false));
        }

        private void ShowEndGameView(Side winSide)
        {
            _firstPlayerWinObj.SetActive(winSide == Side.Left);
            _secondPlayerWinObj.SetActive(winSide == Side.Right);
            _drawObj.SetActive(winSide == Side.None);
            _restartButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _disposables?.Clear();
        }
    }
}