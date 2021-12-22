using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ThiefFind : MonoBehaviour  //도둑이미지와 주민이미지 인식을 수행하는 클래스
{
    public GameObject[] items;          //잃어버린 물건
    public GameObject[] person;         //물건찾기가 해결된 경우 웃는 얼굴이미지
    int[] thiefindex = { 0, 1, 2 };     //도둑이 훔쳐간 아이템의 인덱스
    int[] personIndex = { 0, 1, 2 };    //주민이 잃어버린 아이템 인덱스

    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<string, GameObject> arobjects = new Dictionary<string, GameObject>();
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();

        //도둑을 인식했을 때 나타날 물건
        for (int i=0; i <= items.Length; i++)       //아이템 인덱스를 랜덤으로 셔플
        {
            int a = Random.Range(0, items.Length);
            int b = Random.Range(0, items.Length);
            int temp;

            temp = thiefindex[a];
            thiefindex[a] = thiefindex[b];
            thiefindex[b] = temp;
        }
        for (int i = 1; i <= items.Length; i++)
        {   //아이템 thiefindex[i-1]인덱스에 해당하는 items 배열에 저장된 Prefab을 인스턴스화
            GameObject newARobject = Instantiate(items[thiefindex[i-1]], Vector3.zero, Quaternion.identity);
            //key stringd은 각 thief1, thief2, thief3으로 하여 같은 이름이 이미지가 인식되면 해당 아이템이 나타나나도록 함
            newARobject.name = "thief" + i;
            arobjects.Add("thief" + i, newARobject);
        }
        //주민을 인식했을 때 나타날 물건
        for (int i = 0; i <= items.Length; i++)     //아이템 인덱스를 랜덤으로 셔플
        {
            int a = Random.Range(0, items.Length);
            int b = Random.Range(0, items.Length);
            int temp;

            temp = personIndex[a];
            personIndex[a] = personIndex[b];
            personIndex[b] = temp;
        }
        for (int i = 1; i <= items.Length; i++)
        {   //도둑의 아이템 설정과 같은 방식
            GameObject newARobject = Instantiate(items[personIndex[i - 1]], Vector3.zero, Quaternion.identity);
            newARobject.name = "lostperson" + i;
            arobjects.Add("lostperson" + i, newARobject);
            newARobject.SetActive(false);   //초기에는 주민의 아이템은 나타나면 안되므로 모두 비활성화
        }
        for (int i = 0; i < thiefindex.Length; i++)
        {   //각 도둑과 주민의 아이템 인덱스 값을 GameManager의 GameManager_Thief에 있는 배열에 각 저장
            //정답을 확인하는데 필요한 작업
            GameObject.Find("GameManager").GetComponent<GameManager_Thief>().thiefitemIndex[i] = thiefindex[i];
            GameObject.Find("GameManager").GetComponent<GameManager_Thief>().personitemIndex[i] = personIndex[i];
        }
        for (int i = 1; i <= items.Length; i++)
        {   //주민의 웃는 얼굴을 findversion1~findversion3의 string을 key로 해서 저장함
            GameObject newARobject = Instantiate(person[i-1], Vector3.zero, Quaternion.identity);
            newARobject.name = "findversion" + i;
            arobjects.Add("findversion" + i, newARobject);
            newARobject.SetActive(false);   //성공하지 않은 상태에서는 나타나면 안되므로 비활성화
        }
    }
    private void Start()
    {
        if (xRReferenceImageLibrary != null)
        {
            arTrackedImageManager.referenceLibrary = xRReferenceImageLibrary;
        }
    }
    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateTrackingGameObject(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                UpdateTrackingGameObject(trackedImage);
            }
            else if (trackedImage.trackingState == TrackingState.None || trackedImage.trackingState == TrackingState.Limited)
            {
                UpdateNoneGameObject(trackedImage);
            }
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            UpdateNoneGameObject(trackedImage);
        }
    }
    private void UpdateTrackingGameObject(ARTrackedImage updatedImage)
    {   //물건돌려주기를 시작했는지 확인하기 위해 GameManager_Thief에 접근해서 isStart 값 확인
        bool isStart = GameObject.Find("GameManager").GetComponent<GameManager_Thief>().isStart;
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {
                //도둑일 때 오브젝트 활성화 기능
                if (updatedImage.referenceImage.name.Contains("thief"))
                {
                    if (!isStart)   //돌려주기가 시작되지 않은 경우 도둑이미지를 인식하면 아이템 활성화 
                    {
                        prefab.transform.position = updatedImage.transform.position;
                        prefab.transform.rotation = updatedImage.transform.rotation;
                        //나타날 오브젝트는 Quad인데 90도 회전한 상태로 나타나야하므로 90 회전한 후 활성화
                        prefab.transform.Rotate(90, 0, 0, Space.Self);
                        prefab.SetActive(true);
                    }
                    else                //돌려주기가 시작된 경우
                    {
                        int index = 0;
                        //현재 인식된 도둑 이미지의 번호를 확인함
                        if (updatedImage.referenceImage.name.Contains("1")) { index = 0; }
                        else if (updatedImage.referenceImage.name.Contains("2")) { index = 1; }
                        else if (updatedImage.referenceImage.name.Contains("3")) { index = 2; }
                        //인식된 번호를 GameManager_Thief의 thiefNow 값으로 변경
                        GameObject.Find("GameManager").GetComponent<GameManager_Thief>().thiefNow = index;
                    }
                }
                else if (updatedImage.referenceImage.name.Contains("lostperson"))
                {
                    PersonAllSetFalse();
                    bool isCheckItem;
                    int index=0;
                    if (updatedImage.referenceImage.name.Contains("1")) 
                    { index = 0; }
                    else if (updatedImage.referenceImage.name.Contains("2")) 
                    { index = 1; }
                    else if (updatedImage.referenceImage.name.Contains("3")) 
                    { index = 2; }

                    isCheckItem = GameObject.Find("GameManager").
                        GetComponent<GameManager_Thief>().isReturn[index];
                    GameObject.Find("GameManager").
                        GetComponent<GameManager_Thief>().personNow = index;

                    if (isCheckItem)
                    {
                        GameObject arItem = arobjects["findversion" + (index + 1)];
                        arItem.transform.position 
                            = updatedImage.transform.position;
                        arItem.transform.rotation 
                            = updatedImage.transform.rotation;
                        arItem.transform.Rotate(90, 0, 0, Space.Self);
                        arItem.SetActive(true);
                    }
                    else if (isStart)  
                    {
                        prefab.transform.position 
                            = updatedImage.transform.position;
                        prefab.transform.rotation 
                            = updatedImage.transform.rotation;
                        prefab.transform.Rotate(90, 0, 0, Space.Self);
                        prefab.SetActive(true);
                    }
                }
            }
        }
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updateImage.referenceImage.name, out GameObject prefab))
            {
                prefab.SetActive(false);
            }
        }
    }
    public void AllSetFalse()  //도둑이 가진 아이템 오브젝트를 모두 비활성화
    {
        for(int i=1; i <= 3; i++)
        {
            GameObject arItem = arobjects["thief" + i];
            arItem.SetActive(false);
        }
    }
    public void PersonAllSetFalse()    //웃는 주민의 얼굴을 모두 비활성화
    {                                  //오브젝트가 공중에 계속 남아있는 것을 방지하기 위함
        for (int i = 1; i <= 3; i++)
        {
            GameObject arItem = arobjects["findversion" + i];
            arItem.SetActive(false);
        }
    }
}
