using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManagerF : MonoBehaviour   //��깰 �������� ���ӿ� ���� Ŭ����
{
    public GameObject[] radishOrCabbage;    //���� �Ǵ� ���� �� ������Ʈ
    public Material[] farmProduceType;      //���� �Ǵ� �� ����
    public GameObject noticePanel;  //�ȳ� �г�
    public GameObject inputPanel;   //�����Է� �г�
    public Image howtoImg;          //���� ��� �ȳ�
    public Image resultImg;         //��� �̹���
    public Image playerImg;         //���� �÷��̾� �̹���
    public Image[] heart;           //ȹ�� �ν� ������
    public Image checkanswerImg;    //������� ǥ�� �̹���
    public Image farmer;            //��� �̹���
    public Text timerText;          //Ÿ�̸� �ؽ�Ʈ
    public InputField radishInput;  //���� ���� �Է��ʵ�
    public InputField cabbageInput; //������ ���� �Է��ʵ�

    public Button checkBtn;         //��������Ϸ� ��ư
    public Button successBtn;       //�����ֱ⼺�� ��ư
    public Button failBtn;          //�����ֱ���� ��ư
    public Sprite wrong;            //Xǥ�� �̹���
    public Sprite right;            //Oǥ�� �̹���
    public Sprite[] emotion;        //��� ǥ��

    bool isOpen = false;        //���ӹ�� �̹��� Ȱ��ȭ ���� ����
    bool isPaused = false;      //Ÿ�̸� ���� ����
    bool isStart = false;       //���� ���� ����
    float starttime;            //���� �ð�
    float timeremain;           //���� �ð�
    string nowplayer;           //���� �÷��̾�
    int radishCount=0;          //���� ����
    int cabbageCount=0;         //������ ����
    int getheart;               //���� ��Ʈ�� ����   
    void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg");

        for(int i=0; i < radishOrCabbage.Length; i++)
        {   //type�� ���� 0�̳� 1 �� �ϳ��� ����
            int type = Random.Range(0, 2);
            //���߶Ǵ¹��� �� ������Ʈ�� material�� farmProduceType �� type�� �ε����� ����
            radishOrCabbage[i].GetComponent<MeshRenderer>().material = farmProduceType[type];
            //type==0 : ���� type==1 : ��, type�� ���� ���� �� ���� ���� ����
            if (type == 0) { cabbageCount++; }
            else { radishCount++; }
        }
    }
    void Update()
    {   //Ÿ�̸Ӱ� ������� �ʾҰ� ������ ���۵ƴٸ� Ÿ�̸� ����
        if (!isPaused && isStart) { CheckTimer(); }
    }
    void CheckTimer()
    {   //���� �ð����� Time.time�� ���� ���� �ð� ���
        timeremain = starttime - Time.time;
        if (timeremain <=0)
        {   //���� �ð��� 0�̵Ǹ� isPaused�� �����ϰ� ���� �Է� �г��� Ȱ��ȭ��
            timeremain = 0;
            isPaused = true;
            inputPanel.SetActive(true);
        }
        timerText.text = ((int)timeremain).ToString();
    }
    public void StartBtn()  //�����ַ����� ��ư�� ������ ����
    {
        noticePanel.SetActive(false);
        //isStart�� ���� ����
        isStart = true;
        //���� �ð��� �ʱ�ȭ��
        starttime = 10f + Time.time;
    }
    public void ShowHowToPlay() //���� ��� �ȳ� �г�
    {
        isOpen = !isOpen;
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void CheckCount()    //��������Ϸ� ��ư�� ������ ����
    {                           //�Էµ� ���� �������� Ȯ��
        //�� �Է� �ʵ忡�� �Էµ� ������ ������ �о��
        int radishinput = int.Parse(radishInput.text);  
        int cabbageinput = int.Parse(cabbageInput.text);
        //�Էµ� ���� ������ ������ Start()���� ������ ���� ������ ������ ���ٸ� ����
        if (radishinput == radishCount && cabbageinput == cabbageCount)
        {   //Oǥ�ø� ��Ÿ���� ����� ǥ���� ���� �󱼷� ����
            checkanswerImg.sprite = right;
            farmer.sprite = emotion[0];
            successBtn.gameObject.SetActive(true);
        }
        else
        {   //Xǥ�ø� ��Ÿ���� ����� ǥ���� ȭ�� �󱼷� ����
            checkanswerImg.sprite = wrong;
            farmer.sprite = emotion[1];
            failBtn.gameObject.SetActive(true);
        }
        checkBtn.gameObject.SetActive(false);
        checkanswerImg.gameObject.SetActive(true);
    }
    public void SuccessBtn()        //�����ֱ⼺�� ��ư�� ������ ����
    {
        getheart = 2;   //�ν��� 1�� ���� �� �ǹ�
        successBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    void FinishGame()
    {
        for(int i=0; i < getheart; i++) { heart[i].gameObject.SetActive(true); }
        resultImg.gameObject.SetActive(true);
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + getheart;             //getheart�� ���� ���� �ν��� ������ ����
        if (heartcount > 0) { PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount); }
        else { PlayerPrefs.SetInt(nowplayer + "_Heart", 0); }
        FinishTurn.NewGame(nowplayer);
    }
    public void FailBtn()       //�����ֱ���� ��ư�� ������ ����
    {
        getheart = 0;
        failBtn.gameObject.SetActive(false);
        Invoke("FinishGame", 1f);
    }
    public void EndGame()
    {
        SceneManager.LoadScene("TreasureMap");
    }
}
