using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class GhostTapHandler : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GhostManager ghostManager;

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        ghostManager = FindObjectOfType<GhostManager>();
    }

    void Update()
    {
        // Only respond to taps if seeker mode is active
        if (!GameModeManager.Instance.isSeeking) return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Ghost"))
                {
                    GhostBehavior ghost = hit.collider.GetComponent<GhostBehavior>();
                    if (ghost != null)
                    {
                        ghost.Collect();
                        ghostManager.GhostFound(); // Notify manager
                    }
                }
            }
        }
    }
}
