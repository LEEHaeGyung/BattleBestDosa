using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoHomeBtn : MonoBehaviour
{
    public void LoadHomeScene() //Ȩ��ư�� �������� ����ȭ������ �̵�
    {
        SceneManager.LoadScene("Home");
    }
}
