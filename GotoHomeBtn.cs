using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoHomeBtn : MonoBehaviour
{
    public void LoadHomeScene() //홈버튼을 눌렀을때 시작화면으로 이동
    {
        SceneManager.LoadScene("Home");
    }
}
