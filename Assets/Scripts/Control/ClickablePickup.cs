using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] float pickupDistance = 1f;
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp())
            {
                return CursorType.pickup;
            }
            else
            {
                return CursorType.fullPickup;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            float distance = Vector3.Distance(GetComponent<Transform>().position,
                                    callingController.GetComponent<Transform>().position);
            Debug.Log(distance);
            if (distance < pickupDistance)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    pickup.PickupItem();
                }
                return true;
            }
            return false;
        }
    
    }
}
