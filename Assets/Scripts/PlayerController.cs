using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField, Range(0,1)]
    private float _dotProductValue = 0.5f;
    [SerializeField]
    private float _mouseSensitivity = 2f;
    [SerializeField]
    private Camera _playerCamera;
    [SerializeField]
    private List<PuzzleController> _puzzleControllers = new List<PuzzleController>();

    private CharacterController _characterController;
    private float _cameraRotationX = 0f;
    private PuzzleController _activePuzzleController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        if (_playerCamera == null)
        {
            _playerCamera = Camera.main;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleMouseInput();
        GetActivePuzzleController();

        if (Input.GetKeyDown(KeyCode.R))
        {
            _activePuzzleController.Reset();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Take the camera rotation into account
        Vector3 cameraForward = _playerCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = _playerCamera.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movement = (cameraForward * vertical + cameraRight * horizontal) * _speed * Time.deltaTime;

        // Add gravity
        movement.y = Physics.gravity.y * Time.deltaTime;

        _characterController.Move(movement);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _cameraRotationX -= mouseY;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -90f, 90f);

        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotationX, 0, 0);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + mouseX, 0);
    }

    private void GetActivePuzzleController()
    {
        foreach (var controller in _puzzleControllers)
        {
            if (IsPlayerFacingPuzzle(controller))
            {
                _activePuzzleController = controller;
            }
        }
    }

    private bool IsPlayerFacingPuzzle(PuzzleController puzzleController)
    {
        Vector3 playerToPuzzle = puzzleController.transform.position - transform.position;
        float dotProduct = Vector3.Dot(transform.forward, playerToPuzzle.normalized);

        return dotProduct > _dotProductValue;
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                VertexController vertexController = hit.collider.GetComponent<VertexController>();
                if (vertexController != null)
                {
                    _activePuzzleController.SelectVertex(vertexController);
                }
            }
        }
    }
}





