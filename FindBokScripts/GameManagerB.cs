using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerB : MonoBehaviour   //���ָӴ� ã�� ���ӿ� ���Ǵ� Ŭ����
{
    public GameObject noticePanel;  //�ȳ� �г�
    public Image howtoImg;          //���� ��� �ȳ�
    public Image bokhint;           //��Ʈ���� ���ָӴ� �̹���
    public Image itemHint;          //��Ʈ���� ���� �̹���
    public Image hintImg;           //��Ʈ �̹���
    public Image resultImg;         //��� �̹���
    public Image playerImg;         //���� �÷��̾� �̹���
    public Image heart;             //ȹ�� �ν� ������
    public Image checkbok;          //���ָӴϰ� �νĵ� ���¿� ��Ÿ�� ����ǥ ������
    public Image checkanswerImg;    //������� ǥ�� �̹���
    public Image itembubble;        //������ ã�� ������ ��Ÿ�� �̹���
    public Image woman;             //���� �̹���

    public Button startMatchBtn;    //���ǵ����ֱ� ��ư
    public Button checkBtn;         //����Ȯ���ϱ� ��ư
    public Button successBtn;       //�����ֱ⼺�� ��ư
    public Button failBtn;          //�����ֱ���� ��ư
    public Sprite wrong;            //Xǥ�� �̹���
    public Sprite right;            //Oǥ�� �̹���
    public Sprite lostheart;        //�ν��� ���� ��� ��Ʈ ������
    public Sprite[] bokType;        //���ָӴ� �̹��� ����
    public Sprite[] itemType;       //���� �̹��� ����
    public Sprite[] needitems;      //������ ���ϴ� ������ �׷��� ��ǳ�� ����
    public Sprite[] emotion;        //������ ǥ��
    public Text hintText;           //��Ʈ ����

    public int[] bokitem = { 0, 0, 0 }; //���ָӴϿ� ����ִ� ������ �ε���
    public bool isStart = false;        //�����ֱ� ������ ����
    public string checkBokName="None";  //���� �νĵ� ���ָӴ� �̸�
    bool isOpen = false;        //���ӹ�� �̹��� Ȱ��ȭ ���� ����
    bool isFinish = false;      //������ ����� ����
    int hintCount;              //��Ʈ ����
    string nowplayer;           //���� �÷��̾�
    int needitem;               //������ ���ϴ� ������ �ε���
    int getheart;               //�ν��� ���� ������ ȹ���� ������ ����     
    void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");
        hintText.text = hintCount.ToString();
        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg");

        needitem = Random.Range(0, 3);              //������ ã�� ������ ��ȣ�� �������� ����
        itembubble.sprite = needitems[needitem];    //�̿� ���� ��ǳ�� �̹����� ����(����Ȱ��ȭX)
    }
    void Update()
    {
        if (!isFinish)
        {
            if (checkBokName != "None") //���ָӴϰ� �νĵǸ� ����ǥ �������� Ȱ��ȭ
                checkbok.gameObject.SetActive(true);
        }
    }
    public void StartCheckBok()     //���ǵ����ֱ� ����
    {
        isStart = true;
        startMatchBtn.gameObject.SetActive(false);
        checkBtn.gameObject.SetActive(true);
        woman.gameObject.SetActive(true);
    }
    public void StartBtn()
    {
        noticePanel.SetActive(false);
    }
    public void ShowHowToPlay()
    {
        isOpen = !isOpen;
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void UseHint()       //��Ʈ ��� �Լ�
    {
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");
        if (hintCount > 0 && isStart)
        {
            int hintType = Random.Range(0, 3);              //��Ʈ�� ������ �������� ����, �� ������ ã�� ������ ����ִ� ���ָӴϸ� �˷��ִ� ���� �ƴ�
            bokhint.sprite = bokType[hintType];
            itemHint.sprite = itemType[bokitem[hintType]];
            hintImg.gameObject.SetActive(true);
            Invoke("CloseHint", 1.5f);
            hintCount--;
            PlayerPrefs.SetInt(nowplayer + "_Hint", hintCount);
            hintText.text = hintCount.ToString();
        }
    }
    public void CloseHint()
    {
        hintImg.gameObject.SetActive(false);
    }
    public void CheckBok()  //�´� ���ָӴ����� Ȯ���ϴ� �Լ�, ����Ȯ���ϱ� ��ư�� ������ ����
    {
        int checkindex = 0;
        if (checkBokName != "None")     //���ָӴ��� �̸��� None �ƴ� ���� ����
        {
            if (checkBokName.Contains("1")) { checkindex = 0; }        //���ָӴ��� ��ȣ Ȯ��
            else if (checkBokName.Contains("2")) { checkindex = 1; }
            else if (checkBokName.Contains("3")) { checkindex = 2; }
            if (bokitem[checkindex] == needitem)        //���ָӴ� ��ȣ�� �ش��ϴ� bokitem�� ���� ������ ã�� ������ ��ȣ�� ������ ����
            {
                successBtn.gameObject.SetActive(true);
                woman.sprite = emotion[1];          //������ ǥ�� ����
                checkanswerImg.sprite = right;
            }
            else
            {
                failBtn.gameObject.SetActive(true);
                woman.sprite = emotion[0];          //������ ǥ�� ����
                checkanswerImg.sprite = wrong;
            }
            checkBtn.gameObject.SetActive(false);
            checkanswerImg.gameObject.SetActive(true);
        }
    }
    public void SuccessBtn()        //�����ֱ⼺�� ��ư�� ������ ����
    {  
        getheart = 1;   //�ν��� 1�� ���� �� �ǹ�
        successBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    void FinishGame()
    {
        if (getheart < 0) { heart.sprite = lostheart; } //getheart�� �������� ��Ʈ�� �Ҵ� ��Ȳ�̸� ��Ʈ �������� ȸ�� ���������� ����
        resultImg.gameObject.SetActive(true);
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + getheart;             //getheart�� ���� ���� �ν��� ������ ����
        if (heartcount > 0) { PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount); }
        else { PlayerPrefs.SetInt(nowplayer + "_Heart", 0); }
        FinishTurn.NewGame(nowplayer);
    }
    public void FailBtn()       //�����ֱ���� ��ư�� ������ ����
    {
        getheart = -1;  //�ν��� 1�� ���� �� �ǹ�
        failBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("TreasureMap");
    }
}
