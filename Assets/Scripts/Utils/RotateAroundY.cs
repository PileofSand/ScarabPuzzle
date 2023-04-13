using UnityEngine;

namespace ScarabPuzzle
{
    public class RotateAroundY : MonoBehaviour
    {
        [SerializeField] 
        private float _rotationSpeed = 10f;

        private void Update()
        {
            float rotationAmount = _rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }
}