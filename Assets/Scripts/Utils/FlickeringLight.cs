using UnityEngine;
using UnityEngine.Assertions;

namespace ScarabPuzzle
{
    public class FlickeringLight : MonoBehaviour
    {
        [SerializeField] private float _minIntensity = 0.5f;
        [SerializeField] private float _maxIntensity = 1.5f;
        [SerializeField] private float _flickerSpeed = 0.1f;
        [SerializeField] private Material _materialFlicking;
        [SerializeField] private MeshRenderer _lightOrb;
        [SerializeField] private Color[] _flickerColors;
        [SerializeField] private float _colorChangeSpeed = 1f;

        private Light _pointLight;
        private float _colorLerpTimer = 0f;
        private int _currentColorIndex;
        private Color _startColor;
        private Color _endColor;
        private Material _materialInstance;

        private void Awake()
        {
            Assert.IsNotNull(_materialFlicking);
            Assert.IsNotNull(_lightOrb);

            _pointLight = GetComponent<Light>();
            _materialInstance = new Material(_materialFlicking);
            _lightOrb.material = _materialInstance;
        }

        private void OnDestroy()
        {
            Destroy(_materialInstance);
        }

        private void Start()
        {
            _startColor = _flickerColors[0];
            _endColor = _flickerColors[1];
        }

        private void Update()
        {
            FlickerIntensity();
            ChangeColor();
        }

        private void FlickerIntensity()
        {
            float intensity = Random.Range(_minIntensity, _maxIntensity);
            _pointLight.intensity = Mathf.Lerp(_pointLight.intensity, intensity, _flickerSpeed * Time.deltaTime);
        }

        private void ChangeColor()
        {
            _colorLerpTimer += Time.deltaTime * _colorChangeSpeed;
            if (_colorLerpTimer >= 1f)
            {
                _colorLerpTimer = 0f;
                _currentColorIndex = (_currentColorIndex + 1) % _flickerColors.Length;
                _startColor = _pointLight.color;
                _endColor = _flickerColors[_currentColorIndex];
            }

            _pointLight.color = Color.Lerp(_startColor, _endColor, _colorLerpTimer);
            _materialInstance.color = Color.Lerp(_startColor, _endColor, _colorLerpTimer);
        }
    }
}