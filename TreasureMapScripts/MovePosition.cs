using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class MovePosition : MonoBehaviour   //위치 이동하는 클래스
{
    private ARTrackedImageManager arTrackedImageManager;
    public XRReferenceImageLibrary xRReferenceImageLibrary; //위치 이미지를 저장하고 있는 래퍼런스
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    private void Start()
    {
        if (xRReferenceImageLibrary != null)
        {   //ARTrackedImageManager에 현재 필요한 래퍼런스를 연결
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
    {   //이미지를 인식한 경우 CheckPosition() 함수를 호출함
        //매개변수로는 현재 인식된 레퍼런스 이미지의 이름을 전달함
        GameObject.Find("GameManager").GetComponent<GameManagerMove>().CheckPosition(updatedImage.referenceImage.name);
        
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        
    }
}
