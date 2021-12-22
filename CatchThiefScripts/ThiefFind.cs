using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ThiefFind : MonoBehaviour  //�����̹����� �ֹ��̹��� �ν��� �����ϴ� Ŭ����
{
    public GameObject[] items;          //�Ҿ���� ����
    public GameObject[] person;         //����ã�Ⱑ �ذ�� ��� ���� ���̹���
    int[] thiefindex = { 0, 1, 2 };     //������ ���İ� �������� �ε���
    int[] personIndex = { 0, 1, 2 };    //�ֹ��� �Ҿ���� ������ �ε���

    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<string, GameObject> arobjects = new Dictionary<string, GameObject>();
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();

        //������ �ν����� �� ��Ÿ�� ����
        for (int i=0; i <= items.Length; i++)       //������ �ε����� �������� ����
        {
            int a = Random.Range(0, items.Length);
            int b = Random.Range(0, items.Length);
            int temp;

            temp = thiefindex[a];
            thiefindex[a] = thiefindex[b];
            thiefindex[b] = temp;
        }
        for (int i = 1; i <= items.Length; i++)
        {   //������ thiefindex[i-1]�ε����� �ش��ϴ� items �迭�� ����� Prefab�� �ν��Ͻ�ȭ
            GameObject newARobject = Instantiate(items[thiefindex[i-1]], Vector3.zero, Quaternion.identity);
            //key stringd�� �� thief1, thief2, thief3���� �Ͽ� ���� �̸��� �̹����� �νĵǸ� �ش� �������� ��Ÿ�������� ��
            newARobject.name = "thief" + i;
            arobjects.Add("thief" + i, newARobject);
        }
        //�ֹ��� �ν����� �� ��Ÿ�� ����
        for (int i = 0; i <= items.Length; i++)     //������ �ε����� �������� ����
        {
            int a = Random.Range(0, items.Length);
            int b = Random.Range(0, items.Length);
            int temp;

            temp = personIndex[a];
            personIndex[a] = personIndex[b];
            personIndex[b] = temp;
        }
        for (int i = 1; i <= items.Length; i++)
        {   //������ ������ ������ ���� ���
            GameObject newARobject = Instantiate(items[personIndex[i - 1]], Vector3.zero, Quaternion.identity);
            newARobject.name = "lostperson" + i;
            arobjects.Add("lostperson" + i, newARobject);
            newARobject.SetActive(false);   //�ʱ⿡�� �ֹ��� �������� ��Ÿ���� �ȵǹǷ� ��� ��Ȱ��ȭ
        }
        for (int i = 0; i < thiefindex.Length; i++)
        {   //�� ���ϰ� �ֹ��� ������ �ε��� ���� GameManager�� GameManager_Thief�� �ִ� �迭�� �� ����
            //������ Ȯ���ϴµ� �ʿ��� �۾�
            GameObject.Find("GameManager").GetComponent<GameManager_Thief>().thiefitemIndex[i] = thiefindex[i];
            GameObject.Find("GameManager").GetComponent<GameManager_Thief>().personitemIndex[i] = personIndex[i];
        }
        for (int i = 1; i <= items.Length; i++)
        {   //�ֹ��� ���� ���� findversion1~findversion3�� string�� key�� �ؼ� ������
            GameObject newARobject = Instantiate(person[i-1], Vector3.zero, Quaternion.identity);
            newARobject.name = "findversion" + i;
            arobjects.Add("findversion" + i, newARobject);
            newARobject.SetActive(false);   //�������� ���� ���¿����� ��Ÿ���� �ȵǹǷ� ��Ȱ��ȭ
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
    {   //���ǵ����ֱ⸦ �����ߴ��� Ȯ���ϱ� ���� GameManager_Thief�� �����ؼ� isStart �� Ȯ��
        bool isStart = GameObject.Find("GameManager").GetComponent<GameManager_Thief>().isStart;
        for (int i = 0; i < arobjects.Count; i++)
        {
            if (arobjects.TryGetValue(updatedImage.referenceImage.name, out GameObject prefab))
            {
                //������ �� ������Ʈ Ȱ��ȭ ���
                if (updatedImage.referenceImage.name.Contains("thief"))
                {
                    if (!isStart)   //�����ֱⰡ ���۵��� ���� ��� �����̹����� �ν��ϸ� ������ Ȱ��ȭ 
                    {
                        prefab.transform.position = updatedImage.transform.position;
                        prefab.transform.rotation = updatedImage.transform.rotation;
                        //��Ÿ�� ������Ʈ�� Quad�ε� 90�� ȸ���� ���·� ��Ÿ�����ϹǷ� 90 ȸ���� �� Ȱ��ȭ
                        prefab.transform.Rotate(90, 0, 0, Space.Self);
                        prefab.SetActive(true);
                    }
                    else                //�����ֱⰡ ���۵� ���
                    {
                        int index = 0;
                        //���� �νĵ� ���� �̹����� ��ȣ�� Ȯ����
                        if (updatedImage.referenceImage.name.Contains("1")) { index = 0; }
                        else if (updatedImage.referenceImage.name.Contains("2")) { index = 1; }
                        else if (updatedImage.referenceImage.name.Contains("3")) { index = 2; }
                        //�νĵ� ��ȣ�� GameManager_Thief�� thiefNow ������ ����
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
    public void AllSetFalse()  //������ ���� ������ ������Ʈ�� ��� ��Ȱ��ȭ
    {
        for(int i=1; i <= 3; i++)
        {
            GameObject arItem = arobjects["thief" + i];
            arItem.SetActive(false);
        }
    }
    public void PersonAllSetFalse()    //���� �ֹ��� ���� ��� ��Ȱ��ȭ
    {                                  //������Ʈ�� ���߿� ��� �����ִ� ���� �����ϱ� ����
        for (int i = 1; i <= 3; i++)
        {
            GameObject arItem = arobjects["findversion" + i];
            arItem.SetActive(false);
        }
    }
}
