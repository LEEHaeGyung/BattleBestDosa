using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TMGameManager : MonoBehaviour  //���� �̹��� Ȱ��ȭ�� ������ Ŭ����
{
    public Button mapButton;                //�ڵ� �̹����� ��ư
    public Button isFoundImg;               //�̹� ã�� �����̶�� �� �˷��ִ� �̹�����ư
    public Sprite[] treasureType;           //���� �̹��� ����

    bool isCheck;   //������ Ȯ���ߴ��� �Ǵ��ϴ� ����
    private void Start()
    {   //���� �̹����� ��Ȱ��ȭ �س��� ������
        mapButton.gameObject.SetActive(false);
        isCheck = false;
    }
    void Update()
    {   //�ڷΰ��� ��ư�� ������ Ȩȭ������ �̵���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home");
        }
    }
    public void ShowMap(int type)   //���� �̹����� �����ִ� �Լ�
    {                               //�ʱ⿡�� ���� �̹���������, ���� Sprite �������� ������ ��
        if (!isCheck)               //isCheck�� false �϶��� ����, �ߺ������� ������ Ȯ���ϴ� �� ���� 
        {
            string treasureKey = "Treasure" + type;             //�Ű������� ���� type ������ key ���� ����
            if (PlayerPrefs.GetString(treasureKey)=="None") {   //key�� ����� ���ڿ��� "None"�̸� ���� ã�� ��
                isFoundImg.gameObject.SetActive(false);         //�̹� ã�� �����̶�� �̹����� Ȱ��ȭ ���¸� �ٽ� ��Ȱ��ȭ
                mapButton.image.sprite = treasureType[type - 1];//�̹����� �ν��� ���� �̹����� ���� ������ ����
                mapButton.gameObject.SetActive(true);           //�̹��� ��ư�� Ȱ��ȭ
                PlayerPrefs.SetInt("FindTreasure", type);       //"FindTreasure":���� ã������ ������ �ǹ�, �ν��� ������ ��ȣ�� �����
                isCheck = true;     //isCheck�� ���� ������, �̷����� ������ �ߺ������� Ȯ���ϴ� �� ������
            }
            else//key�� ����� ���ڿ��� "None"�� �ƴ϶��, ������ ã�� �����̹Ƿ� �̹� ã�Ҵٴ� �ȳ� ǥ��
            {isFoundImg.gameObject.SetActive(true);}
        }
    }
    public void CloseImg()  //�̹� ã�� ���� �ȳ� �̹����� ������ �� ����Ǵ� �Լ�
    {   //�̹����� �ٽ� ��Ȱ��ȭ ��
        isFoundImg.gameObject.SetActive(false);
    }
    public void CloseMap()
    {
        if (!isCheck) mapButton.gameObject.SetActive(false);
    }
}
