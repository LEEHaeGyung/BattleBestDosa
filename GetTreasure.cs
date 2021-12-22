using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GetTreasure : MonoBehaviour    //������ ȹ���ϴ� ��鿡�� ���� Ŭ����
{
    public Sprite[] dialogImg;      //����� �������� �̹���
    public Image speechBubble;      //�������� ��ǳ��
    public Image stone;             //���� �̹���
    public GameObject stoneImg;     //�����̹���+������ȿ���̹����� �Բ� �ڽĿ�����Ʈ�� �� ���ӿ�����Ʈ
    public Button findBtn;          //����ȹ�� ��ư(������ ȹ���� ���)
    public Button returnBtn;        //���ư��� ��ư(ȹ�� �ڰ��� ������ ���� ���)

    string nowplayer;               //���� �÷��̾�
    string heartkey;                //��Ʈ�� ������ �˱� ���� ������ key�� �̸�, �����÷��̾�+_Heart�� ��
    int dialogIndex = 0;            //����� �ε���, �ε����� �����ϸ� ��簡 �����
    private void Start()
    {
        nowplayer = PlayerPrefs.GetString("NowPlayer"); //���� �÷��̾� ���ڿ��� ����
        heartkey = nowplayer + "_Heart";                //��Ʈ ������ Ȯ���� key�� �̸� ����
    }
    public void NextDialog(Button charBtn)        //������ �̹����� ���� ������ ����Ǵ� �Լ�, ��簡 �����
    {
        if (dialogIndex == 0)       //0�̸� ���� ��ȭ�� �����Ѱ� �ƴϹǷ� ��ǳ���� ��Ÿ���� ��
            speechBubble.gameObject.SetActive(true);
        if (dialogIndex < 2)        //2���� �������� dialogIndex�� ���� �����ϸ� ��� ����
        {   speechBubble.sprite = dialogImg[dialogIndex];
            dialogIndex++;
        }
        else                        //2���� ū ��� �ڰ��� ����� ���� ��簡 ����ǹǷ� �߰������� ���ǹ� �ۼ�
        {
            charBtn.enabled = false;   //���� �� �̻� ������ ���� ���̹Ƿ� ��ư�� ��Ȱ��ȭ ��
            if (PlayerPrefs.GetInt(heartkey) >= 5)
            {   //���� �ν��� 5�� �̻� ȹ���ߴٸ� ��������� ���� �Բ� ���� �̹����� �����ϴ� �Լ� ȣ��
                speechBubble.sprite = dialogImg[dialogIndex];
                Invoke("ShowStone", 2f);
            }
            else
            {   //���� �ν��� 5�� �̻� ������ ���ߴٸ� ���ư���� ������ �Բ� ������ ���ư��� ��ư�� ����
                speechBubble.sprite = dialogImg[dialogIndex + 1];
                returnBtn.gameObject.SetActive(true);
            }
        }
    }
    void ShowStone()
    {   //"FindTreasure" : ã�� ������ ����, ����Ȯ�� �������� ���� �׸��� �ν��ϸ� ������
        int type = PlayerPrefs.GetInt("FindTreasure");
        //������ Sprite�� type�� �ش��ϴ� �̹����� ����
        stone.sprite = Resources.Load<Sprite>("mysteriousStone" + type);
        //������ �̹����� Ȱ��ȭ
        stoneImg.SetActive(true);
        //����ȹ�� ��ư�� ��Ÿ��
        findBtn.gameObject.SetActive(true);
    }
    public void FindBtn()
    {   //"Treasure"�� ���� ã�� ������ ��ȣ�� ������ ���ڿ��� key�� �����ϰ�
        //���� �÷��̾��� �̸��� ������
        string findkey = "Treasure" + PlayerPrefs.GetInt("FindTreasure");
        PlayerPrefs.SetString(findkey, nowplayer);
        //������ ȹ�������Ƿ� ���ο� ������ ã�� �����ϹǷ� NewTreasure() �Լ� ȣ��
        FinishTurn.NewTreasure(nowplayer);
        SceneManager.LoadScene("Home");
    }
    public void ReturnMap()     //���ư��� ��ư�� ������ ����Ǵ� �Լ�
    {   //���� ���ʸ� �����ϰ� ���� Scene���� �̵���
        FinishTurn.NewGame(nowplayer);
        SceneManager.LoadScene("TreasureMap");
    }
}
