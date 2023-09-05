using UnityEngine.SceneManagement;

namespace Pong.Scene
{
    public class SceneService : ISceneService
    {
        public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}