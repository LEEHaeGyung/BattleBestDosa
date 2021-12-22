using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class CheckDress : MonoBehaviour //도령 옷이미지를 인식하는데 사용할 클래스
{
    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private void Awake()
    {   //오브젝트 활성화가 필요없으므로 이전과 다르게 딕셔너리에 추가하는 코드가 없음
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
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
    {   //현재 인식된 이미지의 이름 문자열 중에 마지막값을 정수값으로 변환함
        int type = int.Parse(updatedImage.referenceImage.name.Substring(7, 1));
        //변환한 정수를 매개변수로 하여 옷을 갈아입히는 함수인 SetDress() 호출
        GameObject.Find("GameManager").GetComponent<GameManagerDD>().SetDress(type-1);
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        return;
    }
}
