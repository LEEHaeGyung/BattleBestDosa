using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager_Thief : MonoBehaviour
{
    public GameObject noticePanel; //���� �ȳ� �̹���
    public Image howtoImg;          //���� ��� �ȳ� �̹���
    public Image thiefHint;         //��Ʈ �̹��� �� �����̹���
    public Image itemHint;          //��Ʈ �̹��� �� �����̹���
    public Image hintImg;           //�����̹����� �����̹����� �����ϴ� �θ� �̹���
    public Image resultImg;         //�ν� ȹ�� ��� �ƹ���
    public Image playerImg;         //�÷��̾� �̹���
    public Image findthief;         //�����̹����� �νĵ� ��� ����ǥ �̹���
    public Image findperson;        //�ֹ��̹����� �νĵ� ��� ����ǥ �̹���
    public Image checkanswerImg;    //����� ���� ǥ�� �̹���
    public Image[] heart;           //������ ���⶧���� �����ϴ� ��Ʈ ������
    public Image[] resulteheart;    //��� �̹����� ��Ÿ�� ��Ʈ ������
    public Button startMatchBtn;    //���ǵ����ֱ� ��ư
    public Button checkBtn;         //����Ȯ���ϱ� ��ư
    public Button successBtn;       //�����ֱ⼺�� ��ư
    public Button failBtn;          //�����ֱ���� ��ư
    public Sprite wrong;            //���� ǥ��
    public Sprite right;            //���� ǥ��
    public Sprite[] thiefType;      //���� �̹��� ����, ��Ʈ�� ���
    public Sprite[] itemType;       //���� �̹��� ����, ��Ʈ�� ���
    public Text hintText;           //��Ʈ ����

    public bool[] isReturn = { false, false, false };   //�ֹ��� ������ �����޾Ҵ��� Ȯ���ϴ� bool �迭, �� �ֹ��� ������ �������� ������ true�� �����
    public int[] thiefitemIndex = { 0, 0, 0 };  //������ ������ �ִ� ������ ��ȣ
    public int[] personitemIndex = { 0, 0, 0 }; //�ֹ��� �Ҿ���� ������ ��ȣ
    public int thiefNow;    //���� �νĵ� ������ ��ȣ
    public int personNow;   //���� �νĵ� �ֹ��� ��ȣ
    public bool isStart = false;    //���ǵ����ֱⰡ ���۵ƴ��� ����, true�� �Ǹ� ������ ������ �ִ� ������ ���̻� ��Ÿ���� ����
    bool isOpen = false;    //��Ʈ �̹��� Ȱ��ȭ�� �����ϴ� ����
    int hintCount;          //��Ʈ�� ����
    string nowplayer;       //���� �÷��̾�
    int MatchCount=0;       //���ǵ����ֱ⸦ ������ Ƚ��
    int matchIndex = 0;     //���ǵ����ֱ⸦ ������ Ƚ��
    void Start()
    {
        thiefNow = -1;      //���ۿ� �����̳� �ֹ��� �νĵ��� ���� �����̹Ƿ�
        personNow = -1;     //thiefNow�� personNow�� ���� -1�� ����
        nowplayer = PlayerPrefs.GetString("NowPlayer");
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");    //���� �÷��̾��� ��Ʈ ����
        hintText.text = hintCount.ToString();

        isStart = false;    //���ǵ����ֱ⸦ ���� ���� �� �� ����

        playerImg.sprite = Resources.Load<Sprite>(nowplayer + "letsgoImg"); //����ȭ�鿡 ���� �÷��̾��� �̹����� ��Ÿ��
    }
    void Update()
    {
        if (isStart)            //���ǵ����ֱ⸦ �����ϸ� �����̳� ��� �̹����� �νĵǸ� ����ǥ�� ��Ÿ������ ��
        {
            if (matchIndex < 3) //���� ������ 3������ Ŀ���� ���� �����̹Ƿ� ����ǥ�� �� ��Ÿ���� �ʵ��� ��
            {
                if (personNow != -1)    //-1�� �ƴϸ� �ֹ� �̹��� �νĻ���
                    findperson.gameObject.SetActive(true);
                if (thiefNow != -1)     //-1�� �ƴϸ� ���� �̹��� �νĻ���
                    findthief.gameObject.SetActive(true);
            }
        }
    }
    public void StartMatchPerson()  //���ǵ����ֱ� ��ư�� ������ ����Ǵ� �Լ�
    {
        isStart = true; 
        startMatchBtn.gameObject.SetActive(false);  //�����ֱ��ư ��Ȱ��ȭ
        //�����̹����� �ν����� �� ��Ÿ�� ������Ʈ ��� ��Ȱ��ȭ
        GameObject.Find("Thief AR Setting").GetComponent<ThiefFind>().AllSetFalse();
        checkBtn.gameObject.SetActive(true);        //����Ȯ���ϱ� ��ư Ȱ��ȭ
    }
    public void StartBtn()          //���� �ȳ� �гο� �ִ� �����ַ����� ��ư�� ������ ����
    {
        noticePanel.SetActive(false);//�ȳ� �г��� ��Ȱ��ȭ ��
    }
    public void ShowHowToPlay()     //�ȳ� �гο��� ? ��ư�� ������ ����
    {
        isOpen = !isOpen;           //isOpen�� �����ϸ� ���� ��� �̹����� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
        howtoImg.gameObject.SetActive(isOpen);
    }
    public void UseHint()   //��Ʈ ��ư�� ������ ����Ǵ� �Լ�
    {   //��Ʈ�� ������ �о��
        hintCount = PlayerPrefs.GetInt(nowplayer + "_Hint");
        if (hintCount > 0&& isStart)            //��Ʈ�� ������ 0 �̻��̰� ������ ������ ��쿡�� ����
        {
            int hintType = Random.Range(0, 3);
            thiefHint.sprite = thiefType[hintType]; //hintType�� ���� ���� ��Ʈ�� ���� �̹����� ��Ÿ��
            itemHint.sprite = itemType[thiefitemIndex[hintType]];   //hintType�� �ش��ϴ� ������ ���� �̹����� ��Ÿ��
            hintImg.gameObject.SetActive(true);
            Invoke("CloseHint", 1.5f);  //��Ʈ�� 1.5�� �Ŀ� �����
            hintCount--;                //��Ʈ�� ������ �����ϰ� �ٽ� key�� ���� ����
            PlayerPrefs.SetInt(nowplayer + "_Hint", hintCount);
            hintText.text = hintCount.ToString();   //��Ʈ ���� �ؽ�Ʈ ����
        }
    }
    public void CloseHint() //��Ʈ �̹����� �ݴ� �Լ�
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
    public void SuccessBtn()        //������ ��� ��Ÿ���� ��ư�� ������ �� ����
    {
        heart[MatchCount].gameObject.SetActive(true);   //MatchCount==0 : ó�� ������ ���� / MatchCount==1 : 2��° ������ ���� / MatchCount==3 : 3��° ������ ����
        MatchCount++;
        successBtn.gameObject.SetActive(false);
        matchIndex++;                   //����Ȯ�� Ƚ�� ����
        if (matchIndex >= 3)            //3�� �Ǹ� 3�� ��� Ȯ���غ� ���� �ǹ���
            Invoke("FinishGame", 1f);   //���� ���� �Լ��� ����
        else                            //3���� ���� ��� ����Ȯ���ϱ� ��ư�� Ȱ��ȭ �� ������ ��� ����
        {
            checkBtn.gameObject.SetActive(true);
        }
        resetNotice();
    }
    void FinishGame()           //���� ���� �Լ�
    {
        resultImg.gameObject.SetActive(true);           //�ν� ȹ�� ����� ������
        for(int i=0; i<MatchCount; i++)                 //������ ���� Ƚ����ŭ ��Ʈ ������ Ȱ��ȭ
        {
            resulteheart[i].gameObject.SetActive(true);
        }
        int heartcount = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heartcount = heartcount + MatchCount;           //ȹ���� �νɰ� ���� �ν��� ���ؼ� key�� ����
        PlayerPrefs.SetInt(nowplayer + "_Heart", heartcount);
        FinishTurn.NewGame(nowplayer);                  //���ʸ� ������
    }
    public void FailBtn()   //������ ��� ��Ÿ���� ��ư�� ������ �� ����
    {
        failBtn.gameObject.SetActive(false);
        matchIndex++;                   //���� Ȯ�� Ƚ���� ������
        if (matchIndex >= 3)
            Invoke("FinishGame", 1f);
        else
        {
            checkBtn.gameObject.SetActive(true);
        }
        resetNotice();
    }
    public void EndGame()   //��� �̹����� ���Ե� �����ư�� ������ ����
    {
        SceneManager.LoadScene("TreasureMap");  //�ٽ� ���� Scene���� �̵�
    }
    void resetNotice()      //����Ȯ���ϱ� �� ����ǥ ǥ�ô� �����ϰ� �ֹΰ� ���� �ν� ���� �ʱ�ȭ
    {
        findperson.gameObject.SetActive(false);
        findthief.gameObject.SetActive(false);
        checkanswerImg.gameObject.SetActive(false);
        thiefNow = -1;
        personNow = -1;
        //ThiefFind�� �ִ� �Լ��� ȣ���� ���� �ֹ� ���� ��� ��Ȱ��ȭ �ص�
        GameObject.Find("Thief AR Setting").GetComponent<ThiefFind>().PersonAllSetFalse();
    }
}