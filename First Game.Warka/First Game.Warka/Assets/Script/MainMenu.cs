using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // ����� ��� ������ "Play"
    public void PlayGame()
    {
        // �������� "GameScene" �� �������� ����� � ����� �����
        SceneManager.LoadScene("GameScene");
    }

    // ����� ��� ������ "Exit"
    public void ExitGame()
    {
        // ��� ������ �� ����
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
