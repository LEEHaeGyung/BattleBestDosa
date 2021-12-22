using UnityEngine;
public class PictureMatch : MonoBehaviour   //���� �浹�� Ȯ���� �� ����ϴ� Ŭ����
{                                           //���� �� ������Ʈ�� ����Ǿ�����
    private void OnCollisionEnter(Collision collision)
    {
        string name1 = collision.gameObject.name;   //�浹�� ������Ʈ�� �̸��� Ȯ��
        string name2 = this.gameObject.name;        //���� ������Ʈ�� �̸� Ȯ��
        //���� ������Ʈ�� �浹�� ������Ʈ�� �̸� �� ���� ���ڰ� ���� ������ ���� ���� �ǹ�
        //���� ShowMap() �Լ��� ȣ����, �Ű������� ���� ���� ���� �̹����� ��ȣ�� �ǹ���
        if (name2.Equals("left1") && name1.Contains("1")) {
            GameObject.Find("GameManager").GetComponent<TMGameManager>().ShowMap(1);
        }
        else if (name2.Equals("left2") && name1.Contains("2")) {
            GameObject.Find("GameManager").GetComponent<TMGameManager>().ShowMap(2);
        }
        else if (name2.Equals("left3") && name1.Contains("3")) {
            GameObject.Find("GameManager").GetComponent<TMGameManager>().ShowMap(3);
        }
        else if (name2.Equals("left4") && name1.Contains("4")) {
            GameObject.Find("GameManager").GetComponent<TMGameManager>().ShowMap(4);
        }
        else if (name2.Equals("left5") && name1.Contains("5")) {
            GameObject.Find("GameManager").GetComponent<TMGameManager>().ShowMap(5);
        }
    }
    private void OnCollisionExit(Collision collision)
    {   //�浹�� ������ ������ ��������� ��
        GameObject.Find("GameManager").GetComponent<TMGameManager>().CloseMap();
    }
}
