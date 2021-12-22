using UnityEngine;
using UnityEngine.UI;
public class ShowTreasureMap : MonoBehaviour
{   //���� �̹����� ������� ������ �������� ����� �����ϴ� Ŭ����
    public Button mapButton;        //���� ��ư
    public Button checkButton;      //����Ȯ�οϷ� ��ư
    public Sprite mapImg;           //���� �̹���
    public Sprite startImg;         //���� ��ġ ǥ�� �̹��� O ǥ��
    public Sprite goalImg;          //���� ��ġ ǥ�� �̹��� X ǥ��
    public Image[] mapPos;          //������ ��ġ�� ǥ���� �̹���, Sprite�� �������

    int startPos;           //������ġ ����
    int goalPos;            //������ġ ����
    bool setPos= false;     //��ġ�� �̹� �����ߴ��� �Ǵ��ϴ� ����
    private void Start()
    {   //��ġ ���� ���δ� false�� ����
        setPos = false;
    }
    public void OnPictureTouch()    //���� ��ư�� ������ �� ����Ǵ� �Լ�
    {
        if (!setPos)
        {
            startPos = Random.Range(0, 9);  //���� ��ġ�� 0~8 �� �� �������� ����
            goalPos = Random.Range(0, 9);   //���� ��ġ�� 0~8 �� �� �������� ����
            while (startPos == goalPos)     //���� ������ġ�� ���� ��ġ�� ������ �ٸ� ������ ����
            { goalPos = Random.Range(0, 9); }

            PlayerPrefs.SetInt("StartPos", startPos);       //"StartPos" : ���� ��ġ
            PlayerPrefs.SetInt("Jin_Position", startPos);   //"Jin_Position" : ���� ��ġ, �ʱ⿣ ���������� ��
            PlayerPrefs.SetInt("Yoon_Position", startPos);  //"Yoon_Position" : ���� ��ġ, �ʱ⿣ ���������� ��
            PlayerPrefs.SetInt("GoalPos", goalPos);         //"GoalPos" : ���� ��ġ

            mapPos[startPos].sprite = startImg;             //������ġ�� �ش��ϴ� ���� �̹����� O ǥ�÷� ����
            mapPos[startPos].gameObject.SetActive(true);    //���� ��ġ �̹����� Ȱ��ȭ��
            mapPos[goalPos].sprite = goalImg;               //������ġ�� �ش��ϴ� ���� �̹����� X ǥ�÷� ����
            mapPos[goalPos].gameObject.SetActive(true);     //���� ��ġ �̹����� Ȱ��ȭ
            mapButton.image.sprite = mapImg;                //���� ��ư�� �̹����� ������ ����

            checkButton.gameObject.SetActive(true);         //����Ȯ�οϷ� ��ư Ȱ��ȭ
            setPos = true;
            ShuffleGame();
        }
    }
    public void ShuffleGame()   //������ �������� ���� �Լ�
    {                           //� ��ġ���� � ������ ����Ǵ��� �� �� ������ �ϱ� ����
        //����� ������ 4������ ������ �����̹Ƿ� index�� 0~3 ������ ���� ���� 
        int[] index = { 0, 0, 1, 1, 1, 2, 2, 3, 3 };
        //���� Scene�� �̸���
        string[] gamesceneName = { "CatchThieves", "FindBok", "DressingDoryung", "FarmCount" };
        //index �迭�� ���� �����ϰ� ����
        for (int i = 0; i < index.Length; i++)
        {
            int a = Random.Range(0, index.Length);
            int b = Random.Range(0, index.Length);
            int temp;
            temp = index[a];
            index[a] = index[b];
            index[b] = temp;
        }
        for (int i = 1; i <= 9; i++)
        {
            //"Game1", "Game2", ..., "Game9"�� �� key�� ���� Scene�� �̸��� �����ϰ� ����
            //index �迭�� ���� �����ϰ� �������Ƿ� index �迭�� ���� ���� �̸��� �ٸ��� �����
            PlayerPrefs.SetString("Game" + i, gamesceneName[index[i - 1]]);
        }
    }
}
