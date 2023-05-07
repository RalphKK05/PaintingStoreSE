using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacePainting : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private ARAnchorManager _anchorManager;
    [SerializeField] private GameObject _paintingPrefab;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _currentPainting;
    public PaintingScale paintingScale;

    public GameObject image;
    public Renderer imageRenderer;

    public float X, Y;
    public Texture2D paintingImage;

    void Start()
    {
        //Get the image of the whole painting (Child of child of parent prefab)
        image = _paintingPrefab.transform.GetChild(0).GetChild(0).gameObject;
        imageRenderer = image.GetComponent<Renderer>();
    }

    void Update()
    {
        //Update dimensions and image depending on the painting the user is inspectingS
        X = paintingScale.Xvalue;
        Y = paintingScale.Yvalue;
        paintingImage = paintingScale.spriteImage.texture;

        // check if the user touches the screen to place painting
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Destroy the previously instantiated painting
            if (_currentPainting != null)
            {
                Destroy(_currentPainting);
            }

            // cast a ray from the touch position into the AR environment
            var touchPosition = Input.GetTouch(0).position;
            if (_raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinBounds))
            {
                // get the first hit result
                var hit = _hits[0];

                // create a new anchor at the hit pose
                var anchorGO = new GameObject("Anchor");
                anchorGO.transform.position = hit.pose.position;
                anchorGO.transform.rotation = hit.pose.rotation;
                var anchor = anchorGO.AddComponent<ARAnchor>();

                _paintingPrefab.transform.localScale = new Vector3(X, Y, 1f);


                // set painting image
                imageRenderer.material.mainTexture = paintingImage;

                // instantiate the painting prefab and attach it to the anchor
                var painting = Instantiate(_paintingPrefab, anchor.transform);

                // Make the painting face the plane
                painting.transform.LookAt(hit.pose.position + hit.pose.up, hit.pose.up);

                // Keep track of the currently instantiated painting
                _currentPainting = painting;
            }
        }
    }
}
