using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LuckyOrHitandMiss : MonoBehaviour  //�������, ���������� ���� ����Ǵ� Ŭ����
{
    public GameObject findStone;    //������ ���� ��� ��Ÿ���� ���ӿ�����Ʈ
    public GameObject lorhPanel;    //�������, �������� ��Ȳ�� ��� Ȱ��ȭ�� �г�
    public Image stoneImg;          //findStone�� ���Ե� ���� �̹���
    public Image noticePanel;       //�ȳ� �̹���, � ��Ȳ�� ���õǴ��� �ȳ��Ǵ� �̹���
    public Image cantstillPanel;    //����� ������ ���� ������ ���� �� ���� ��� �ȳ� �̹���
    public Image hintResult;        //��Ʈ ȹ�� ���ΰ� �ȳ��Ǵ� �̹���
    public Image heartResult;       //�ν� ȹ�� ���ΰ� �ȳ��Ǵ� �̹���
    public Image heartImg;          //�ν� ������
    public Image[] luckyIcon;       //��� ������

    public Button endBtn;           //���� ��ư
    public Button showBtn;          //������� ��ư
    public Sprite lostheart;        //��Ʈ�� ���� ����� ��Ʈ �̹���(ȸ���̹���)
    public Sprite losthint;         //��Ʈ�� ���� ��� ��Ʈ �̹���(ȸ���̹���)
    public Sprite[] luckyTypeImg;   //������� ��Ȳ ����
    public Sprite[] HitMissTypeImg; //�������� ��Ȳ ����
    public Sprite[] stoneType;      //������ ����

    string typename="none";         //������� ��Ȳ���� �ƴ��� Ȯ�� ���ڿ�, "none"�̸� �� �� � ��Ȳ�� �ƴ� �� �ǹ�
    string nowplayer;               //���� �÷��̾� �̸�
    int luckyindex;                 //luckytype�� �ε����� �� ����
    int[] luckytype = { 0, 0, 0, 1, 1, 1, 2, 2, 2, 3 }; //��������� ��Ȳ, 3�� ���� 0~2���� ����ϵ��� �迭�� ���� ������ �ٸ��� ������
    int notType;                    //�������� ��Ȳ
    bool isLost;                    //���� ��Ȳ���� �Ǵ��ϴ� ����
private void Start()
    {
        isLost = false;
        if (PlayerPrefs.HasKey("LuckyOrNot"))           //"LuckyOrNot":������� ��Ȳ���� �ƴ��� Ȯ���ϴ� key, "Lucky"�� �������, �ƴ� ��� "Not"
        { typename = PlayerPrefs.GetString("LuckyOrNot"); }
        nowplayer = PlayerPrefs.GetString("NowPlayer"); //���� �÷��̾� �̸��� �о��
        if (typename != "none")                         //typename�� none�� �ƴ϶��
        {                                               //������� �Ǵ� �������� ��Ȳ �߻�
            lorhPanel.SetActive(true);                  //�ȳ� �г� Ȱ��ȭ
            if (typename == "Lucky") { SetLucky(); }    //"Lucky"�̸� ������� ��Ȳ ����
            else { SetHitandMiss(); }                   //�ƴ϶�� �������� ��Ȳ ����
        }
    }
    void SetLucky()     //������� ���� ����
    {
        luckyindex = Random.Range(0, luckytype.Length); //�ε��� ���� �������� ����
        //luckytype[luckyindex]�� �ش��ϴ� ������� ��Ȳ���� �ȳ� �̹��� ����
        noticePanel.sprite = luckyTypeImg[luckytype[luckyindex]];
        //luckytype[luckyindex]�� ���� 0�̸� �����ϱ� ��ư Ȱ��ȭ
        //0�� ���� ���ϴ� ������ �̵��ϴ� ���̹Ƿ� �����ư�� Ȱ��ȭ
        if (luckytype[luckyindex] == 0) { endBtn.gameObject.SetActive(true); }
        else { showBtn.gameObject.SetActive(true); }    //0�� �ƴ� ��� ������� ��ư Ȱ��ȭ
    }
    void SetHitandMiss()                //�������� ���� ����
    {
        notType = Random.Range(0, 3);                   //�������� ��Ȳ ����
        noticePanel.sprite = HitMissTypeImg[notType];   //���� ���� �ȳ� �̹��� ����
        int random = Random.Range(0, 2);
        if (random == 0) { isLost = true; } //random�� ���� ���� isLost�� ����, true�� �Ҵ� ��Ȳ
        if (notType == 2){ //notType�� 2�� ��� ��ġ�� �ٲٰ� �����ư Ȱ��ȭ
            ExchangePos();
            endBtn.gameObject.SetActive(true);
        }
        else{   //�ƴ� ��� ������� ��ư Ȱ��ȭ
            showBtn.gameObject.SetActive(true);
        }
    }
    public void ExchangePos()       //�� �÷��̾��� ��ġ�� �����ϴ� �Լ�
    {   //�����ϱ� ����϶��� ����, ȥ���ϱ�� ��ġ ������ ����
        if (PlayerPrefs.GetInt("GameMode") == 2)
        {
            int jpos = PlayerPrefs.GetInt("Jin_Position");  //�� ��ġ�� �о��
            int ypos = PlayerPrefs.GetInt("Yoon_Position");
            PlayerPrefs.SetInt("Jin_Position", ypos);       //�о�� ��ġ�� �� key�� ����
            PlayerPrefs.SetInt("Yoon_Position", jpos);
        }
    }
    public void ShowResult()    //������� ��ư�� ������ ��� ����Ǵ� �Լ�
    {
        noticePanel.gameObject.SetActive(false);    //�ȳ� �̹����� �����
        if (typename == "Lucky")    //��������� ���
        {
            if (luckytype[luckyindex] == 1)
            { GetHint(); }          //��Ʈ ȹ���� �� �ִ� ������� ��Ȳ
            else if (luckytype[luckyindex] == 2)
            {                       //�ν��� �� �� ȹ���� �� �ִ� ������� ��Ȳ
                GetHeart();
            }
            else if (luckytype[luckyindex] == 3)
            {   //����� ������ �ϳ� ������ �� �ִ� ������� ��Ȳ
                GetOthersStone();
            }
        }
        else    //���������� ���
        {
            if (notType == 0)
            {   //��Ʈ�� ��ų� ���� �� �ִ� �������� ��Ȳ
                if (isLost) { LostHint(); }
                else { GetHint(); }
            }
            else if (notType == 1)
            {   //�ν��� ��ų� ���� �� �ִ� �������� ��Ȳ
                if (isLost) { LostHeart(); }
                else { GetHeart(); }
            }
        }
    }
    public void GetHeart()  //�ν��� ȹ���ϴ� ��Ȳ
    {   //���� �ν� ������ Ȯ���ϰ� 1�� ������ ���� �ٽ� ������
        int heart = PlayerPrefs.GetInt(nowplayer + "_Heart");
        heart++;
        PlayerPrefs.SetInt(nowplayer + "_Heart", heart);
        heartResult.gameObject.SetActive(true);
    }
    public void LostHeart() //�ν��� �Ҵ� ��Ȳ
    {   //���� �ν� ������ �о��
        int heart = PlayerPrefs.GetInt(nowplayer + "_Heart");
        if (heart > 0)  
        {   //�ν��� ������ 0���� ũ�� �ν��� ������
            heart--;
            PlayerPrefs.SetInt(nowplayer + "_Heart", heart);
            heartImg.sprite = lostheart;
        }
        else{   //0���� ū ��Ȳ�� �ƴϸ� �������� ����
            heartImg.gameObject.SetActive(false);
        }
        heartResult.gameObject.SetActive(true);
    }
    public void GetHint()   //��Ʈ�� ȹ���ϴ� ��Ȳ
    {   //1~3�� ������ ���� ���� ��Ʈ ������ ���ؼ� ������
        int getHint = Random.Range(1, 4);
        int nowHint = PlayerPrefs.GetInt(nowplayer + "_Hint");
        PlayerPrefs.SetInt(nowplayer + "_Hint", nowHint + getHint);
        for (int i = 0; i < getHint; i++){
            luckyIcon[i].gameObject.SetActive(true);
        }
        hintResult.gameObject.SetActive(true);
    }
    public void LostHint()  //��Ʈ�� �Ҵ� ��Ȳ
    {   ////1~3�� ������ ���� �پ�� ��Ʈ�� ����
        int lostHint = Random.Range(1, 4);
        int nowHint = PlayerPrefs.GetInt(nowplayer + "_Hint");
        //���� nowHint - lostHint�� ���� 0���� ũ�� �� ���� ������
        if ((nowHint - lostHint) > 0)
        {
            PlayerPrefs.SetInt(nowplayer + "_Hint", nowHint - lostHint);
            for (int i = 0; i < lostHint; i++)
            {   //���� ��Ʈ�� ������ŭ ȸ�� ��Ʈ �������� ���
                luckyIcon[i].sprite = losthint;
                luckyIcon[i].gameObject.SetActive(true);
            }
        }
        else
        {   //���� nowHint - lostHint�� ���� 0���� ũ�� ������
            //����� ���� ������ ���� �ʵ��� �׳� 0�� ������
            PlayerPrefs.SetInt(nowplayer + "_Hint", 0);
            for (int i = 0; i < nowHint; i++)
            {   //���� ��Ʈ�� ��� ���� ���̹Ƿ� �� ����ŭ ȸ�� ��Ʈ �������� ���
                luckyIcon[i].sprite = losthint;
                luckyIcon[i].gameObject.SetActive(true);
            }
        }
        hintResult.gameObject.SetActive(true);
    }
    void GetOthersStone()   //�ٸ� ������ ������ �������� ��Ȳ
    {
        List<int> checkindex = new List<int>(); //��밡 ������ ���� ��ȣ�� �߰��� ����Ʈ
        string otherplayer;
        //���� �÷��̾��� �̸��� ���� ��� �÷��̾��� �̸��� ����
        if (nowplayer == "Yoon") { otherplayer = "Jin"; }
        else { otherplayer = "Yoon"; }
        for(int i=0; i < 5; i++)
        {   //"Treasure1"~"Treasure5" �߿� ��� �̸��� ����� ���
            //�ش� ��ȣ�� checkindex ����Ʈ�� �߰���
            if (PlayerPrefs.GetString("Treasure" + (i+1)) == otherplayer)
                checkindex.Add(i);
        }
        if (checkindex.Count > 0)
        {   //checkindex�� ũ�Ⱑ 0���� ũ�� ������ ������ �ִ� ���� �ǹ���
            findStone.SetActive(true);  //���� �̹��� Ȱ��ȭ
            int gettype = Random.Range(checkindex[0], checkindex.Count);    //checkindex�� ����� �� �� �ϳ��� ������ ������ ��ȣ�� ����
            stoneImg.sprite = stoneType[gettype];   //������ �̹����� �ش� ��ȣ�� �̹����� ����
            PlayerPrefs.SetString("Treasure"+ (gettype+1), nowplayer);  //Ű�� ����� �̸��� ���� �÷��̾�� ����
        }
        else { cantstillPanel.gameObject.SetActive(true); } //checkindex�� ����� ���� ������ ��ĥ�� ���ٴ� �ȳ� �̹����� Ȱ��ȭ��
    }
    public void ClosePanel(GameObject needclose)
    {   //needclose�� �ش��ϴ� ���� ������Ʈ�� ��Ȱ��ȭ
        needclose.SetActive(false);
    }
    public void EndLuckyOrNot() //���� ��ư�� ������ ����� �Լ�
    {
        PlayerPrefs.SetString("LuckyOrNot", "none");    //"LuckyOrNot" ���� none���� �����ؼ� ��Ȳ�� ������
        SceneManager.LoadScene("TreasureMap"); //�� Scene�� �ٽ� �ε���
    }
}
