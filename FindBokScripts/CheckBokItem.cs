using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class CheckBokItem : MonoBehaviour   //���ָӴ� �νĿ� ���Ǵ� Ŭ����
{
    public GameObject[] bokItem;            //���ָӴϿ� ����ִ� ����
    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<string, GameObject> arobjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        //���ָӴ� �ȿ� �ִ� ������ �������� ���ϱ� ���� itemIndex�� ����
        //���ָӴ� �ȿ� �ִ� ������ �������� ���ϱ� ���� itemIndex�� ����
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
        {   //itemIndex[i-1]�� ���� ���� ������Ʈ�� �ν��Ͻ�ȭ �ϰ� ��ųʸ��� ����
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
    {   //�����ֱⰡ ���۵� ���θ� Ȯ��
        bool isStart = GameObject.Find("GameManager").GetComponent<GameManagerB>().isStart;
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {
                if (!isStart)   //�����ֱⰡ ���۵��� �ʾ����� ������ Ȯ���� �� �ֵ��� ������Ʈ�� Ȱ��ȭ��
                {
                    prefab.transform.position = updatedImage.transform.position;
                    prefab.transform.rotation = updatedImage.transform.rotation;
                    prefab.transform.Rotate(90, 0, 0, Space.Self);
                    prefab.SetActive(true);
                }
                else        //�����ֱⰡ ���۵� ��� ������Ʈ�� ��� ��Ȱ��ȭ��
                {
                    for(int j=1; j<=3; j++)
                    {
                        GameObject arItem = arobjects["bok" + j];
                        arItem.SetActive(false);
                    }
                    //GameManagerB�� checkBokName�� ���� �νĵ� �̹����� �̸����� ������
                    //GameManagerB�� ���� �Լ����� checkBokName�� �ǵڿ� �ִ� ���ڸ� Ȯ���Ͽ� � ���ָӴϰ� �νĵ� ���� �Ǵ���
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
