using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerB : MonoBehaviour   //복주머니 찾기 게임에 사용되는 클래스
{
    public GameObject noticePanel;  //안내 패널
    public Image howtoImg;          //게임 방법 안내
    public Image bokhint;           //힌트에서 복주머니 이미지
    public Image itemHint;          //힌트에서 물건 이미지
    public Image hintImg;           //힌트 이미지
    public Image resultImg;         //결과 이미지
    public Image playerImg;         //현재 플레이어 이미지
    public Image heart;             //획득 민심 아이콘
    public Image checkbok;          //복주머니가 인식된 상태에 나타는 느낌표 아이콘
    public Image checkanswerImg;    //정답오답 표시 이미지
    public Image itembubble;        //부인이 찾는 물건이 나타날 이미지
    public Image woman;             //부인 이미지

    public Button startMatchBtn;    //물건돌려주기 버튼
    public Button checkBtn;         //물건확인하기 버튼
    public Button successBtn;       //도와주기성공 버튼
    public Button failBtn;          //도와주기실패 버튼
    public Sprite wrong;            //X표시 이미지
    public Sprite right;            //O표시 이미지
    public Sprite lostheart;        //민심을 잃은 경우 하트 아이콘
    public Sprite[] bokType;        //복주머니 이미지 조류
    public Sprite[] itemType;       //물건 이미지 종류
    public Sprite[] needitems;      //부인이 원하는 물건이 그려진 말풍선 종류
    public Sprite[] emotion;        //부인의 표정
    public Text hintText;           //힌트 개수

    public int[] bokitem = { 0, 0, 0 }; //복주머니에 들어있는 아이템 인덱스
    public bool isStart = false;        //돌려주기 시작한 여부
    public string checkBokName="None";  //현재 인식된 복주머니 이름
    bool isOpen = false;        //게임방법 이미지 활성화 여부 결정
    bool isFinish = false;      //게임이 종료된 여부
    int hintCount;              //힌트 개수
    string nowplayer;           //현재 플레이어
    int needitem;               //부인이 원하는 아이텐 인덱스
    int getheart;               //민심을 잃은 것인지 획득할 것인지 여부     
    void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");
        hintText.text = hintCount.ToString();
        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg");

        needitem = Random.Range(0, 3);              //부인이 찾는 아이템 번호를 랜덤으로 결정
        itembubble.sprite = needitems[needitem];    //이에 따라 말풍선 이미지를 변경(아직활성화X)
    }
    void Update()
    {
        if (!isFinish)
        {
            if (checkBokName != "None") //복주머니가 인식되면 느낌표 아이콘을 활성화
                checkbok.gameObject.SetActive(true);
        }
    }
    public void StartCheckBok()     //물건돌려주기 시작
    {
        isStart = true;
        startMatchBtn.gameObject.SetActive(false);
        checkBtn.gameObject.SetActive(true);
        woman.gameObject.SetActive(true);
    }
    public void StartBtn()
    {
        noticePanel.SetActive(false);
    }
    public void ShowHowToPlay()
    {
        isOpen = !isOpen;
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void UseHint()       //힌트 사용 함수
    {
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");
        if (hintCount > 0 && isStart)
        {
            int hintType = Random.Range(0, 3);              //힌트의 종류를 랜덤으로 결정, 꼭 부인이 찾는 물건이 들어있는 복주머니를 알려주는 것이 아님
            bokhint.sprite = bokType[hintType];
            itemHint.sprite = itemType[bokitem[hintType]];
            hintImg.gameObject.SetActive(true);
            Invoke("CloseHint", 1.5f);
            hintCount--;
            PlayerPrefs.SetInt(nowplayer + "_Hint", hintCount);
            hintText.text = hintCount.ToString();
        }
    }
    public void CloseHint()
    {
        hintImg.gameObject.SetActive(false);
    }
    public void CheckBok()  //맞는 복주머니인지 확인하는 함수, 물건확인하기 버튼을 누르면 실행
    {
        int checkindex = 0;
        if (checkBokName != "None")     //복주머니의 이름이 None 아닐 때만 실행
        {
            if (checkBokName.Contains("1")) { checkindex = 0; }        //복주머니의 번호 확인
            else if (checkBokName.Contains("2")) { checkindex = 1; }
            else if (checkBokName.Contains("3")) { checkindex = 2; }
            if (bokitem[checkindex] == needitem)        //복주머니 번호에 해당하는 bokitem의 값과 부인이 찾는 물건의 번호와 같으면 정답
            {
                successBtn.gameObject.SetActive(true);
                woman.sprite = emotion[1];          //부인의 표정 변경
                checkanswerImg.sprite = right;
            }
            else
            {
                failBtn.gameObject.SetActive(true);
                woman.sprite = emotion[0];          //부인의 표정 변경
                checkanswerImg.sprite = wrong;
            }
            checkBtn.gameObject.SetActive(false);
            checkanswerImg.gameObject.SetActive(true);
        }
    }
    public void SuccessBtn()        //도와주기성공 버튼을 누르면 실행
    {  
        getheart = 1;   //민심을 1개 얻은 걸 의미
        successBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    void FinishGame()
    {
        if (getheart < 0) { heart.sprite = lostheart; } //getheart가 음수여서 하트를 잃는 상황이면 하트 아이콘을 회색 아이콘으로 변경
        resultImg.gameObject.SetActive(true);
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + getheart;             //getheart의 값을 더해 민심의 개수를 변경
        if (heartcount > 0) { PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount); }
        else { PlayerPrefs.SetInt(nowplayer + "_Heart", 0); }
        FinishTurn.NewGame(nowplayer);
    }
    public void FailBtn()       //도와주기실패 버튼을 누르면 실행
    {
        getheart = -1;  //민심을 1개 잃은 걸 의미
        failBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("TreasureMap");
    }
}
