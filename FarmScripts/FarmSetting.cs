using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class FarmSetting : MonoBehaviour
{
    public GameObject farmObject;   //밭 오브젝트

    public XRReferenceImageLibrary xRReferenceImageLibrary;
    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<string, GameObject> arobjects = new Dictionary<string, GameObject>();
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
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
    {   //밭 이미지가 인식되면 밭 오브젝트 활성화
        farmObject.transform.position = updatedImage.transform.position;
        farmObject.transform.rotation = updatedImage.transform.rotation;
        farmObject.transform.Rotate(90, 0, 0, Space.Self);
        farmObject.SetActive(true);
    }
    private void UpdateNoneGameObject(ARTrackedImage updateImage)
    {   //밭 이미지가 사라지면 밭 오브젝트 비활성화
        farmObject.SetActive(false);
    }
}