using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GetTreasure : MonoBehaviour    //보물을 획득하는 장면에서 사용될 클래스
{
    public Sprite[] dialogImg;      //변경될 도깨비의 이미지
    public Image speechBubble;      //도깨비의 말풍선
    public Image stone;             //보물 이미지
    public GameObject stoneImg;     //보물이미지+빛나는효과이미지를 함께 자식오브젝트로 둔 게임오브젝트
    public Button findBtn;          //보물획득 버튼(보물을 획득한 경우)
    public Button returnBtn;        //돌아가기 버튼(획득 자격을 갖추지 못한 경우)

    string nowplayer;               //현재 플레이어
    string heartkey;                //하트의 갯수를 알기 위해 접근할 key의 이름, 현재플레이어+_Heart가 됨
    int dialogIndex = 0;            //대사의 인덱스, 인덱스가 증가하면 대사가 변경됨
    private void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer"); //현재 플레이어 문자열을 읽음
        heartkey = nowplayer + "_Heart";                //하트 갯수를 확인할 key의 이름 설정
    }
    public void NextDialog(Button charBtn)        //도깨비 이미지를 누를 때마다 실행되는 함수, 대사가 진행됨
    {
        if (dialogIndex == 0)       //0이면 아직 대화를 시작한게 아니므로 말풍선이 나타나게 함
            speechBubble.gameObject.SetActive(true);
        if (dialogIndex < 2)        //2보다 작을때는 dialogIndex의 값을 증가하며 대사 변경
        {   speechBubble.sprite = dialogImg[dialogIndex];
            dialogIndex++;
        }
        else                        //2보다 큰 경우 자격을 갖췄는 지에 대사가 변경되므로 추가적으로 조건문 작성
        {
            charBtn.enabled = false;   //대사는 더 이상 나오지 않을 것이므로 버튼은 비활성화 함
            if (PlayerPrefs.GetInt(heartkey) >= 5)
            {   //만약 민심을 5개 이상 획득했다면 가져가라는 대사와 함께 보물 이미지가 등장하는 함수 호출
                speechBubble.sprite = dialogImg[dialogIndex];
                Invoke("ShowStone", 2f);
            }
            else
            {   //만약 민심을 5개 이상 모으지 못했다면 돌아가라는 문구와 함께 지도로 돌아가는 버튼이 나옴
                speechBubble.sprite = dialogImg[dialogIndex + 1];
                returnBtn.gameObject.SetActive(true);
            }
        }
    }
    void ShowStone()
    {   //"FindTreasure" : 찾을 보물의 종류, 지도확인 과정에서 보물 그림을 인식하면 결정됨
        int type = PlayerPrefs.GetInt("FindTreasure");
        //보물의 Sprite를 type에 해당하는 이미지로 변경
        stone.sprite = Resources.Load<Sprite>("mysteriousStone" + type);
        //보물의 이미지를 활성화
        stoneImg.SetActive(true);
        //보물획득 버튼이 나타남
        findBtn.gameObject.SetActive(true);
    }
    public void FindBtn()
    {   //"Treasure"와 현재 찾은 보물의 번호를 연결한 문자열을 key로 설정하고
        //현재 플레이어의 이름을 저장함
        string findkey = "Treasure" + PlayerPrefs.GetInt("FindTreasure");
        PlayerPrefs.SetString(findkey, nowplayer);
        //보물을 획득했으므로 새로운 보물을 찾기 시작하므로 NewTreasure() 함수 호출
        FinishTurn.NewTreasure(nowplayer);
        SceneManager.LoadScene("Home");
    }
    public void ReturnMap()     //돌아가기 버튼을 누르면 실행되는 함수
    {   //현재 차례를 변경하고 지도 Scene으로 이동함
        FinishTurn.NewGame(nowplayer);
        SceneManager.LoadScene("TreasureMap");
    }
}
