using UnityEngine;
public class FinishTurn : MonoBehaviour //새로운 차례를 다시 세팅하는 클래스
{
    static public void NewTreasure(string nowplayer)
    {   //보물을 찾은 경우 새로운 보물이미지를 인식하도록 PlayerPrefs의 값을 변경함
        if (PlayerPrefs.GetInt("GameMode") == 2)
        {   //PlayerPrefs.GetInt("GameMode") == 2라면
            //같이하기 모드이므로 차례를 변경해야함
            ChangeTurn(nowplayer);
        }
        PlayerPrefs.SetInt("NowGame", 0);
        PlayerPrefs.SetInt("GameStart", 0);
        PlayerPrefs.SetInt("Yoon_Heart", 0);
        PlayerPrefs.SetInt("Jin_Heart", 0);
    }
    static public void NewGame(string nowplayer)
    {   //한 게임이 종료되면 PlayerPrefs의의 값을 변경함
        if (PlayerPrefs.GetInt("GameMode") == 2)
        {   //PlayerPrefs.GetInt("GameMode") == 2라면
            //같이하기 모드이므로 차례를 변경해야함
            ChangeTurn(nowplayer);
        }
        PlayerPrefs.SetInt("NowGame", 0);
    }
    static void ChangeTurn(string nowplayer)
    {
        //게임 차례를 변경함
        if (nowplayer == "Jin")
            PlayerPrefs.SetString("NowPlayer", "Yoon");
        else
            PlayerPrefs.SetString("NowPlayer", "Jin");
    }
}
