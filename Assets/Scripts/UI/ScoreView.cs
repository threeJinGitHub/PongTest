using Pong.Match;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Pong.View
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _leftPlayerScore;
        [SerializeField] private TextMeshProUGUI _rightPlayerScore;

        private readonly CompositeDisposable _disposables = new();
        private IMatchControlService _matchControlService;

        [Inject]
        private void Inject(IMatchControlService matchControlService)
        {
            _matchControlService = matchControlService;
        }

        private void OnDestroy()
        {
            _disposables?.Clear();
        }

        private void Awake()
        {
            var (player1, player2) = _matchControlService.GetPlayersInMatch();
            player1.PlayerScore.Subscribe(x => UpdateScore(player1.Side, x)).AddTo(_disposables);
            player2.PlayerScore.Subscribe(x => UpdateScore(player2.Side, x)).AddTo(_disposables);
        }

        private void UpdateScore(Side side, int score)
        {
            switch (side)
            {
                case Side.Left:
                    _leftPlayerScore.text = score.ToString("D2");
                    break;
                case Side.Right:
                    _rightPlayerScore.text = score.ToString("D2");
                    break;
            }
        }
    }
}