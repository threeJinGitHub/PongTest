using Pong.Balls;
using Pong.Boosters;
using Pong.Input;
using Pong.Match;
using Pong.Player;
using Pong.Player.Ai;
using Pong.Scene;
using Pong.Settings;
using UnityEngine;
using Zenject;

namespace Pong
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private Board _board1;
        [SerializeField] private Board _board2;
        [SerializeField] private BallPoolFactory _ballPoolFactory;
        [SerializeField] private InputFactory _inputFactory;
        [SerializeField] private BoosterFactory _boosterFactory;

        private BoosterService _boosterService;
        private MatchControlService _matchControlService;
        private BoosterSpawnService _boosterSpawnService;

        public override void InstallBindings()
        {
            var sceneService = new SceneService();

            var ballSpawnerService = new BallSpawnerService(_gameSettings, _ballPoolFactory);

            var playerBardController =
                new BoardController(_board1, _inputFactory.GetInput(_gameSettings.InputSettings));

            var player = new Player.Player(playerBardController);

            var aiBoardController = new BoardController(_board2, new AiInput());

            var ai = new AiPlayer(aiBoardController, ballSpawnerService, _gameSettings);

            _boosterSpawnService = new BoosterSpawnService(_boosterFactory, _gameSettings);

            _boosterService = new BoosterService(
                playerBardController,
                aiBoardController,
                _gameSettings.BoostersSetting,
                ballSpawnerService,
                _boosterSpawnService);

            _matchControlService = new MatchControlService(
                player,
                ai,
                ballSpawnerService,
                _gameSettings.MatchSettings,
                _boosterSpawnService);

            Container.BindInstance(_boosterService).AsSingle();
            Container.BindInstance<IBoosterSpawnService>(_boosterSpawnService).AsSingle();
            Container.BindInstance<IBallSpawnerService>(ballSpawnerService).AsSingle();
            Container.BindInstance<IMatchControlService>(_matchControlService).AsSingle();
            Container.BindInstance<ISceneService>(sceneService).AsSingle();
            Container.BindInstance(_gameSettings).AsSingle();
        }

        private void OnDestroy()
        {
            _boosterService.Deactivate();
            _boosterSpawnService.Deactivate();
            _matchControlService.Deactivate();
        }
    }
}