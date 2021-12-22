using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerDD : MonoBehaviour  //���� �������� ���ӿ� ���Ǵ� Ŭ����
{
    public GameObject noticePanel;      //���� �ȳ� �г�
    public GameObject outPanel;         //������ ������ �г�
    public Image howtoImg;              //���� ��� �ȳ� �̹���
    public Image resultImg;             //��� �̹���
    public Image playerImg;             //�÷��̾� �̹���
    public Image heart;                 //�ν� ������
    public Image textbubble;            //��� ��ǳ��
    public Image doryung;               //���� �̹���
    public Image resultbubble;          //������ ��� �ٸ� ����� ���� ��ǳ��

    public Button successBtn;           //�����ֱ⼺�� ��ư
    public Button failBtn;              //�����ֱ���� ��ư
    public Sprite lostheart;            //�ν��� ���� ��� ������
    public Sprite[] dressType;          //������ �� ����
    public Sprite[] requestBubble;      //������ ��� �̹�����
    public Sprite[] resultText;         //��� ����� ����

    bool isOpen = false;                //���� ����� Ȱ��ȭ ����
    string nowplayer;                   //���� �÷��̾�
    int needdress;                      //�ʿ��� ���� �ε���
    int getheart;                       //��Ʈ�� �Ҵ��� ����� ����
    int nowDress=-1;                    //���� ���� ���� �ε���
    void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg");

        needdress = Random.Range(0, 3);             //�ʿ��� ���� ������ �������� ����
        textbubble.sprite = requestBubble[needdress];//�ʿ��� �ʿ� ���� ������ �䱸 ��� ����
    }
    public void LetsGoOut()     //�����ϱ� ��ư�� ������ �� ����Ǵ� �Լ�
    {   //nowDress�� -1�̶�� ���� �ƹ� �ʵ� ���� ���� �����̹Ƿ� �������� ����
        if (nowDress != -1)     
        {
            outPanel.SetActive(true);   //������� �г� Ȱ��ȭ
            if (needdress == nowDress)  //�ʿ��� �ʰ� ���� ���� ���� �ε����� ������ ����
            {
                resultbubble.sprite = resultText[needdress];    //������� ����
                successBtn.gameObject.SetActive(true);
            }
            else
            {
                failBtn.gameObject.SetActive(true);
            }
        }
    }
    public void StartBtn()      //�����ַ����� ��ư ������ ����
    {
        noticePanel.SetActive(false);
    }
    public void ShowHowToPlay() //���ӹ�� Ȯ��
    {
        isOpen = !isOpen;
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void SuccessBtn()    //�����ֱ⼺�� ��ư ������ ����
    {
        getheart = 1;           //�ν��� ����
        successBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    void FinishGame()
    {
        if (getheart < 0) { heart.sprite = lostheart; }
        resultImg.gameObject.SetActive(true);
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + getheart;
        if (heartcount > 0) { PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount); }
        else { PlayerPrefs.SetInt(nowplayer + "_Heart", 0); }
        FinishTurn.NewGame(nowplayer);
    }
    public void FailBtn()   //�����ֱ���� ��ư ������ ����
    {
        getheart = -1;      //�ν��� ����
        failBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("TreasureMap");
    }
    public void SetDress(int type)          //�� �̹����� �νĵ� ��� ����Ǵ� �Լ�
    {
        doryung.sprite = dressType[type];   //���� ���� ���� ������ ���� �����̹��� ����
        nowDress = type;                    //nowDress�� ���� ����
    }
}
