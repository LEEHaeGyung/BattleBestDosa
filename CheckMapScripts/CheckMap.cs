using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckMap : MonoBehaviour   //���� Ȯ�� �Ϸ��� ��� ��ư�� �������� ����Ǵ� �Լ�
{
    public void SetGame()
    {   //"GameStart"�� ����� ���� �����ϰ� Ȩȭ������ �̵�, ���� 1�� �����Ƿ� Ȩȭ�鿡 ��ư�� �޶���
        PlayerPrefs.SetInt("GameStart", 1);
        SceneManager.LoadScene("Home");
    }
}