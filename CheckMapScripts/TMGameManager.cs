using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TMGameManager : MonoBehaviour  //보물 이미지 활성화와 관련한 클래스
{
    public Button mapButton;                //자도 이미지인 버튼
    public Button isFoundImg;               //이미 찾은 보물이라는 걸 알려주는 이미지버튼
    public Sprite[] treasureType;           //보물 이미지 종류

    bool isCheck;   //지도를 확인했는지 판단하는 변수
    private void Start()
    {   //지도 이미지는 비활성화 해놓고 시작함
        mapButton.gameObject.SetActive(false);
        isCheck = false;
    }
    void Update()
    {   //뒤로가기 버튼을 누르면 홈화면으로 이동함
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home");
        }
    }
    public void ShowMap(int type)   //지도 이미지를 보여주는 함수
    {                               //초기에는 보물 이미지이지만, 추후 Sprite 변경으로 지도가 됨
        if (!isCheck)               //isCheck가 false 일때만 수행, 중복적으로 지도를 확인하는 걸 방지 
        {
            string treasureKey = "Treasure" + type;             //매개변수로 받은 type 변수로 key 값을 결정
            if (PlayerPrefs.GetString(treasureKey)=="None") {   //key에 저장된 문자열이 "None"이면 아직 찾기 전
                isFoundImg.gameObject.SetActive(false);         //이미 찾은 보물이라는 이미지가 활성화 상태면 다시 비활성화
                mapButton.image.sprite = treasureType[type - 1];//이미지를 인식한 보물 이미지와 같은 것으로 변경
                mapButton.gameObject.SetActive(true);           //이미지 버튼를 활성화
                PlayerPrefs.SetInt("FindTreasure", type);       //"FindTreasure":현재 찾으려는 보물을 의미, 인식한 보물의 번호가 저장됨
                isCheck = true;     //isCheck의 값을 변경함, 이로인해 지도를 중복적으로 확인하는 걸 방지함
            }
            else//key에 저장된 문자열이 "None"이 아니라면, 누군가 찾은 보물이므로 이미 찾았다는 안내 표시
            {isFoundImg.gameObject.SetActive(true);}
        }
    }
    public void CloseImg()  //이미 찾은 보물 안내 이미지를 눌렀을 때 실행되는 함수
    {   //이미지를 다시 비활성화 함
        isFoundImg.gameObject.SetActive(false);
    }
    public void CloseMap()
    {
        if (!isCheck) mapButton.gameObject.SetActive(false);
    }
}
