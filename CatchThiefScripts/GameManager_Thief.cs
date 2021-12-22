using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager_Thief : MonoBehaviour
{
    public GameObject noticePanel; //게임 안내 이미지
    public Image howtoImg;          //게임 방법 안내 이미지
    public Image thiefHint;         //힌트 이미지 중 도둑이미지
    public Image itemHint;          //힌트 이미지 중 물건이미지
    public Image hintImg;           //도둑이미지와 물건이미지를 포함하는 부모 이미지
    public Image resultImg;         //민심 획득 결과 아미지
    public Image playerImg;         //플레이어 이미지
    public Image findthief;         //도둑이미지가 인식된 경우 느낌표 이미지
    public Image findperson;        //주민이미지가 인식된 경우 느낌표 이미지
    public Image checkanswerImg;    //정답와 오답 표시 이미지
    public Image[] heart;           //정답을 맞출때마다 증가하는 하트 아이콘
    public Image[] resulteheart;    //결과 이미지에 나타날 하트 아이콘
    public Button startMatchBtn;    //물건돌려주기 버튼
    public Button checkBtn;         //물건확인하기 버튼
    public Button successBtn;       //도와주기성공 버튼
    public Button failBtn;          //도와주기실패 버튼
    public Sprite wrong;            //오답 표시
    public Sprite right;            //정답 표시
    public Sprite[] thiefType;      //도둑 이미지 종류, 힌트에 사용
    public Sprite[] itemType;       //물건 이미지 종류, 힌트에 사용
    public Text hintText;           //힌트 개수

    public bool[] isReturn = { false, false, false };   //주민이 물건을 돌려받았는지 확인하는 bool 배열, 각 주민이 물건을 돌려받을 때마다 true로 변경됨
    public int[] thiefitemIndex = { 0, 0, 0 };  //도둑이 가지고 있는 물건의 번호
    public int[] personitemIndex = { 0, 0, 0 }; //주민이 잃어버린 물건의 번호
    public int thiefNow;    //현재 인식된 도둑의 번호
    public int personNow;   //현재 인식된 주민의 번호
    public bool isStart = false;    //물건돌려주기가 시작됐는지 여부, true가 되면 도둑이 가지고 있는 물건은 더이상 나타나지 않음
    bool isOpen = false;    //힌트 이미지 활성화를 결정하는 변수
    int hintCount;          //힌트의 개수
    string nowplayer;       //현재 플레이어
    int MatchCount=0;       //물건돌려주기를 성공한 횟수
    int matchIndex = 0;     //물건돌려주기를 진행한 횟수
    void Start()
    {
        thiefNow = -1;      //시작엔 도둑이나 주민이 인식되지 않은 상태이므로
        personNow = -1;     //thiefNow와 personNow의 값은 -1로 설정
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");    //현재 플레이어의 힌트 개수
        hintText.text = hintCount.ToString();

        isStart = false;    //물건돌려주기를 아직 시작 안 한 상태

        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg"); //시작화면에 현재 플레이어의 이미지가 나타남
    }
    void Update()
    {
        if (isStart)            //물건돌려주기를 시작하면 도둑이나 사람 이미지가 인식되면 느낌표가 나타나도록 함
        {
            if (matchIndex < 3) //게임 수행이 3번보다 커지면 게임 종료이므로 느낌표가 더 나타나지 않도록 함
            {
                if (personNow != -1)    //-1이 아니면 주민 이미지 인식상태
                    findperson.gameObject.SetActive(true);
                if (thiefNow != -1)     //-1이 아니면 도둑 이미지 인식상태
                    findthief.gameObject.SetActive(true);
            }
        }
    }
    public void StartMatchPerson()  //물건돌려주기 버튼을 누르면 실행되는 함수
    {
        isStart = true; 
        startMatchBtn.gameObject.SetActive(false);  //돌려주기버튼 비활성화
        //도둑이미지를 인식했을 때 나타난 오브젝트 모두 비활성화
        GameObject.Find("Thief AR Setting").GetComponent<ThiefFind>().AllSetFalse();
        checkBtn.gameObject.SetActive(true);        //물건확인하기 버튼 활성화
    }
    public void StartBtn()          //게임 안내 패널에 있는 도와주러가기 버튼을 누르면 실행
    {
        noticePanel.SetActive(false);//안내 패널을 비활성화 함
    }
    public void ShowHowToPlay()     //안내 패널에서 ? 버튼을 누르면 실행
    {
        isOpen = !isOpen;           //isOpen을 반전하며 게임 방법 이미지를 활성화 또는 비활성화
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void UseHint()   //힌트 버튼을 누르면 실행되는 함수
    {   //힌트의 개수를 읽어옴
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");
        if (hintCount > 0&& isStart)            //힌트의 개수가 0 이상이고 게임을 시작한 경우에만 실행
        {
            int hintType = Random.Range(0, 3);
            thiefHint.sprite = thiefType[hintType]; //hintType의 값에 따라 힌트의 도둑 이미지를 나타냄
            itemHint.sprite = itemType[thiefitemIndex[hintType]];   //hintType에 해당하는 도둑의 물건 이미지를 나타냄
            hintImg.gameObject.SetActive(true);
            Invoke("CloseHint", 1.5f);  //힌트는 1.5초 후에 사라짐
            hintCount--;                //힌트의 개수를 감소하고 다시 key에 값을 저장
            PlayerPrefs.SetInt(nowplayer + "_Hint", hintCount);
            hintText.text = hintCount.ToString();   //힌트 개수 텍스트 변경
        }
    }
    public void CloseHint() //힌트 이미지를 닫는 함수
    {
        hintImg.gameObject.SetActive(false);
    }
    public void CheckMatch()    
    {
        if (!isReturn[personNow]
            &&(thiefNow != -1&&personNow != -1)) 
        {
            checkBtn.gameObject.SetActive(false);
            if (personitemIndex[personNow] 
                == thiefitemIndex[thiefNow]) 
            {
                successBtn.gameObject.SetActive(true);
                isReturn[personNow] = true;
                checkanswerImg.sprite = right;
            }
            else  
            {
                failBtn.gameObject.SetActive(true);
                checkanswerImg.sprite = wrong;
            }
            checkanswerImg.gameObject.SetActive(true); 
        }
        else { resetNotice(); }
    }
    public void SuccessBtn()        //정답일 경우 나타나는 버튼을 눌렀을 때 실행
    {
        heart[MatchCount].gameObject.SetActive(true);   //MatchCount==0 : 처음 문제를 맞춤 / MatchCount==1 : 2번째 문제를 맞춤 / MatchCount==3 : 3번째 문제를 맞춤
        MatchCount++;
        successBtn.gameObject.SetActive(false);
        matchIndex++;                   //정답확인 횟수 증가
        if (matchIndex >= 3)            //3이 되면 3번 모두 확인해본 것을 의미함
            Invoke("FinishGame", 1f);   //게임 종료 함수를 실행
        else                            //3보다 작은 경우 물건확인하기 버튼을 활성화 후 게임을 계속 진행
        {
            checkBtn.gameObject.SetActive(true);
        }
        resetNotice();
    }
    void FinishGame()           //게임 종료 함수
    {
        resultImg.gameObject.SetActive(true);           //민심 획득 결과를 보여줌
        for(int i=0; i<MatchCount; i++)                 //정답을 맞춘 횟수만큼 하트 아이콘 활성화
        {
            resulteheart[i].gameObject.SetActive(true);
        }
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + MatchCount;           //획득한 민심과 현재 민심을 더해서 key에 저장
        PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount);
        FinishTurn.NewGame(nowplayer);                  //차례를 변경함
    }
    public void FailBtn()   //오답일 경우 나타나는 버튼을 눌렀을 때 실행
    {
        failBtn.gameObject.SetActive(false);
        matchIndex++;                   //정답 확인 횟수만 증가함
        if (matchIndex >= 3)
            Invoke("FinishGame", 1f);
        else
        {
            checkBtn.gameObject.SetActive(true);
        }
        resetNotice();
    }
    public void EndGame()   //결과 이미지에 포함된 종료버튼을 누르면 실행
    {
        SceneManager.LoadScene("TreasureMap");  //다시 지도 Scene으로 이동
    }
    void resetNotice()      //물건확인하기 후 느낌표 표시는 제거하고 주민과 도둑 인식 상태 초기화
    {
        findperson.gameObject.SetActive(false);
        findthief.gameObject.SetActive(false);
        checkanswerImg.gameObject.SetActive(false);
        thiefNow = -1;
        personNow = -1;
        //ThiefFind에 있는 함수를 호출해 웃는 주민 얼굴을 모두 비활성화 해둠
        GameObject.Find("Thief AR Setting").GetComponent<ThiefFind>().PersonAllSetFalse();
    }
}