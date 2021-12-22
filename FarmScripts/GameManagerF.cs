using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerF : MonoBehaviour   //농산물 개수세기 게임에 사용될 클래스
{
    public GameObject[] radishOrCabbage;    //배추 또는 무가 될 오브젝트
    public Material[] farmProduceType;      //배추 또는 무 재질
    public GameObject noticePanel;  //안내 패널
    public GameObject inputPanel;   //개수입력 패널
    public Image howtoImg;          //게임 방법 안내
    public Image resultImg;         //결과 이미지
    public Image playerImg;         //현재 플레이어 이미지
    public Image[] heart;           //획득 민심 아이콘
    public Image checkanswerImg;    //정답오답 표시 이미지
    public Image farmer;            //농부 이미지
    public Text timerText;          //타이머 텍스트
    public InputField radishInput;  //무의 개수 입력필드
    public InputField cabbageInput; //배추의 개수 입력필드

    public Button checkBtn;         //개수세기완료 버튼
    public Button successBtn;       //도와주기성공 버튼
    public Button failBtn;          //도와주기실패 버튼
    public Sprite wrong;            //X표시 이미지
    public Sprite right;            //O표시 이미지
    public Sprite[] emotion;        //농부 표정

    bool isOpen = false;        //게임방법 이미지 활성화 여부 결정
    bool isPaused = false;      //타이머 멈춤 여부
    bool isStart = false;       //게임 시작 여부
    float starttime;            //시작 시간
    float timeremain;           //남은 시간
    string nowplayer;           //현재 플레이어
    int radishCount=0;          //무의 개수
    int cabbageCount=0;         //배추의 개수
    int getheart;               //얻을 하트의 개수   
    void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg");

        for(int i=0; i < radishOrCabbage.Length; i++)
        {   //type의 값을 0이나 1 중 하나로 지정
            int type = Random.Range(0, 2);
            //배추또는무가 될 오브젝트의 material을 farmProduceType 중 type의 인덱스로 변경
            radishOrCabbage[i].GetComponent<MeshRenderer>().material = farmProduceType[type];
            //type==0 : 배추 type==1 : 무, type의 값에 따라 무 배추 개수 증가
            if (type == 0) { cabbageCount++; }
            else { radishCount++; }
        }
    }
    void Update()
    {   //타이머가 종료되지 않았고 게임이 시작됐다면 타이머 진행
        if (!isPaused && isStart) { CheckTimer(); }
    }
    void CheckTimer()
    {   //시작 시간에서 Time.time을 빼서 남은 시간 계산
        timeremain = starttime - Time.time;
        if (timeremain <=0)
        {   //남은 시간이 0이되면 isPaused를 변경하고 개수 입력 패널을 활성화함
            timeremain = 0;
            isPaused = true;
            inputPanel.SetActive(true);
        }
        timerText.text = ((int)timeremain).ToString();
    }
    public void StartBtn()  //도와주러가기 버튼을 누르면 실행
    {
        noticePanel.SetActive(false);
        //isStart의 값을 변경
        isStart = true;
        //시작 시간을 초기화함
        starttime = 10f + Time.time;
    }
    public void ShowHowToPlay() //게임 방법 안내 패널
    {
        isOpen = !isOpen;
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void CheckCount()    //개수세기완료 버튼을 누르면 실행
    {                           //입력된 값이 정답인지 확인
        //각 입력 필드에서 입력된 개수를 정수로 읽어옴
        int radishinput = int.Parse(radishInput.text);  
        int cabbageinput = int.Parse(cabbageInput.text);
        //입력된 무와 배추의 개수가 Start()에서 결정된 무와 배추의 개수와 같다면 정답
        if (radishinput == radishCount && cabbageinput == cabbageCount)
        {   //O표시를 나타내고 농부의 표정을 웃는 얼굴로 변경
            checkanswerImg.sprite = right;
            farmer.sprite = emotion[0];
            successBtn.gameObject.SetActive(true);
        }
        else
        {   //X표시를 나타내고 농부의 표정을 화난 얼굴로 변경
            checkanswerImg.sprite = wrong;
            farmer.sprite = emotion[1];
            failBtn.gameObject.SetActive(true);
        }
        checkBtn.gameObject.SetActive(false);
        checkanswerImg.gameObject.SetActive(true);
    }
    public void SuccessBtn()        //도와주기성공 버튼을 누르면 실행
    {
        getheart = 2;   //민심을 1개 얻은 걸 의미
        successBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    void FinishGame()
    {
        for(int i=0; i < getheart; i++) { heart[i].gameObject.SetActive(true); }
        resultImg.gameObject.SetActive(true);
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + getheart;             //getheart의 값을 더해 민심의 개수를 변경
        if (heartcount > 0) { PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount); }
        else { PlayerPrefs.SetInt(nowplayer + "_Heart", 0); }
        FinishTurn.NewGame(nowplayer);
    }
    public void FailBtn()       //도와주기실패 버튼을 누르면 실행
    {
        getheart = 0;
        failBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("TreasureMap");
    }
}
