using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class CheckDress : MonoBehaviour //���� ���̹����� �ν��ϴµ� ����� Ŭ����
{
    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private void Awake()
    {   //������Ʈ Ȱ��ȭ�� �ʿ�����Ƿ� ������ �ٸ��� ��ųʸ��� �߰��ϴ� �ڵ尡 ����
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
    {   //���� �νĵ� �̹����� �̸� ���ڿ� �߿� ���������� ���������� ��ȯ��
        int type = int.Parse(updatedImage.referenceImage.name.Substring(7, 1));
        //��ȯ�� ������ �Ű������� �Ͽ� ���� ���������� �Լ��� SetDress() ȣ��
        GameObject.Find("GameManager").GetComponent<GameManagerDD>().SetDress(type-1);
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {
        return;
    }
}
