using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject endPanel;         //���Ӱ��(�����̹����� ��Ÿ����) �г�
    public GameObject resultNotice;     //�������� �ȳ� �̹���
    public GameObject yoonCharBtn;      //Ȩȭ���� ��ĳ���� ��ư
    public GameObject jinCharBtn;       //Ȩȭ���� ��ĳ���� ��ư
    public Button checkmapButton;       //����Ȯ���ϱ� ��ư
    public Button findtreasureButton;   //����ã�������� ��ư
    public Button jinBtn;               //ĳ���� ���� ��ư �� ����ư
    public Button yoonBtn;              //ĳ���� ���� ��ư �� ����ư
    public Button yooncollection;       //���� �νɰ� ���� ��Ȳ �̹���
    public Button jincollection;        //���� �νɰ� ���� ��Ȳ �̹���
    public Button modeChangeBtn;        //ȥ���ϱ�<->�����ϱ� ��ȯ ��ư
    public Image selectImg;             //ĳ���� ������ �����ϴ� �̹���
    public Image winnerImg;             //�������� �� ���� �̹���
    public Image setting;               //���� �̹���

    public Sprite heartGet;             //��Ʈ ȹ���� �̹���
    public Sprite singlemode;           //ȥ���ϱ� ����� ��� �����̹��� ���
    public Sprite modeChange;           //ȥ���ϱ� ����� ��� ��� ��ȯ��ư �̹���
    public Sprite[] findStone;          //�� ĳ������ ��Ȳ�̹����� ��Ÿ�� ���� ������
    public Sprite[] notImg;             //ĳ���͸� ���� ���� ��� ȸ���������� �̹���
    public Image[] jinsheart;           //���� �ν� �����ܵ�
    public Image[] yoonsheart;          //���� �ν� �����ܵ�
    public Image[] jinsstone;           //���� ���� �����ܵ�
    public Image[] yoonsstrone;         //���� ���� �����ܵ�

    int isSelect=-1;                    //������ ĳ���� �Ǵ� ������ ĳ���͸� �����ߴ��� ����
    int jincount=0;                     //���� ���� ������ ����
    int yooncount=0;                    //���� ���� ������ ����
    int gamemode = 0;                   //ȥ���ϱ����� �����ϱ����� �Ǵ��ϴ� ����
    bool isSettingOpen = false;         //���� �̹����� Ȱ��ȭ�� �����ϴ� ����

    void Start()
    {   //"Select" : ĳ���͸� ������ ����, 0�̸� ���þ���, 1�̸� ������
        if (!PlayerPrefs.HasKey("Select")) { PlayerPrefs.SetInt("Select", 0); }
        isSelect = PlayerPrefs.GetInt("Select");
        if (isSelect == 0){ 
            //0�̸� ĳ���͸� �����϶�� ���� �̹����� ���� ����Ȯ�ι�ư�� ��Ȱ��ȭ�ص�
            selectImg.gameObject.SetActive(true);
            checkmapButton.enabled = false;}

        //"GameMode" : ������ ���, 1�̸� ȥ���ϱ�, 2�̸� �����ϱ�
        if (!PlayerPrefs.HasKey("GameMode")) { PlayerPrefs.SetInt("GameMode", 2); }
        gamemode = PlayerPrefs.GetInt("GameMode");
        //��尡 1�̶�� ȥ���ϱ��� �����ϴ� �Լ��� ȣ����
        if (gamemode == 1) { SingleModeSetting(); }

        //"GameStart" : ������ �����ߴ��� ����, 0�� ���۾���, 1�� ������
        //�����̹����� �ν��� �Ŀ� ���� 1�� �����
        if (PlayerPrefs.GetInt("GameStart") == 1)
            findtreasureButton.gameObject.SetActive(true);  //����ã�������� ��ư Ȱ��ȭ
        else
            checkmapButton.gameObject.SetActive(true);      //����Ȯ���ϱ� ��ư Ȱ��ȭ

        Treasure();             //������ ������ �ʱ�ȭ��

        SetYoonsCollection();   //���� ��Ȳ�� ������
        SetJinsCollection();    //���� ��Ȳ�� ������
        CheckFinish();          //������ ����Ǿ����� Ȯ����
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //������ư�� ������ �� ����
        {
            Application.Quit();
        }
    }
    public void ShowSetting()   //���� ��ư�� ������ �Լ�
    {                           //isSettingOpen�� ���� ���� ���� �̹��� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
        isSettingOpen = !isSettingOpen;
        setting.gameObject.SetActive(isSettingOpen);
    }
    public void ResetGame()         //�������� ���Ӵٽý����� ������ ����Ǵ� �Լ�
    {
        PlayerPrefs.DeleteAll();                    //��� key�� ������
        PlayerPrefs.SetInt("GameMode", gamemode);   //���Ӹ��� ������� �����Ƿ� �ٽ� ���� ����
        PlayerPrefs.SetInt("Select", 0);
        SceneManager.LoadScene("Home");             //Ȩȭ���� �ٽ� �ε���
    }
    public void ChangeMode()        //�����ȯ ��ư�� ������ ����Ǵ� �Լ�
    {
        if (gamemode == 2) { gamemode = 1; }        //����� ���� ���� �ٸ� ������ ����
        else if (gamemode == 1) { gamemode = 2; }
        ResetGame();                                //��带 �����ϸ� ���� �ٽ� ����
    }
    public void SingleModeSetting() //ȥ������ ���� ������ ����
    {   //ĳ���� ���� ���� �̹����� ����� ����
        selectImg.sprite = singlemode;
        //�����ȯ ��ư�� �̹����� ����, �⺻�� [ȥ���ϱ����ȯ], ������ [�����ϱ����ȯ]
        modeChangeBtn.image.sprite = modeChange;
        if (PlayerPrefs.GetInt("Select") != 0)
        {
            //PlayerPrefs.GetInt("Select") != 0 ��� ĳ���͸� �ϳ� ������ ���̹Ƿ� ���� ����
            //���� �÷��̾ �ش��ϴ� ĳ���� ��ư�� ���ΰ� �ٸ� ĳ���� ��ư�� ��Ȱ��ȭ
            //�׸��� ��ư�� ��ġ�� ����� �̵���
            if (PlayerPrefs.GetString("NowPlayer") == "Jin") {
                yoonCharBtn.SetActive(false);
                jinCharBtn.transform.localPosition = new Vector2(0, -197);
            }
            else{
                jinCharBtn.SetActive(false);
                yoonCharBtn.transform.localPosition = new Vector2(0, -197);
            }
        }
    }
    public void GotoCheckMap()      //����Ȯ���ϱ� ��ư�� ������ �� ����Ǵ� �Լ�
    {   //���� Ȯ�� Scene�� �ε���
        SceneManager.LoadScene("CheckMap");
    }
    public void GotoFindTreasure()  //����ã�������� ��ư�� ������ �� ����Ǵ� �Լ�
    {   //������ �׷��� Scene�� �ε���
        SceneManager.LoadScene("TreasureMap");
    }
    public void SelectJin()         //ĳ���� ���ÿ��� ���� ����� �� ����Ǵ� ��ư
    {   //"NowPlayer" : ���� �÷��̾ �̹�, "Jin" �Ǵ� "Yoon"�� ����
        PlayerPrefs.SetString("NowPlayer", "Jin");
        SelectPlayer(yoonBtn, 0);
        if (gamemode == 1) SingleModeSetting();
    }
    public void SelectYoon()        //ĳ���� ���ÿ��� ���� ����� �� ����Ǵ� ��ư
    {
        PlayerPrefs.SetString("NowPlayer", "Yoon");
        SelectPlayer(jinBtn, 1);
        if (gamemode == 1) SingleModeSetting();
    }
    void SelectPlayer(Button charBtn, int type)
    {   //���õ��� ���� ĳ������ ��ư�� �̹����� ����, type�� ���� 0�� ��, 1�� ���� �������� ���� �̹���
        charBtn.image.sprite = notImg[type];
        //���õ��� ���� ��ư�� ũ�⸦ ������
        charBtn.transform.transform.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        //"Select"�� ���� ����
        PlayerPrefs.SetInt("Select", 1);
        //ĳ���� ���� �̹����� 2�� �Ŀ� ����
        Invoke("CloseSelectImg", 2f);
        //����Ȯ���ϱ� ��ư�� Ȱ��ȭ
        checkmapButton.enabled = true;
    }
    public void CloseSelectImg()    //ĳ���� ���� �̹����� �ݴ� �Լ�
    {
        selectImg.gameObject.SetActive(false);
    }
    public void ShowJins()          //���� ���������� Ȱ��ȭ�ϴ� �Լ�
    {
        if (PlayerPrefs.GetInt("Select") != 0)
        {   //���� ���������� Ȱ��ȭ�ϸ� ���� ���������� ��Ȱ��ȭ
            jincollection.gameObject.SetActive(true);
            yooncollection.gameObject.SetActive(false);
        }
    }
    public void ShowYoons()         //���� ���������� Ȱ��ȭ�ϴ� �Լ�
    {
        if (PlayerPrefs.GetInt("Select") != 0)
        {   //���� ���������� Ȱ��ȭ�ϸ� ���� ���������� ��Ȱ��ȭ
            yooncollection.gameObject.SetActive(true);
            jincollection.gameObject.SetActive(false);
        }
    }
    public void CloseJins()     //���� �������� ��Ȱ��ȭ, ���� ���������� ������ �����
    {
        jincollection.gameObject.SetActive(false);
    }
    public void CloseYoons()    //���� �������� ��Ȱ��ȭ, ���� ���������� ������ �����
    {
        yooncollection.gameObject.SetActive(false);
    }
    public void Treasure()      //������ ������ �ʱ�ȭ
    {
        for (int i = 1; i <= 5; i++)
        {   //"Treasure1"~"Treasure5"�� key�� ���ٸ� ���� �ʱ� ���¸� �ǹ��ϹǷ� �� key�� "None"�� ����
            //���� ȹ���� �� �� key�� string ���� ȹ���� �÷��̾��� �̸��� ��
            if (!PlayerPrefs.HasKey("Treasure" + i)) { PlayerPrefs.SetString("Treasure" + i, "None"); }
        }
    }
    public void SetYoonsCollection()    //���� ���������� ������
    {   //"Yoon_Heart" : ���� ���� �ν��� ����
        int heartcount = PlayerPrefs.GetInt("Yoon_Heart");
        for(int i=0; i<heartcount; i++){
            //�ν��� ������ŭ ��Ʈ�� �̹����� ȹ���� �������� ������
            yoonsheart[i].sprite = heartGet;
        }
        for(int i=1; i <= 5; i++)
        {   //"Treasure1"~"Treasure5"�� ���� "Yoon" �͵��� Ȯ���� ������ �̹����� ȹ����·� ������
            if (PlayerPrefs.GetString("Treasure" + i) == "Yoon"){
                yoonsstrone[i - 1].sprite = findStone[i - 1];
                //���� ���� ȹ�� ������ ������
                yooncount++;
            }
        }
    }
    public void SetJinsCollection() //���� ���������� ������
    {   //"Jin_Heart" : ���� ���� �ν��� ����
        int heartcount = PlayerPrefs.GetInt("Jin_Heart");
        for (int i = 0; i < heartcount; i++){
            jinsheart[i].sprite = heartGet;
        }
        for (int i = 1; i <= 5; i++)
        {   //"Treasure1"~"Treasure5"�� ���� "Jin" �͵��� Ȯ���� ������ �̹����� ȹ����·� ������
            if (PlayerPrefs.GetString("Treasure" + i) == "Jin") {
                jinsstone[i - 1].sprite = findStone[i - 1];
                //���� ���� ȹ�� ������ ������
                jincount++;
            }
        }
    }
    void CheckFinish()                      //������ ����� �� �ִ� ��Ȳ���� �Ǵ���
    {   //�� ����� ���� ȹ�� ���� 5�̻��̸� ��� ������ ã�� �� �ǹ�
        //���Ḧ �ȳ��ϴ� �̹����� Ȱ��ȭ
        if ((jincount + yooncount) >= 5) { resultNotice.SetActive(true); } 
    }
    public void WhoIsWinner() //���ڸ� Ȯ���ϴ� �Լ�, ���� �ȳ��̹������� ������� ��ư�� ������ ����
    {   //������ �̸��� ����� ���ڿ�
        string winner;
        //����ȹ�� ������ ���� ���� �̸��� ����
        if (jincount > yooncount)
            winner = "Jin";
        else
            winner = "Yoon";
        //��� �̹����� Ȱ��ȭ
        endPanel.SetActive(true);
        //�¸��� ĳ������ �̹����� ���� �̹����� ������
        winnerImg.sprite = Resources.Load<Sprite>(winner + "winner_ver");
    }
}
