using UnityEngine;

public class SimplePickupSystem : MonoBehaviour
{
	private GameObject currentItem;

	public void UpdatePickupPosition(Vector3 position)
	{
		if (currentItem == null)
			return;
		
		currentItem.transform.position = position;
	}

	public void Pickup(GameObject item)
	{
		currentItem = item;

		// Disable physics while holding
		Rigidbody rb = item.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = true;
		}
		Collider collider = rb.GetComponent<Collider>();
		if (collider != null) 
		{
			collider.enabled = false;
		}

		Debug.Log("Picked up: " + item.name);
	}

	public void Drop()
	{
		if (currentItem == null) return;

		Rigidbody rb = currentItem.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = false;
		}
		Collider collider = rb.GetComponent<Collider>();
		if (collider != null) 
		{
			collider.enabled = true;
		}

		Debug.Log("Dropped: " + currentItem.name);
		currentItem = null;
	}
}
