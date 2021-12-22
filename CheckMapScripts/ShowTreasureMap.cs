using UnityEngine;
using UnityEngine.UI;
public class ShowTreasureMap : MonoBehaviour
{   //보물 이미지를 누를경우 지도가 보여지는 기능을 수행하는 클래스
    public Button mapButton;        //지도 버튼
    public Button checkButton;      //지도확인완료 버튼
    public Sprite mapImg;           //지도 이미지
    public Sprite startImg;         //시작 위치 표시 이미지 O 표시
    public Sprite goalImg;          //보물 위치 표시 이미지 X 표시
    public Image[] mapPos;          //지도의 위치를 표시한 이미지, Sprite는 투명상태

    int startPos;           //시작위치 변수
    int goalPos;            //보물위치 변수
    bool setPos= false;     //위치를 이미 결정했는지 판단하는 변수
    private void Start()
    {   //위치 결정 여부는 false로 시작
        setPos = false;
    }
    public void OnPictureTouch()    //지도 버튼을 눌렀을 때 실행되는 함수
    {
        if (!setPos)
        {
            startPos = Random.Range(0, 9);  //시작 위치를 0~8 값 중 랜덤으로 결정
            goalPos = Random.Range(0, 9);   //보물 위치를 0~8 값 중 랜덤으로 결정
            while (startPos == goalPos)     //만약 시작위치와 보물 위치가 같으면 다를 때까지 변경
            { goalPos = Random.Range(0, 9); }

            PlayerPrefs.SetInt("StartPos", startPos);       //"StartPos" : 시작 위치
            PlayerPrefs.SetInt("Jin_Position", startPos);   //"Jin_Position" : 진의 위치, 초기엔 시작점으로 함
            PlayerPrefs.SetInt("Yoon_Position", startPos);  //"Yoon_Position" : 윤의 위치, 초기엔 시작점으로 함
            PlayerPrefs.SetInt("GoalPos", goalPos);         //"GoalPos" : 보물 위치

            mapPos[startPos].sprite = startImg;             //시작위치에 해당하는 곳의 이미지를 O 표시로 변경
            mapPos[startPos].gameObject.SetActive(true);    //시작 위치 이미지를 활성화함
            mapPos[goalPos].sprite = goalImg;               //보물위치에 해당하는 곳의 이미지를 X 표시로 변경
            mapPos[goalPos].gameObject.SetActive(true);     //보물 위치 이미지를 활성화
            mapButton.image.sprite = mapImg;                //지도 버튼의 이미지를 지도로 변경

            checkButton.gameObject.SetActive(true);         //지도확인완료 버튼 활성화
            setPos = true;
            ShuffleGame();
        }
    }
    public void ShuffleGame()   //게임을 랜덤으로 섞는 함수
    {                           //어떤 위치에서 어떤 게임이 실행되는지 알 수 없도록 하기 위함
        //현재는 게임이 4가지만 구현된 상태이므로 index에 0~3 사이의 값만 존재 
        int[] index = { 0, 0, 1, 1, 1, 2, 2, 3, 3 };
        //게임 Scene의 이름들
        string[] gamesceneName = { "CatchThieves", "FindBok", "DressingDoryung", "FarmCount" };
        //index 배열의 값을 랜덤하게 섞음
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
            //"Game1", "Game2", ..., "Game9"의 각 key에 게임 Scene의 이름을 랜덤하게 저장
            //index 배열의 값이 랜덤하게 섞였으므로 index 배열의 값에 따라 이름이 다르게 저장됨
            PlayerPrefs.SetString("Game" + i, gamesceneName[index[i - 1]]);
        }
    }
}
