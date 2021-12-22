using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class CheckBokItem : MonoBehaviour   //복주머니 인식에 사용되는 클래스
{
    public GameObject[] bokItem;            //복주머니에 들어있는 물건
    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<string, GameObject> arobjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        //복주머니 안에 있는 물건을 랜덤으로 정하기 위해 itemIndex를 셔플
        //복주머니 안에 있는 물건을 랜덤으로 정하기 위해 itemIndex를 셔플
        int[] itemIndex = { 0,1,2 };
        for (int i = 0; i < itemIndex.Length; i++)
        {
            int a = Random.Range(0, itemIndex.Length);
            int b = Random.Range(0, itemIndex.Length);
            int temp;

            temp = itemIndex[a];
            itemIndex[a] = itemIndex[b];
            itemIndex[b] = temp;
        }
        for (int i = 1; i <= bokItem.Length; i++)
        {   //itemIndex[i-1]의 값에 따라 오브젝트를 인스턴스화 하고 딕셔너리에 저장
            GameObject newARobject = Instantiate(bokItem[itemIndex[i-1]], Vector3.zero, Quaternion.identity);
            newARobject.name = "bok" + i;
            arobjects.Add("bok" + i, newARobject);
            GameObject.Find("GameManager").GetComponent<GameManagerB>().bokitem[i - 1] = itemIndex[i - 1];
        }
    }
    public void Start()
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
    {   //돌려주기가 시작된 여부를 확인
        bool isStart = GameObject.Find("GameManager").GetComponent<GameManagerB>().isStart;
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {
                if (!isStart)   //돌려주기가 시작되지 않았으면 물건을 확인할 수 있도록 오브젝트를 활성화함
                {
                    prefab.transform.position = updatedImage.transform.position;
                    prefab.transform.rotation = updatedImage.transform.rotation;
                    prefab.transform.Rotate(90, 0, 0, Space.Self);
                    prefab.SetActive(true);
                }
                else        //돌려주기가 시작된 경우 오브젝트를 모두 비활성화함
                {
                    for(int j=1; j<=3; j++)
                    {
                        GameObject arItem = arobjects["bok" + j];
                        arItem.SetActive(false);
                    }
                    //GameManagerB의 checkBokName을 현재 인식된 이미지의 이름으로 변경함
                    //GameManagerB의 내부 함수에서 checkBokName의 맨뒤에 있는 숫자를 확인하여 어떤 복주머니가 인식된 건지 판단함
                    GameObject.Find("GameManager").GetComponent<GameManagerB>().checkBokName = updatedImage.referenceImage.name;
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
}
