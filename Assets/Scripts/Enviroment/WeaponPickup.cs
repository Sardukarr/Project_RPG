using RPG.Control;
using RPG.Core;
using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Enviroment
{

public class WeaponPickup : MonoBehaviour, IRaycastable
{
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] float maxDistanceToPickUp = 3f;



        private void OnTriggerEnter(Collider other)
            {
            if (other.CompareTag(Config.playerTag))
            {
               // PickItUp(other.GetComponent<Fighter>());
            }
        }
        private void PickItUp(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            float distance = Vector3.Distance(callingController.transform.position, transform.position);
            if (Input.GetMouseButtonDown(0))
                if (distance < maxDistanceToPickUp)
                    PickItUp(callingController.GetComponent<Fighter>());
                else
                    callingController.onDistancePickupClick(transform.position, maxDistanceToPickUp);
           
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.pickup;
        }
    }
}