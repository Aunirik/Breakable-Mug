using UnityEngine;

namespace Aunirik.BreakableMug
{
    public class BreakableMug_Sample_Shooter : MonoBehaviour
    {
        [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField]
        private float velocity;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var projectile = Instantiate(projectilePrefab, ray.origin, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().velocity = ray.direction * velocity;
            }
        }
    }
}
