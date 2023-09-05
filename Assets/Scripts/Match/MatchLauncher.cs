using Pong.Scene;
using UnityEngine;
using Zenject;

namespace Pong.Match
{
    public class MatchLauncher : MonoBehaviour
    {
        private IMatchControlService _matchControlService;
        private ISceneService _sceneService;

        public static bool FirstStart { get; private set; } = true;

        private void Start()
        {
            if (!FirstStart)
            {
                StartMatch();
            }
        }

        [Inject]
        private void Inject(IMatchControlService matchControlService, ISceneService sceneService)
        {
            _matchControlService = matchControlService;
            _sceneService = sceneService;
        }

        public void StartMatch()
        {
            _matchControlService.StartMatch();
            FirstStart = false;
        }

        public void RestartMatch() => _sceneService.ReloadScene();
    }
}