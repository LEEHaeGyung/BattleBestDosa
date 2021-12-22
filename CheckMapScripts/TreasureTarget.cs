using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class TreasureTarget : MonoBehaviour
{
    public GameObject right;        //오른쪽 이미지에 나타날 구 오브젝트
    public GameObject left;         //왼쪽 이미지에 나타날 구 오브젝트
    public XRReferenceImageLibrary xRReferenceImageLibrary; //현재 씬에서 인식할 이미지 라이브러리
    private ARTrackedImageManager arTrackedImageManager;    //AR Session Origin에 포함된 ARTrackedImageManager
    //문자열과 Prefab을 저장할 딕셔너리, 딕셔너리에 저장된 이름에 따라 오브젝트를 활성화함
    private Dictionary<string, GameObject> arobjects = new Dictionary<string, GameObject>();
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        for (int i = 1; i <= 5; i++)
        { 
            GameObject newARobject 
                = Instantiate(right, new Vector3(0, i, 0), Quaternion.identity);
            newARobject.name = "right" + i;
            arobjects.Add("right" + i, newARobject);
        }
        for (int i = 1; i <= 5; i++)
        {
            GameObject newARobject 
                = Instantiate(left, new Vector3(1, i, 0), Quaternion.identity);
            newARobject.name = "left" + i;
            arobjects.Add("left" + i, newARobject);
        }
    }
    public void Start()
    {
        if (xRReferenceImageLibrary != null)
        {   //ARTrackedImageManager에 연결된 레퍼런스 이미지를 변경
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
        {   //이미지를 처음 인식한 상태
            UpdateTrackingGameObject(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {   //인식한 이미지가 변경된 상태
            if (trackedImage.trackingState == TrackingState.Tracking)
            {   //이미지가 추적되고 있는 상태
                UpdateTrackingGameObject(trackedImage);
            }
            else if (trackedImage.trackingState == TrackingState.None || trackedImage.trackingState == TrackingState.Limited)
            {   //추적되던 이미지가 사라지거나 아예 인식되지 않은 상태
                UpdateNoneGameObject(trackedImage);
            }
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {   //인식할 이미지가 사라진 상태
            UpdateNoneGameObject(trackedImage);
        }
    }
    private void UpdateTrackingGameObject(ARTrackedImage updatedImage)
    {
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {   //딕셔너리에서 인식된 이미지의 이름과 같은 오브젝트를 찾아서
                //이미지의 위치로 위치를 변경한 후 해당 오브젝트를 활성화함
                prefab.transform.position = updatedImage.transform.position;
                prefab.transform.rotation = updatedImage.transform.rotation;
                prefab.SetActive(true);
            }
        }
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updateImage.referenceImage.name, out GameObject prefab))
            {   //사라진 이미지와 같은 이름의 게임 오브젝트를 비활성화
                prefab.SetActive(false);
            }
        }
    }
}