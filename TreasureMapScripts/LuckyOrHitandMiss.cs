using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LuckyOrHitandMiss : MonoBehaviour  //운수대통, 새옹지마에 따라 수행되는 클래스
{
    public GameObject findStone;    //보물을 뺏은 경우 나타나는 게임오브젝트
    public GameObject lorhPanel;    //운수대통, 새옹지마 상황인 경우 활성화할 패널
    public Image stoneImg;          //findStone에 포함된 보물 이미지
    public Image noticePanel;       //안내 이미지, 어떤 상황이 제시되는지 안내되는 이미지
    public Image cantstillPanel;    //상대의 보물이 없어 보물을 뺏을 수 없는 경우 안내 이미지
    public Image hintResult;        //힌트 획득 여부가 안내되는 이미지
    public Image heartResult;       //민심 획득 여부가 안내되는 이미지
    public Image heartImg;          //민심 아이콘
    public Image[] luckyIcon;       //행운 아이콘

    public Button endBtn;           //종료 버튼
    public Button showBtn;          //결과보기 버튼
    public Sprite lostheart;        //하트를 잃을 경우의 하트 이미지(회색이미지)
    public Sprite losthint;         //힌트를 잃을 경우 힌트 이미지(회색이미지)
    public Sprite[] luckyTypeImg;   //운수대통 상황 종류
    public Sprite[] HitMissTypeImg; //새옹지마 상황 종류
    public Sprite[] stoneType;      //보물의 종류

    string typename="none";         //운수대통 상황인지 아닌지 확인 문자열, "none"이면 둘 중 어떤 상황도 아닌 걸 의미
    string nowplayer;               //현재 플레이어 이름
    int luckyindex;                 //luckytype의 인덱스가 될 변수
    int[] luckytype = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3 }; //운수대통의 상황, 3인 경우는 0~2보다 희귀하도록 배열의 값의 개수를 다르게 설정함
    int notType;                    //새옹지마 상황
    bool isLost;                    //잃을 상황인지 판단하는 변수
