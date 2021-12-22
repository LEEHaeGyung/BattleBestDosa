using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject endPanel;         //게임결과(도사이미지가 나타나는) 패널
    public GameObject resultNotice;     //게임종료 안내 이미지
    public GameObject yoonCharBtn;      //홈화면의 윤캐릭터 버튼
    public GameObject jinCharBtn;       //홈화면의 진캐릭터 버튼
    public Button checkmapButton;       //지도확인하기 버튼
    public Button findtreasureButton;   //보물찾으러가기 버튼
    public Button jinBtn;               //캐릭터 선택 버튼 중 진버튼
    public Button yoonBtn;              //캐릭터 선택 버튼 중 윤버튼
    public Button yooncollection;       //윤의 민심과 보물 현황 이미지
    public Button jincollection;        //진의 민심과 보물 현황 이미지
    public Button modeChangeBtn;        //혼자하기<->같이하기 전환 버튼
    public Image selectImg;             //캐릭터 선택을 지시하는 이미지
    public Image winnerImg;             //게임종료 후 도사 이미지
    public Image setting;               //설정 이미지

    public Sprite heartGet;             //하트 획득한 이미지
    public Sprite singlemode;           //혼자하기 모드일 경우 지시이미지 배경
    public Sprite modeChange;           //혼자하기 모드일 경우 모드 전환버튼 이미지
    public Sprite[] findStone;          //각 캐릭터의 현황이미지에 나타날 보물 아이콘
    public Sprite[] notImg;             //캐릭터를 고르지 않은 경우 회색프레임의 이미지
    public Image[] jinsheart;           //진의 민심 아이콘들
    public Image[] yoonsheart;          //윤의 민심 아이콘들
    public Image[] jinsstone;           //진의 보물 아이콘들
    public Image[] yoonsstrone;         //윤의 보물 아이콘들

    int isSelect=-1;                    //먼저할 캐릭터 또는 진행할 캐릭터를 선택했는지 여부
    int jincount=0;                     //진이 모은 보물의 개수
    int yooncount=0;                    //윤이 모은 보물의 개수
    int gamemode = 0;                   //혼자하기인지 같이하기인지 판단하는 변수
    bool isSettingOpen = false;         //설정 이미지의 활성화를 결정하는 변수

    void Start()
    {   //"Select" : 캐릭터를 선택한 여부, 0이면 선택안함, 1이면 선택함
        if (!PlayerPrefs.HasKey("Select")) { PlayerPrefs.SetInt("Select", 0); }
        isSelect = PlayerPrefs.GetInt("Select");
        if (isSelect == 0){ 
            //0이면 캐릭터를 선택하라는 지시 이미지를 띄우고 지도확인버튼은 비활성화해둠
            selectImg.gameObject.SetActive(true);
            checkmapButton.enabled = false;}

        //"GameMode" : 게임의 모드, 1이면 혼자하기, 2이면 같이하기
        if (!PlayerPrefs.HasKey("GameMode")) { PlayerPrefs.SetInt("GameMode", 2); }
        gamemode = PlayerPrefs.GetInt("GameMode");
        //모드가 1이라면 혼자하기모드 세팅하는 함수를 호출함
        if (gamemode == 1) { SingleModeSetting(); }

        //"GameStart" : 게임이 시작했는지 여부, 0은 시작안함, 1은 시작함
        //보물이미지를 인식한 후에 값이 1로 변경됨
        if (PlayerPrefs.GetInt("GameStart") == 1)
            findtreasureButton.gameObject.SetActive(true);  //보물찾으러가기 버튼 활성화
        else
            checkmapButton.gameObject.SetActive(true);      //지도확인하기 버튼 활성화

        Treasure();             //보물의 설정을 초기화함

        SetYoonsCollection();   //윤의 현황을 세팅함
        SetJinsCollection();    //진의 현황을 세팅함
        CheckFinish();          //게임이 종료되었는지 확인함
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //이전버튼을 누르면 앱 종료
        {
            Application.Quit();
        }
    }
    public void ShowSetting()   //설정 버튼에 연결할 함수
    {                           //isSettingOpen의 값에 따라 설정 이미지 활성화 또는 비활성화
        isSettingOpen = !isSettingOpen;
        setting.gameObject.SetActive(isSettingOpen);
    }
    public void ResetGame()         //설정에서 게임다시시작을 누르면 실행되는 함수
    {
        PlayerPrefs.DeleteAll();                    //모든 key를 삭제함
        PlayerPrefs.SetInt("GameMode", gamemode);   //게임모드는 변경되지 않으므로 다시 값을 저장
        PlayerPrefs.SetInt("Select", 0);
        SceneManager.LoadScene("Home");             //홈화면을 다시 로드함
    }
    public void ChangeMode()        //모드전환 버튼을 누르면 실행되는 함수
    {
        if (gamemode == 2) { gamemode = 1; }        //모드의 값에 따라 다른 값으로 변경
        else if (gamemode == 1) { gamemode = 2; }
        ResetGame();                                //모드를 변경하면 게임 다시 시작
    }
    public void SingleModeSetting() //혼자히기 모드와 관련한 설정
    {   //캐릭터 선택 지시 이미지의 배경을 변경
        selectImg.sprite = singlemode;
        //모드전환 버튼의 이미지를 변경, 기본은 [혼자하기로전환], 변경은 [같이하기로전환]
        modeChangeBtn.image.sprite = modeChange;
        if (PlayerPrefs.GetInt("Select") != 0)
        {
            //PlayerPrefs.GetInt("Select") != 0 라면 캐릭터를 하나 선택한 것이므로 설정 변경
            //선택 플레이어에 해당하는 캐릭터 버튼만 남두고 다른 캐릭터 버튼은 비활성화
            //그리고 버튼의 위치를 가운데로 이동함
            if (PlayerPrefs.GetString("NowPlayer") == "Jin") {
                yoonCharBtn.SetActive(false);
                jinCharBtn.transform.localPosition = new Vector2(0, -197);
            }
            else{
                jinCharBtn.SetActive(false);
                yoonCharBtn.transform.localPosition = new Vector2(0, -197);
            }
        }
    }
    public void GotoCheckMap()      //지도확인하기 버튼을 눌렀을 때 실행되는 함수
    {   //지도 확인 Scene을 로드함
        SceneManager.LoadScene("CheckMap");
    }
    public void GotoFindTreasure()  //보물찾으러가기 버튼을 눌렀을 때 실행되는 함수
    {   //지도가 그려진 Scene을 로드함
        SceneManager.LoadScene("TreasureMap");
    }
    public void SelectJin()         //캐릭터 선택에서 진을 골랐을 때 실행되는 버튼
    {   //"NowPlayer" : 현재 플레이어를 이미, "Jin" 또는 "Yoon"을 저장
        PlayerPrefs.SetString("NowPlayer", "Jin");
        SelectPlayer(yoonBtn, 0);
        if (gamemode == 1) SingleModeSetting();
    }
    public void SelectYoon()        //캐릭터 선택에서 윤을 골랐을 때 실행되는 버튼
    {
        PlayerPrefs.SetString("NowPlayer", "Yoon");
        SelectPlayer(jinBtn, 1);
        if (gamemode == 1) SingleModeSetting();
    }
    void SelectPlayer(Button charBtn, int type)
    {   //선택되지 않은 캐릭터의 버튼의 이미지를 변경, type의 값이 0은 윤, 1은 진을 선택하지 않은 이미지
        charBtn.image.sprite = notImg[type];
        //선택되지 않은 버튼의 크기를 변경함
        charBtn.transform.transform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        //"Select"의 값을 변경
        PlayerPrefs.SetInt("Select", 1);
        //캐릭터 선택 이미지를 2초 후에 닫음
        Invoke("CloseSelectImg", 2f);
        //지도확인하기 버튼을 활성화
        checkmapButton.enabled = true;
    }
    public void CloseSelectImg()    //캐릭터 선택 이미지를 닫는 함수
    {
        selectImg.gameObject.SetActive(false);
    }
    public void ShowJins()          //진의 모험일지를 활성화하는 함수
    {
        if (PlayerPrefs.GetInt("Select") != 0)
        {   //진의 모험일지를 활성화하면 윤의 모험일지는 비활성화
            jincollection.gameObject.SetActive(true);
            yooncollection.gameObject.SetActive(false);
        }
    }
    public void ShowYoons()         //윤의 모험일지를 활성화하는 함수
    {
        if (PlayerPrefs.GetInt("Select") != 0)
        {   //윤의 모험일지를 활성화하면 진의 모험일지는 비활성화
            yooncollection.gameObject.SetActive(true);
            jincollection.gameObject.SetActive(false);
        }
    }
    public void CloseJins()     //진의 모험일지 비활성화, 진의 모험일지를 누르면 실행됨
    {
        jincollection.gameObject.SetActive(false);
    }
    public void CloseYoons()    //윤의 모험일지 비활성화, 윤의 모험일지를 누르면 실행됨
    {
        yooncollection.gameObject.SetActive(false);
    }
    public void Treasure()      //보물의 설정을 초기화
    {
        for (int i = 1; i <= 5; i++)
        {   //"Treasure1"~"Treasure5"의 key가 없다면 게임 초기 상태를 의미하므로 이 key에 "None"을 저장
            //모물을 획득할 때 각 key의 string 값은 획득한 플레이어의 이름이 됨
            if (!PlayerPrefs.HasKey("Treasure" + i)) { PlayerPrefs.SetString("Treasure" + i, "None"); }
        }
    }
    public void SetYoonsCollection()    //윤의 모험일지를 설정함
    {   //"Yoon_Heart" : 윤이 모은 민심의 갯수
        int heartcount = PlayerPrefs.GetInt("Yoon_Heart");
        for(int i=0; i<heartcount; i++){
            //민심의 개수만큼 하트의 이미지를 획득한 버전으로 변경함
            yoonsheart[i].sprite = heartGet;
        }
        for(int i=1; i <= 5; i++)
        {   //"Treasure1"~"Treasure5"의 값이 "Yoon" 것들을 확인해 아이콘 이미지를 획득상태로 변경함
            if (PlayerPrefs.GetString("Treasure" + i) == "Yoon"){
                yoonsstrone[i - 1].sprite = findStone[i - 1];
                //윤의 보물 획득 개수를 증가함
                yooncount++;
            }
        }
    }
    public void SetJinsCollection() //진의 모험일지를 설정함
    {   //"Jin_Heart" : 진이 모은 민심의 갯수
        int heartcount = PlayerPrefs.GetInt("Jin_Heart");
        for (int i = 0; i < heartcount; i++){
            jinsheart[i].sprite = heartGet;
        }
        for (int i = 1; i <= 5; i++)
        {   //"Treasure1"~"Treasure5"의 값이 "Jin" 것들을 확인해 아이콘 이미지를 획득상태로 변경함
            if (PlayerPrefs.GetString("Treasure" + i) == "Jin") {
                jinsstone[i - 1].sprite = findStone[i - 1];
                //진의 보물 획득 개수를 증가함
                jincount++;
            }
        }
    }
    void CheckFinish()                      //게임이 종료될 수 있는 상황인지 판단함
    {   //두 사람의 보물 획득 수가 5이상이면 모든 보물을 찾을 걸 의미
        //종료를 안내하는 이미지를 활성화
        if ((jincount + yooncount) >= 5) { resultNotice.SetActive(true); } 
    }
    public void WhoIsWinner() //승자를 확인하는 함수, 종료 안내이미지에서 결과보기 버튼을 누르면 실행
    {   //승자의 이름이 저장될 문자열
        string winner;
        //보물획득 개수에 따라 승자 이름을 변경
        if (jincount > yooncount)
            winner = "Jin";
        else
            winner = "Yoon";
        //결과 이미지를 활성화
        endPanel.SetActive(true);
        //승리한 캐릭터의 이미지를 도사 이미지로 설정함
        winnerImg.sprite = Resources.Load<Sprite>(winner + "winner_ver");
    }
}
