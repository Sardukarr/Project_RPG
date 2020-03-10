using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{ 
    public class PlayerController : MonoBehaviour
    {


        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMashProjectionDistance = 1f;
        [SerializeField] float cursorBlinkTime = 1f;
        [SerializeField] float reycastRadius = 1f;


        // public delegate void OnDistancePickupClick(Vector3 position, float distance);
        // OnDistancePickupClick onDistancePickupClick;

        float TimeSinceMovementCursorChange = 0f;
        bool isDraggingOverUI = false;

        private CursorType currenMovmentCursor= CursorType.movement;
        Mover myMover;
        Fighter myFighter;
        private Health myHealth;

        
        private void Awake()
        {
            myMover = GetComponent<Mover>();
            myFighter = GetComponent<Fighter>();
            myHealth = GetComponent<Health>();
        }
        void Start()
        {
        }
    
        void Update()
        {
            TimeSinceMovementCursorChange += Time.deltaTime;

            // TODO: temporary quit
            if (Input.GetKey(KeyCode.Escape)) 
                Application.Quit();

            //SetCursor(CursorType.none);
            if (interactWithUI())
              {
                SetCursor(CursorType.UI);
                return;
            }
            if (myHealth.IsDead()) return;
            // if (Input.GetMouseButton(0))
            //  {
            if (InteractWithComponent()) return; 

          //  if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.none);
            //  }

        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RayCastAllSort();
            foreach (var hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithCombat()
        {
            return false;
        }

        private bool InteractWithMovement()
        {
         //    bool isThereValidPlaceToGo=false;
            // if (Physics.Raycast(GetMauseRey(), out RaycastHit hit))
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (!hasHit) return false;

            if (!myMover.CanMoveTo(target)) return false;
            if (Input.GetMouseButton(0))
                {
                myMover.StartMoveAction(target, 1f);
                  
                }
            //    isThereValidPlaceToGo = true;
                if (TimeSinceMovementCursorChange > cursorBlinkTime)
                {
                    currenMovmentCursor = (CursorType)(((int)currenMovmentCursor + 1) % 2);
                    SetCursor(currenMovmentCursor);
                    TimeSinceMovementCursorChange = 0f;
                }
            else SetCursor(currenMovmentCursor);
            return hasHit;
        }
        private bool interactWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDraggingOverUI = false;
            }
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingOverUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }
            if (isDraggingOverUI)
            {
                return true;
            }
            return false;
        }
        private bool RaycastNavMesh(out Vector3 target) 
        {
            RaycastHit hit;
         //    if (Physics.Raycast(GetMauseRey(), out  hit))
            target = new Vector3();
            bool hasHit = Physics.Raycast(GetMauseRey(), out  hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh =NavMesh.SamplePosition(hit.point,
                                                    out navMeshHit,
                                                    maxNavMashProjectionDistance,
                                                    NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;

            //NavMeshPath path = new NavMeshPath();
            //bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            //if (!hasPath) return false;
            //if (path.status!=NavMeshPathStatus.PathComplete) return false;
            //if (GetPathLength(path) > MaxNavMeshPathLength) return false;

            return true;
        }
        public void onDistancePickupClick(Vector3 position, float maxDistanceToPickUp)
        {
            myMover.MoveWithinRange(position, maxDistanceToPickUp);
        }
      

        private static Ray GetMauseRey()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        RaycastHit[] RayCastAllSort()
        {
            var hits = Physics.SphereCastAll(GetMauseRey(),reycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot,CursorMode.Auto);
        }
        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (var mapping in cursorMappings)
            {
                if (mapping.type == cursorType)
                    return mapping;
               
            }
            return cursorMappings[0];
        }
    }
}