private void Start()
    {
        isLost = false;
        if (PlayerPrefs.HasKey("LuckyOrNot"))           //"LuckyOrNot":운수대통 상황인지 아닌지 확인하는 key, "Lucky"는 운수대통, 아닐 경우 "Not"
        { typename = PlayerPrefs.GetString("LuckyOrNot"); }
        nowplayer = PlayerPrefs.GetString("NowPlayer"); //현재 플레이어 이름을 읽어옴
        if (typename != "none")                         //typename이 none이 아니라면
        {                                               //운수대통 또는 새옹지마 상황 발생
            lorhPanel.SetActive(true);                  //안내 패널 활성화
            if (typename == "Lucky") { SetLucky(); }    //"Lucky"이면 운수대통 상황 세팅
            else { SetHitandMiss(); }                   //아니라면 새옹지마 상황 세팅
        }
    }
    void SetLucky()     //운수대통 설정 세팅
    {
        luckyindex = Random.Range(0, luckytype.Length); //인덱스 값을 랜덤으로 결정
        //luckytype[luckyindex]에 해당하는 운수대통 상황으로 안내 이미지 변경
        noticePanel.sprite = luckyTypeImg[luckytype[luckyindex]];
        //luckytype[luckyindex]의 값이 0이면 종료하기 버튼 활성화
        //0인 경우는 원하는 곳으로 이동하는 것이므로 종료버튼만 활성화
        if (luckytype[luckyindex] == 0) { endBtn.gameObject.SetActive(true); }
        else { showBtn.gameObject.SetActive(true); }    //0이 아닌 경우 결과보기 버튼 활성화
    }
    void SetHitandMiss()                //새옹지마 설정 세팅
    {
        notType = Random.Range(0, 3);                   //새옹지마 상황 결정
        noticePanel.sprite = HitMissTypeImg[notType];   //값에 따라 안내 이미지 변경
        int random = Random.Range(0, 2);
        if (random == 0) { isLost = true; } //random의 값에 따라 isLost값 변경, true면 잃는 상황
        if (notType == 2){ //notType은 2인 경우 위치만 바꾸고 종료버튼 활성화
            ExchangePos();
            endBtn.gameObject.SetActive(true);
        }
        else{   //아닌 경우 결과보기 버튼 활성화
            showBtn.gameObject.SetActive(true);
        }
    }
    public void ExchangePos()       //두 플레이어의 위치를 변경하는 함수
    {   //같이하기 모드일때만 수행, 혼자하기면 위치 변경이 없음
        if (PlayerPrefs.GetInt("GameMode") == 2)
        {
            int jpos = PlayerPrefs.GetInt("Jin_Position");  //각 위치를 읽어옴
            int ypos = PlayerPrefs.GetInt("Yoon_Position");
            PlayerPrefs.SetInt("Jin_Position", ypos);       //읽어온 위치를 각 key에 저장
            PlayerPrefs.SetInt("Yoon_Position", jpos);
        }
    }
    public void ShowResult()    //결과보기 버튼을 눌렀을 경우 실행되는 함수
    {
        noticePanel.gameObject.SetActive(false);    //안내 이미지는 사라짐
        if (typename == "Lucky")    //운수대통인 경우
        {
            if (luckytype[luckyindex] == 1)
            { GetHint(); }          //힌트 획득할 수 있는 운수대통 상황
            else if (luckytype[luckyindex] == 2)
            {                       //민심을 한 개 획득할 수 있는 운수대통 상황
                GetHeart();
            }
            else if (luckytype[luckyindex] == 3)
            {   //상대의 보물을 하나 가져올 수 있는 운수대통 상황
                GetOthersStone();
            }
        }
        else    //새옹지마인 경우
        {
            if (notType == 0)
            {   //힌트를 얻거나 잃을 수 있는 새옹지마 상황
                if (isLost) { LostHint(); }
                else { GetHint(); }
            }
            else if (notType == 1)
            {   //민심을 얻거나 잃을 수 있는 새옹지마 상황
                if (isLost) { LostHeart(); }
                else { GetHeart(); }
            }
        }
    }
    public void GetHeart()  //민심을 획득하는 상황
    {   //현재 민심 개수를 확인하고 1개 증가한 값을 다시 저장함
        int heart = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heart++;
        PlayerPrefs.SetInt(nowplayer + "_Heart", heart);
        heartResult.gameObject.SetActive(true);
    }
    public void LostHeart() //민심을 잃는 상황
    {   //현재 민심 개수를 읽어옴
        int heart = PlayerPrefs.GetInt(nowplayer + "_Heart");
        if (heart > 0)  
        {   //민심의 개수가 0보다 크면 민심을 감소함
            heart--;
            PlayerPrefs.SetInt(nowplayer + "_Heart", heart);
            heartImg.sprite = lostheart;
        }
        else{   //0보다 큰 상황이 아니면 감소하지 않음
            heartImg.gameObject.SetActive(false);
        }
        heartResult.gameObject.SetActive(true);
    }
    public void GetHint()   //힌트를 획득하는 상황
    {   //1~3개 사이의 값을 현재 힌트 개수에 더해서 저장함
        int getHint = Random.Range(1, 4);
        int nowHint = PlayerPrefs.GetInt(nowplayer + "_Hint");
        PlayerPrefs.SetInt(nowplayer + "_Hint", nowHint + getHint);
        for (int i = 0; i < getHint; i++){
            luckyIcon[i].gameObject.SetActive(true);
        }
        hintResult.gameObject.SetActive(true);
    }
    public void LostHint()  //힌트를 잃는 상황
    {   ////1~3개 사이의 값이 줄어들 힌트의 개수
        int lostHint = Random.Range(1, 4);
        int nowHint = PlayerPrefs.GetInt(nowplayer + "_Hint");
        //만약 nowHint - lostHint의 값이 0보다 크면 이 값을 저장함
        if ((nowHint - lostHint) > 0)
        {
            PlayerPrefs.SetInt(nowplayer + "_Hint", nowHint - lostHint);
            for (int i = 0; i < lostHint; i++)
            {   //잃은 힌트의 개수만큼 회색 힌트 아이콘을 띄움
                luckyIcon[i].sprite = losthint;
                luckyIcon[i].gameObject.SetActive(true);
            }
        }
        else
        {   //만약 nowHint - lostHint의 값이 0보다 크지 않으면
            //저장된 값이 음수가 되지 않도록 그냥 0을 저장함
            PlayerPrefs.SetInt(nowplayer + "_Hint", 0);
            for (int i = 0; i < nowHint; i++)
            {   //현재 하트를 모두 잃은 것이므로 이 수만큼 회색 힌트 아이콘을 띄움
                luckyIcon[i].sprite = losthint;
                luckyIcon[i].gameObject.SetActive(true);
            }
        }
        hintResult.gameObject.SetActive(true);
    }
    void GetOthersStone()   //다른 도사의 보물을 가져오는 상황
    {
        List<int> checkindex = new List<int>(); //상대가 보물을 가진 번호를 추가한 리스트
        string otherplayer;
        //현재 플레이어의 이름에 따라 상대 플레이어의 이름을 변경
        if (nowplayer == "Yoon") { otherplayer = "Jin"; }
        else { otherplayer = "Yoon"; }
        for(int i=0; i < 5; i++)
        {   //"Treasure1"~"Treasure5" 중에 상대 이름이 저장된 경우
            //해당 번호를 checkindex 리스트에 추가함
            if (PlayerPrefs.GetString("Treasure" + (i+1)) == otherplayer)
                checkindex.Add(i);
        }
        if (checkindex.Count > 0)
        {   //checkindex의 크기가 0보다 크면 가져올 보물이 있는 것을 의미함
            findStone.SetActive(true);  //보물 이미지 활성화
            int gettype = Random.Range(checkindex[0], checkindex.Count);    //checkindex에 저장된 값 중 하나를 가져올 보물의 번호로 지정
            stoneImg.sprite = stoneType[gettype];   //보물의 이미지를 해당 번호의 이미지로 변경
            PlayerPrefs.SetString("Treasure"+ (gettype+1), nowplayer);  //키에 저장된 이름을 현재 플레이어로 변경
        }
        else { cantstillPanel.gameObject.SetActive(true); } //checkindex에 저장된 값이 없으면 훔칠게 없다는 안내 이미지를 활성화함
    }
    public void ClosePanel(GameObject needclose)
    {   //needclose에 해당하는 게임 오브젝트를 비활성화
        needclose.SetActive(false);
    }
    public void EndLuckyOrNot() //종료 버튼을 누르면 실행될 함수
    {
        PlayerPrefs.SetString("LuckyOrNot", "none");    //"LuckyOrNot" 값을 none으로 변경해서 상황을 종료함
        SceneManager.LoadScene("TreasureMap"); //현 Scene을 다시 로드함
    }
}
