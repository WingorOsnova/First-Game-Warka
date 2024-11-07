using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод для кнопки "Play"
    public void PlayGame()
    {
        // Замените "GameScene" на название сцены с вашей игрой
        SceneManager.LoadScene("GameScene");
    }

    // Метод для кнопки "Exit"
    public void ExitGame()
    {
        // Для выхода из игры
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
