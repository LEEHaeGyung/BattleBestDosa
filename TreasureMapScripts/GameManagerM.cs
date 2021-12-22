using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerM : MonoBehaviour   //TreasureMap Scene���� ���� ���¸� �����ϴ� Ŭ����
{
    public Image[] playerPos;   //�÷��̾��� ��ġ �̹���
    public Button moveBtn;      //�̵��ϱ� ��ư
    public Button gameBtn;      //���ӽ����ϱ� ��ư
    public Text hintText;       //��Ʈ ���� �ؽ�Ʈ
    public Image jinIcon;       //�� ��ġ�� ���� ĳ���� ������
    public Image yoonIcon;      //�� ��ġ�� ���� ĳ���� ������
    public Sprite jinPos;       //�� ��ġ�� ǥ���ϴ� �̹���
    public Sprite yoonPos;      //�� ��ġ�� ǥ���ϴ� �̹���

    string nowplayer;           //���� �÷��̾� �̸�
    int hintCount;              //��Ʈ�� ����
    private void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");         //���� �÷��̾��� �̸��� �о��
        //"Jin_Hint", "Yoon_Hint" : ���� ���� ������ �ִ� ��Ʈ ����
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");    //���� �÷��̾��� ��Ʈ ������ �о��
        hintText.text= hintCount.ToString();                    //��Ʈ �ؽ�Ʈ�� ��Ʈ ������ ����

        if (PlayerPrefs.GetInt("GameMode") == 2) { ModeMultiSet(); }    //���� ��尡 2�϶� ����
        else
        {   //���� ��尡 2�� �ƴҶ� ����, ���� �÷��̾ �ش��ϴ� �����ܰ� ��ġ �̹����� �Ű������� ����
            if (nowplayer == "Jin") { ModeSingleSet(jinIcon, jinPos); }
            else { ModeSingleSet(yoonIcon, yoonPos); }
        }
        //""NowGame" : ���� �������� ������ ��ȣ, 0�̸� ���� �ƹ� ���ӵ� �������� �ʾ����� �ǹ�
        //���� �̵��ϱ� ��ư�� Ȱ��ȭ��
        if (PlayerPrefs.GetInt("NowGame") == 0) { moveBtn.gameObject.SetActive(true); }
        else { gameBtn.gameObject.SetActive(true); }    //0�� �ƴ϶�� ���ӽ����ϱ� ��ư Ȱ��ȭ
    }
    void ModeSingleSet(Image icon, Sprite posImg)
    {   //ȥ���ϱ� ����� ��� ����, ���� �÷��̾��� ��ġ�� �о �������� ��ġ�� ��ġ �̹����� ����
        int pos = PlayerPrefs.GetInt(nowplayer+"_Position");
        playerPos[pos].sprite = posImg;
        icon.transform.position = playerPos[pos].transform.position;
        icon.gameObject.SetActive(true);    //�ش��ϴ� �����ܸ� Ȱ��ȭ
    }
    void ModeMultiSet()
    {   //�����ϱ� ����� ��� ����
        Vector2 nowsize = new Vector2(220, 265);        //���� �÷��̾��� ������ ũ��
        Vector2 notnowsize = new Vector2(165, 198);     //���ʰ� �ƴ� �÷��̾��� ������ ũ��

        int jpos = PlayerPrefs.GetInt("Jin_Position");  //���� ��ġ�� �о��
        int ypos = PlayerPrefs.GetInt("Yoon_Position"); //���� ��ġ�� �о��

        jinIcon.gameObject.SetActive(true);             //�� �������� Ȱ��ȭ
        yoonIcon.gameObject.SetActive(true);

        if (jpos == ypos)   //�� �÷��̾��� ��ġ�� ���� ��� 
        {                   //��ġ �̹����� ���� �÷��̾��� �̹����� ������
            if (nowplayer == "Yoon")
            {
                playerPos[ypos].sprite = yoonPos;
                yoonIcon.transform.SetAsLastSibling();  //���� �÷��̾ �ش��ϴ� �̹����� �տ� ������ ��
            }
            else
            {
                playerPos[jpos].sprite = jinPos;        //���� �÷��̾ ���� ��쵵 ���� ������� ����
                jinIcon.transform.SetAsLastSibling();
            }
        }
        else
        {   //��ġ�� �ٸ��ٸ� �� ��ġ�� �̹����� ������
            playerPos[jpos].sprite = jinPos;
            playerPos[ypos].sprite = yoonPos;
        }
        jinIcon.transform.position = playerPos[jpos].transform.position;    //�������� ��ġ�� ���� ��ġ�� �̵�
        yoonIcon.transform.position = playerPos[ypos].transform.position;

        //���� �÷��̾ �����Ŀ� ���� ������ ũ�⸦ ����, ���ʰ� �ƴ� �������� �۰� ������
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
    public void MovePosition()                  //�̵��ϱ� ��ư�� ������ ��� ����Ǵ� �Լ�
    {
        SceneManager.LoadScene("MovePosition"); //��ġ�� �̵��ϴ� Scene���� �̵�
    }
    public void GotoGame()                      //���ӽ����ϱ� ��ư�� ������ ��� ����Ǵ� �Լ�
    {
        int playerPos = PlayerPrefs.GetInt(nowplayer+"_Position");  //�÷��̾��� ���� ��ġ�� �о��
        //���� ��ġ�� ������ ��ġ��� ������ ȹ���ϴ� Scene���� �̵�
        if (PlayerPrefs.GetInt("GoalPos") == playerPos) { SceneManager.LoadScene("GetTreasure"); }
        else
        {   //���� ��ġ�� �ƴ϶�� "NowGame" ������ ���� ���� "Game1"~"Game9" �� �ϳ��� key�� ��
            string gameindex = "Game" + PlayerPrefs.GetInt("NowGame");
            //key�� �ش��ϴ� ���� Scene�� �̸��� gamename�� �����ϰ� �ش� Scene�� �ε���
            string gamename = PlayerPrefs.GetString(gameindex);
            SceneManager.LoadScene(gamename);
        }
    }
}
