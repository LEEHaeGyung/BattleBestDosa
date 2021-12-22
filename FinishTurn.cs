using UnityEngine;
public class FinishTurn : MonoBehaviour //���ο� ���ʸ� �ٽ� �����ϴ� Ŭ����
{
    static public void NewTreasure(string nowplayer)
    {   //������ ã�� ��� ���ο� �����̹����� �ν��ϵ��� PlayerPrefs�� ���� ������
        if (PlayerPrefs.GetInt("GameMode") == 2)
        {   //PlayerPrefs.GetInt("GameMode") == 2���
            //�����ϱ� ����̹Ƿ� ���ʸ� �����ؾ���
            ChangeTurn(nowplayer);
        }
        PlayerPrefs.SetInt("NowGame", 0);
        PlayerPrefs.SetInt("GameStart", 0);
        PlayerPrefs.SetInt("Yoon_Heart", 0);
        PlayerPrefs.SetInt("Jin_Heart", 0);
    }
    static public void NewGame(string nowplayer)
    {   //�� ������ ����Ǹ� PlayerPrefs���� ���� ������
        if (PlayerPrefs.GetInt("GameMode") == 2)
        {   //PlayerPrefs.GetInt("GameMode") == 2���
            //�����ϱ� ����̹Ƿ� ���ʸ� �����ؾ���
            ChangeTurn(nowplayer);
        }
        PlayerPrefs.SetInt("NowGame", 0);
    }
    static void ChangeTurn(string nowplayer)
    {
        //���� ���ʸ� ������
        if (nowplayer == "Jin")
            PlayerPrefs.SetString("NowPlayer", "Yoon");
        else
            PlayerPrefs.SetString("NowPlayer", "Jin");
    }
}
