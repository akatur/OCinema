using UnityEngine;
using UnityEngine.SceneManagement;
//������ ��� ������������ ����� �������
public class menuScript : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("out");
        Application.Quit();
    }
    public void toMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void toGame()
    {
        Debug.Log("togame");
        SceneManager.LoadScene("game");
    }
}
