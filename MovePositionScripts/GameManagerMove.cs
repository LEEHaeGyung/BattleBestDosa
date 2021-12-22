using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerMove : MonoBehaviour    //실질적 이동에 사용되는 클래스
{
    public Button moveBtn;      //이동하기 버튼
    public Image foundPos;      //위치이미지를 인식했다는 느낌표 표시
    public Image foundPlayer;   //캐릭터이미지를 인식했다는 느낌표 표시

    bool isfindplayer;          //플레이어를 인식했다는 걸 의미하는 변수
    bool isfindpos;             //위치를 인식했다는 걸 의미하는 변수
    string playerKey;           //플레이어 이름에 해당하는 key를 사용하기 위한 변수
    private void Start()
    {
        playerKey = "None";             //플레이어 이름은 "None"으로 설정
        moveBtn.interactable = false;   //이동하기 버튼을 사용 불가 상태로 설정
        isfindpos = false;              //위치를 인식 못한 상태
        isfindplayer = false;           //아직 캐릭터를 인식 못한 상태
    }
    void Update()
    {   //이전버튼을 누르면 지도 Scene으로 이동
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TreasureMap");
        }
    }
    public void CheckPosition(string _name) //전달받은 이름에 따라 함수를 수행함
    {
        if (_name.Contains("player"))       //이미지의 이름에 player가 있는 경우
        {   //현재 플레이어의 이름을 잃어옴
            string nowplayer = PlayerPrefs.GetString("NowPlayer");
            //현재 플레이어의 이름이 인식된 이미지가 포함된 경우에만 실행
            //다른 플레이어 카드를 올린 경우 수행되지 않도록 함
            if (_name.Contains(nowplayer))
            {
                playerKey = nowplayer + "_Position";
                foundPlayer.gameObject.SetActive(true);
                isfindplayer = true;    //인식상태는 true로 함
            }
        }
        else if (_name.Contains("lucky")) //이미지의 이름에 lucky 있는 경우
        {
            GetLucky(); //운수대통 상황 세팅
        }
        else if (_name.Contains("hitandmiss")) //이미지의 이름에 hitandmiss 있는 경우
        {
            GetHitAndMiss();    //새옹지마 상황 세팅
        }
        else //그 외의 경우는 위치 이미지를 인식한 경우
        {   //이미지 이름의 마지막 글자를 읽어 숫자로 변환함, 위치 인덱스를 의미함
            string isnum = _name.Substring(_name.Length - 1, 1);
            int num;
            if (int.TryParse(isnum, out num))
            {SetPosition(num - 1);} //위치를 세팅함
        }

        if (isfindplayer && isfindpos)
        {   //위치도 인식했고 캐릭터도 인식한 경우 이동하기 버튼 활성화
            moveBtn.interactable = true;
        }
    }
    public void SetPosition(int pos)    //이동 위치를 설정하는 함수
    {
        foundPos.gameObject.SetActive(true);
        isfindpos = true;

        if (playerKey != "None")
        {
            PlayerPrefs.SetInt(playerKey, pos);     //playerKey에 위치를 저장함
            PlayerPrefs.SetInt("NowGame", pos + 1); //"NowGame"의 값을 현재 위치로 변경
        }
    }
    public void GotoMapScene()
    {   //이동하기 버튼을 누른 후 TreasureMap Scene으로 이동함
        SceneManager.LoadScene("TreasureMap");
    }
    public void GetLucky()
    {   //운수대통 상황으로 설정함
        foundPos.gameObject.SetActive(true);
        isfindpos = true;
        PlayerPrefs.SetString("LuckyOrNot", "Lucky");
    }
    public void GetHitAndMiss()
    {   //새옹지마 상황으로 설정함
        foundPos.gameObject.SetActive(true);
        isfindpos = true;
        PlayerPrefs.SetString("LuckyOrNot", "Not");
    }
}
