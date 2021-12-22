using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerM : MonoBehaviour   //TreasureMap Scene에서 현재 상태를 관리하는 클래스
{
    public Image[] playerPos;   //플레이어의 위치 이미지
    public Button moveBtn;      //이동하기 버튼
    public Button gameBtn;      //게임시작하기 버튼
    public Text hintText;       //힌트 갯수 텍스트
    public Image jinIcon;       //진 위치에 있을 캐릭터 아이콘
    public Image yoonIcon;      //윤 위치에 있을 캐릭터 아이콘
    public Sprite jinPos;       //진 위치를 표시하는 이미지
    public Sprite yoonPos;      //윤 위치를 표시하는 이미지

    string nowplayer;           //현재 플레이어 이름
    int hintCount;              //힌트의 개수
    private void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");         //현재 플레이어의 이름을 읽어옴
        //"Jin_Hint", "Yoon_Hint" : 진과 윤이 가지고 있는 힌트 개수
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");    //현재 플레이어의 힌트 개수를 읽어옴
        hintText.text= hintCount.ToString();                    //힌트 텍스트를 힌트 개수로 변경

        if (PlayerPrefs.GetInt("GameMode") == 2) { ModeMultiSet(); }    //게임 모드가 2일때 설정
        else
        {   //게임 모드가 2가 아닐때 설졍, 현재 플레이어에 해당하는 아이콘과 위치 이미지를 매개변수로 전달
            if (nowplayer == "Jin") { ModeSingleSet(jinIcon, jinPos); }
            else { ModeSingleSet(yoonIcon, yoonPos); }
        }
        //""NowGame" : 현재 실행중인 게임의 번호, 0이면 아직 아무 게임도 시작하지 않았음을 의미
        //따라서 이동하기 버튼을 활성화함
        if (PlayerPrefs.GetInt("NowGame") == 0) { moveBtn.gameObject.SetActive(true); }
        else { gameBtn.gameObject.SetActive(true); }    //0이 아니라면 게임시작하기 버튼 활성화
    }
    void ModeSingleSet(Image icon, Sprite posImg)
    {   //혼자하기 모드일 경우 설정, 현재 플레이어의 위치만 읽어서 아이콘의 위치와 위치 이미지를 변경
        int pos = PlayerPrefs.GetInt(nowplayer+"_Position");
        playerPos[pos].sprite = posImg;
        icon.transform.position = playerPos[pos].transform.position;
        icon.gameObject.SetActive(true);    //해당하는 아이콘만 활성화
    }
    void ModeMultiSet()
    {   //같이하기 모드일 경우 설정
        Vector2 nowsize = new Vector2(220, 265);        //현재 플레이어의 아이콘 크기
        Vector2 notnowsize = new Vector2(165, 198);     //차례가 아닌 플레이어의 아이콘 크기

        int jpos = PlayerPrefs.GetInt("Jin_Position");  //진의 위치를 읽어옴
        int ypos = PlayerPrefs.GetInt("Yoon_Position"); //윤의 위치를 읽어옴

        jinIcon.gameObject.SetActive(true);             //각 아이콘을 활성화
        yoonIcon.gameObject.SetActive(true);

        if (jpos == ypos)   //두 플레이어의 위치가 같을 경우 
        {                   //위치 이미지를 현재 플레이어의 이미지로 변경함
            if (nowplayer == "Yoon")
            {
                playerPos[ypos].sprite = yoonPos;
                yoonIcon.transform.SetAsLastSibling();  //현재 플레이어에 해당하는 이미지가 앞에 오도록 함
            }
            else
            {
                playerPos[jpos].sprite = jinPos;        //현재 플레이어가 진을 경우도 같은 방식으로 수행
                jinIcon.transform.SetAsLastSibling();
            }
        }
        else
        {   //위치가 다르다면 각 위치의 이미지를 변경함
            playerPos[jpos].sprite = jinPos;
            playerPos[ypos].sprite = yoonPos;
        }
        jinIcon.transform.position = playerPos[jpos].transform.position;    //아이콘의 위치를 현재 위치로 이동
        yoonIcon.transform.position = playerPos[ypos].transform.position;

        //현재 플레이어가 누구냐에 따라 아이콘 크기를 변경, 차례가 아닌 아이콘은 작게 변경함
        if (nowplayer == "Yoon")
        {
            yoonIcon.transform.transform.gameObject.GetComponent<RectTransform>().sizeDelta = nowsize;
            jinIcon.transform.transform.gameObject.GetComponent<RectTransform>().sizeDelta = notnowsize;
        }
        else
        {
            jinIcon.transform.transform.gameObject.GetComponent<RectTransform>().sizeDelta = nowsize;
            yoonIcon.transform.transform.gameObject.GetComponent<RectTransform>().sizeDelta = notnowsize;
        }
    }
    public void MovePosition()                  //이동하기 버튼을 눌렀을 경우 실행되는 함수
    {
        SceneManager.LoadScene("MovePosition"); //위치를 이동하는 Scene으로 이동
    }
    public void GotoGame()                      //게임시작하기 버튼을 눌렀을 경우 실행되는 함수
    {
        int playerPos = PlayerPrefs.GetInt(nowplayer+"_Position");  //플레이어의 현재 위치를 읽어옴
        //현재 위치가 보물의 위치라면 보물을 획득하는 Scene으로 이동
        if (PlayerPrefs.GetInt("GoalPos") == playerPos) { SceneManager.LoadScene("GetTreasure"); }
        else
        {   //보물 위치가 아니라면 "NowGame" 게임의 값에 따라 "Game1"~"Game9" 중 하나가 key가 됨
            string gameindex = "Game" + PlayerPrefs.GetInt("NowGame");
            //key에 해당하는 게임 Scene의 이름을 gamename에 저장하고 해당 Scene을 로드함
            string gamename = PlayerPrefs.GetString(gameindex);
            SceneManager.LoadScene(gamename);
        }
    }
}
