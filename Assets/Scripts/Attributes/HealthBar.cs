using UnityEngine;


namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
        {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground=null;
        private void Update()
        {

            if (health.IsDead())
            { 
                GetComponentInChildren<Canvas>().enabled = false;
                return;
            }
            var HPPercentage = health.GetHealthFraction();
            if (Mathf.Approximately(HPPercentage, 1f))
            {
                GetComponentInChildren<Canvas>().enabled = false;
                return; 
            }
            GetComponentInChildren<Canvas>().enabled = true;
            foreground.localScale = new Vector3(2f * HPPercentage,1f,1f);
           
        }

    }
}