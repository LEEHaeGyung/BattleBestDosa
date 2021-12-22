using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class MovePosition : MonoBehaviour   //��ġ �̵��ϴ� Ŭ����
{
    private ARTrackedImageManager arTrackedImageManager;
    public XRReferenceImageLibrary xRReferenceImageLibrary; //��ġ �̹����� �����ϰ� �ִ� ���۷���
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    private void Start()
    {
        if (xRReferenceImageLibrary != null)
        {   //ARTrackedImageManager�� ���� �ʿ��� ���۷����� ����
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
    {   //�̹����� �ν��� ��� CheckPosition() �Լ��� ȣ����
        //�Ű������δ� ���� �νĵ� ���۷��� �̹����� �̸��� ������
        GameObject.Find("GameManager").GetComponent<GameManagerMove>().CheckPosition(updatedImage.referenceImage.name);
        
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        
    }
}
