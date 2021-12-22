using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerDD : MonoBehaviour  //도령 옷입히기 게임에 사용되는 클래스
{
    public GameObject noticePanel;      //게임 안내 패널
    public GameObject outPanel;         //외출할 상태으 패널
    public Image howtoImg;              //게임 방법 안내 이미지
    public Image resultImg;             //결과 이미지
    public Image playerImg;             //플레이어 이미지
    public Image heart;                 //민심 아이콘
    public Image textbubble;            //대사 말풍선
    public Image doryung;               //도령 이미지
    public Image resultbubble;          //외출할 경우 다른 사람의 반응 말풍선

    public Button successBtn;           //도와주기성공 버튼
    public Button failBtn;              //도와주기실패 버튼
    public Sprite lostheart;            //민심을 잃은 경우 아이콘
    public Sprite[] dressType;          //도령의 옷 종류
    public Sprite[] requestBubble;      //도령의 대사 이미지들
    public Sprite[] resultText;         //결과 대사의 종류

    bool isOpen = false;                //게임 방법의 활성화 여부
    string nowplayer;                   //현재 플레이어
    int needdress;                      //필요한 옷의 인덱스
    int getheart;                       //하트를 잃는지 얻는지 여부
    int nowDress=-1;                    //현재 입은 옷의 인덱스
    void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg");

        needdress = Random.Range(0, 3);             //필요한 옷의 종류를 랜덤으로 결정
        textbubble.sprite = requestBubble[needdress];//필요한 옷에 따라 도령의 요구 대사 변경
    }
    public void LetsGoOut()     //외출하기 버튼을 눌렀을 때 실행되는 함수
    {   //nowDress가 -1이라면 아직 아무 옷도 입지 않은 상태이므로 실행하지 않음
        if (nowDress != -1)     
        {
            outPanel.SetActive(true);   //외출상태 패널 활성화
            if (needdress == nowDress)  //필요한 옷과 지금 입은 옷의 인덱스가 같으면 성공
            {
                resultbubble.sprite = resultText[needdress];    //반응대사 변경
                successBtn.gameObject.SetActive(true);
            }
            else
            {
                failBtn.gameObject.SetActive(true);
            }
        }
    }
    public void StartBtn()      //도와주러가기 버튼 누르면 실행
    {
        noticePanel.SetActive(false);
    }
    public void ShowHowToPlay() //게임방법 확인
    {
        isOpen = !isOpen;
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void SuccessBtn()    //도와주기성공 버튼 누르면 실행
    {
        getheart = 1;           //민심을 얻음
        successBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    void FinishGame()
    {
        if (getheart < 0) { heart.sprite = lostheart; }
        resultImg.gameObject.SetActive(true);
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + getheart;
        if (heartcount > 0) { PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount); }
        else { PlayerPrefs.SetInt(nowplayer + "_Heart", 0); }
        FinishTurn.NewGame(nowplayer);
    }
    public void FailBtn()   //도와주기실패 버튼 누르면 실행
    {
        getheart = -1;      //민심을 잃음
        failBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("TreasureMap");
    }
    public void SetDress(int type)          //옷 이미지가 인식된 경우 실행되는 함수
    {
        doryung.sprite = dressType[type];   //현재 입은 옷의 종류에 따라 도령이미지 변경
        nowDress = type;                    //nowDress의 값을 변경
    }
}
