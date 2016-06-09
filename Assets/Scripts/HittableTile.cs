using UnityEngine;
using System.Collections;

public class HittableTile : MonoBehaviour {

    private bool IsHit = false;

    public bool HitOnShip { get; set; }

    public GameObject WhenHit;

    public GameObject WhenShipHit;
	
	// Should be activated by raycast
	public void isHit() {
        IsHit = true;
	}

    public bool getIsHit()
    {
        return IsHit;
    }

    public void Update()
    {
        if (IsHit && !WhenHit.activeInHierarchy)
        {
            WhenHit.SetActive(true);
        }
        if (IsHit && WhenShipHit && !WhenShipHit.activeInHierarchy)
        {
            WhenShipHit.SetActive(true);
        }
    }
}
