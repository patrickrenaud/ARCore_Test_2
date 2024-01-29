using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneImageTracker : MonoBehaviour
{
    [SerializeField] private TrackedPrefab[] prefabToInstantiate;

    private Dictionary<string, GameObject> _instanciatedPrefab = new Dictionary<string, GameObject>();

    private ARTrackedImageManager _trackedImageManager;
    private ARPlaneManager _planeManager;

    private Vector3 _foundImagePos = new Vector3();
    private Quaternion _foundImageRot = new Quaternion();
    private bool _imageFound = false;

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnChanged;
    }

    void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnChanged;
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            StartCoroutine(InstantiateGameObject(newImage));
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            for (int i = 0; i < prefabToInstantiate.Length; i++)
            {
                if (updatedImage.referenceImage.name == prefabToInstantiate[i].name)
                {
                    if(updatedImage.transform.position != Vector3.zero)
                    {
                        _foundImagePos = updatedImage.transform.position;
                        _foundImageRot = updatedImage.transform.rotation;
                        _trackedImageManager.enabled = false;
                        _imageFound = true;
                    }
                }
            }
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }

    private IEnumerator InstantiateGameObject(ARTrackedImage addedImage)
    {
        while (!_imageFound)
        {
            yield return null;
        }

        for (int i = 0; i < prefabToInstantiate.Length; i++)
        {
            if (addedImage.referenceImage.name == prefabToInstantiate[i].name)
            {
                ARPlane closestPlane = null;
                float closestDistanceY = float.MaxValue;

                foreach (var plane in _planeManager.trackables)
                {
                    float distanceYToPlane = Mathf.Abs(_foundImagePos.y - plane.center.y);

                    if (distanceYToPlane < closestDistanceY)
                    {
                        closestDistanceY = distanceYToPlane;
                        closestPlane = plane;
                    }
                }

                if (closestPlane != null)
                {
                    Vector3 trackedImagePosition = _foundImagePos;
                    Quaternion trackedImageRotation = _foundImageRot;

                    Vector3 planeNormal = closestPlane.normal;
                    Vector3 planePoint = closestPlane.center;

                    Vector3 closestPointOnPlaneWorld = ClosestPointOnPlane(trackedImagePosition, planeNormal, planePoint);
                    Debug.Log("Point on plane : " + closestPointOnPlaneWorld);

                    Vector3 adjustedPose = new Vector3(trackedImagePosition.x, closestPointOnPlaneWorld.y, trackedImagePosition.z);

                    GameObject prefab = Instantiate(prefabToInstantiate[i].prefab, adjustedPose, trackedImageRotation);

                    _instanciatedPrefab.Add(addedImage.referenceImage.name, prefab);
                }
            }
        }
    }



    Vector3 ClosestPointOnPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint)
    {
        float distance = Vector3.Dot(planeNormal, point - planePoint);
        return point - distance * planeNormal;
    }

    [System.Serializable]
    public struct TrackedPrefab
    {
        public string name;
        public GameObject prefab;
    }
}
