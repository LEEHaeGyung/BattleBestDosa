using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckMap : MonoBehaviour   //지도 확인 완료한 경우 버튼을 눌렀을때 실행되는 함수
{
    public void SetGame()
    {   //"GameStart"에 저장된 값을 변경하고 홈화면으로 이동, 값이 1이 됐으므로 홈화면에 버튼이 달라짐
        PlayerPrefs.SetInt("GameStart", 1);
        SceneManager.LoadScene("Home");
    }
}