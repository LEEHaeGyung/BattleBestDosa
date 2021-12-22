using UnityEngine;
public class PictureMatch : MonoBehaviour   //구의 충돌을 확인할 때 사용하는 클래스
{                                           //왼쪽 구 오브젝트에 연결되어있음
    private void OnCollisionEnter(Collision collision)
    {
        string name1 = collision.gameObject.name;   //충돌한 오브젝트의 이름을 확인
        string name2 = this.gameObject.name;        //현재 오브젝트의 이름 확인
        //현재 오브젝트와 충돌한 오브젝트의 이름 맨 뒤의 숫자가 같은 퍼즐을 맞춘 것을 의미
        //따라서 ShowMap() 함수를 호출함, 매개변수는 현재 맞춘 보물 이미지의 번호를 의미함
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
    {   //충돌이 끝나면 지도가 사라지도록 함
        GameObject.Find("GameManager").GetComponent<TMGameManager>().CloseMap();
    }
}
