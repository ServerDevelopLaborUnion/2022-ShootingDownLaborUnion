using UnityEngine;
using DG.Tweening;
public enum SceneType
{
    Title,
    Login,
    Lobby,
    Room,
    Game
}

public static class SceneLoader
{
    public static void Load(SceneType sceneType)
    {
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(FadeManager.Instance.FadeObject.DOFade(1f, 1f));

        sequence.AppendCallback(() =>
        {
            switch (sceneType)
            {
                case SceneType.Title:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("00.Title");
                        break;
                    }
                case SceneType.Login:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("01.Login");
                        break;
                    }
                case SceneType.Lobby:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("02.Lobby");
                        break;
                    }
                case SceneType.Room:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("03.Room");
                        break;
                    }
                case SceneType.Game:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("04.Game");
                        break;
                    }
            }
        });

    }
}
