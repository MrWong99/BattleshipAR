using UnityEngine;
using Vuforia;
using System.Collections;

public class PrefabInstantiator : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour tb;
    public Transform boardPrefab;
    // Use this for initialization
    void Start()
    {
        tb = GetComponent<TrackableBehaviour>();
        if (tb)
        {
            tb.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (boardPrefab != null)
            {
                Transform instantiatedBoard = Instantiate(boardPrefab) as Transform;
                instantiatedBoard.parent = tb.transform;
                instantiatedBoard.localPosition = Vector3.zero;
                instantiatedBoard.localRotation = Quaternion.identity;
                instantiatedBoard.localScale = Vector3.one;
                instantiatedBoard.gameObject.SetActive(true);
            }
        }
    }
}
