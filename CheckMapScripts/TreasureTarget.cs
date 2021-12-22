using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class TreasureTarget : MonoBehaviour
{
    public GameObject right;        //������ �̹����� ��Ÿ�� �� ������Ʈ
    public GameObject left;         //���� �̹����� ��Ÿ�� �� ������Ʈ
    public XRReferenceImageLibrary xRReferenceImageLibrary; //���� ������ �ν��� �̹��� ���̺귯��
    private ARTrackedImageManager arTrackedImageManager;    //AR Session Origin�� ���Ե� ARTrackedImageManager
    //���ڿ��� Prefab�� ������ ��ųʸ�, ��ųʸ��� ����� �̸��� ���� ������Ʈ�� Ȱ��ȭ��
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
        {   //ARTrackedImageManager�� ����� ���۷��� �̹����� ����
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
        {   //�̹����� ó�� �ν��� ����
            UpdateTrackingGameObject(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {   //�ν��� �̹����� ����� ����
            if (trackedImage.trackingState == TrackingState.Tracking)
            {   //�̹����� �����ǰ� �ִ� ����
                UpdateTrackingGameObject(trackedImage);
            }
            else if (trackedImage.trackingState == TrackingState.None || trackedImage.trackingState == TrackingState.Limited)
            {   //�����Ǵ� �̹����� ������ų� �ƿ� �νĵ��� ���� ����
                UpdateNoneGameObject(trackedImage);
            }
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {   //�ν��� �̹����� ����� ����
            UpdateNoneGameObject(trackedImage);
        }
    }
    private void UpdateTrackingGameObject(ARTrackedImage updatedImage)
    {
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {   //��ųʸ����� �νĵ� �̹����� �̸��� ���� ������Ʈ�� ã�Ƽ�
                //�̹����� ��ġ�� ��ġ�� ������ �� �ش� ������Ʈ�� Ȱ��ȭ��
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
            {   //����� �̹����� ���� �̸��� ���� ������Ʈ�� ��Ȱ��ȭ
                prefab.SetActive(false);
            }
        }
    }
}