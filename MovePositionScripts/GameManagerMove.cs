using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerMove : MonoBehaviour    //������ �̵��� ���Ǵ� Ŭ����
{
    public Button moveBtn;      //�̵��ϱ� ��ư
    public Image foundPos;      //��ġ�̹����� �ν��ߴٴ� ����ǥ ǥ��
    public Image foundPlayer;   //ĳ�����̹����� �ν��ߴٴ� ����ǥ ǥ��

    bool isfindplayer;          //�÷��̾ �ν��ߴٴ� �� �ǹ��ϴ� ����
    bool isfindpos;             //��ġ�� �ν��ߴٴ� �� �ǹ��ϴ� ����
    string playerKey;           //�÷��̾� �̸��� �ش��ϴ� key�� ����ϱ� ���� ����
    private void Start()
    {
        playerKey = "None";             //�÷��̾� �̸��� "None"���� ����
        moveBtn.interactable = false;   //�̵��ϱ� ��ư�� ��� �Ұ� ���·� ����
        isfindpos = false;              //��ġ�� �ν� ���� ����
        isfindplayer = false;           //���� ĳ���͸� �ν� ���� ����
    }
    void Update()
    {   //������ư�� ������ ���� Scene���� �̵�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TreasureMap");
        }
    }
    public void CheckPosition(string _name) //���޹��� �̸��� ���� �Լ��� ������
    {
        if (_name.Contains("player"))       //�̹����� �̸��� player�� �ִ� ���
        {   //���� �÷��̾��� �̸��� �Ҿ��
            string nowplayer = PlayerPrefs.GetString("NowPlayer");
            //���� �÷��̾��� �̸��� �νĵ� �̹����� ���Ե� ��쿡�� ����
            //�ٸ� �÷��̾� ī�带 �ø� ��� ������� �ʵ��� ��
            if (_name.Contains(nowplayer))
            {
                playerKey = nowplayer + "_Position";
                foundPlayer.gameObject.SetActive(true);
                isfindplayer = true;    //�νĻ��´� true�� ��
            }
        }
        else if (_name.Contains("lucky")) //�̹����� �̸��� lucky �ִ� ���
        {
            GetLucky(); //������� ��Ȳ ����
        }
        else if (_name.Contains("hitandmiss")) //�̹����� �̸��� hitandmiss �ִ� ���
        {
            GetHitAndMiss();    //�������� ��Ȳ ����
        }
        else //�� ���� ���� ��ġ �̹����� �ν��� ���
        {   //�̹��� �̸��� ������ ���ڸ� �о� ���ڷ� ��ȯ��, ��ġ �ε����� �ǹ���
            string isnum = _name.Substring(_name.Length - 1, 1);
            int num;
            if (int.TryParse(isnum, out num))
            {SetPosition(num - 1);} //��ġ�� ������
        }

        if (isfindplayer && isfindpos)
        {   //��ġ�� �ν��߰� ĳ���͵� �ν��� ��� �̵��ϱ� ��ư Ȱ��ȭ
            moveBtn.interactable = true;
        }
    }
    public void SetPosition(int pos)    //�̵� ��ġ�� �����ϴ� �Լ�
    {
        foundPos.gameObject.SetActive(true);
        isfindpos = true;

        if (playerKey != "None")
        {
            PlayerPrefs.SetInt(playerKey, pos);     //playerKey�� ��ġ�� ������
            PlayerPrefs.SetInt("NowGame", pos + 1); //"NowGame"�� ���� ���� ��ġ�� ����
        }
    }
    public void GotoMapScene()
    {   //�̵��ϱ� ��ư�� ���� �� TreasureMap Scene���� �̵���
        SceneManager.LoadScene("TreasureMap");
    }
    public void GetLucky()
    {   //������� ��Ȳ���� ������
        foundPos.gameObject.SetActive(true);
        isfindpos = true;
        PlayerPrefs.SetString("LuckyOrNot", "Lucky");
    }
    public void GetHitAndMiss()
    {   //�������� ��Ȳ���� ������
        foundPos.gameObject.SetActive(true);
        isfindpos = true;
        PlayerPrefs.SetString("LuckyOrNot", "Not");
    }
}
