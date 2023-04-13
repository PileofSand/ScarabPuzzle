using ScarabPuzzle.Audio;
using ScarabPuzzle.Edges;
using ScarabPuzzle.Vertices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ScarabPuzzle
{
    public class PuzzleController : MonoBehaviour
    {
        [SerializeField]
        private List<VertexController> _vertexControllers;
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private AudioClip _rightMoveClip;
        [SerializeField]
        private AudioClip _wrongMoveClip;
        [SerializeField]
        private AudioClip _winnerClip;
        [SerializeField]
        private GameObject _winnerObject;
        [SerializeField]
        private float _lineMoveSpeed = 10f;

        private VertexController _selectedVertexController;

        private int _currentIndex = 0;
        private Vector3 _lastAddedPoint;

        public Graph Graph { get; private set; }
        public Path CurrentPath { get; private set; }

        private void Awake()
        {
            Assert.IsNotNull(_vertexControllers);
            Assert.IsNotNull(_lineRenderer);

            Graph = new Graph();
            CurrentPath = new Path();

            for (int i = 0; i < _vertexControllers.Count; i++)
            {
                VertexController vertexController = _vertexControllers[i];
                vertexController.Initialize();
                Graph.AddVertex(vertexController.Vertex);
            }
        }

        private void Start()
        {
            Graph.GenerateEdges(_vertexControllers);
        }

        public void SelectVertex(VertexController vertexController)
        {
            if (_selectedVertexController == null)
            {
                _selectedVertexController = vertexController;
                _selectedVertexController.Select();
                AddLinePoint(0, _selectedVertexController.transform.position);
                return;
            }

            Edge edge = Graph.GetEdge(_selectedVertexController.Vertex, vertexController.Vertex);
            if (edge != null && !CurrentPath.Contains(edge))
            {
                CurrentPath.AddEdge(edge);
                vertexController.Vertex.ConnectTo(_selectedVertexController.Vertex);
                _selectedVertexController.Vertex.ConnectTo(vertexController.Vertex);
                _selectedVertexController.Deselect();
                vertexController.Select();
                _selectedVertexController = vertexController;
                AudioManager.Instance.PlayClip(_rightMoveClip);

                if (_lastAddedPoint == edge.VertexAPosition)
                {
                    AddLinePoint(_currentIndex, edge.VertexBPosition);
                }
                else
                {
                    AddLinePoint(_currentIndex, edge.VertexAPosition);
                }

                // Check if the puzzle is solved
                if (IsPuzzleSolved())
                {
                    // Display a message and trigger a visual effect
                    StartCoroutine(FinishGame());
                }
            }
            else
            {
                vertexController.SignalWrongMove();
                AudioManager.Instance.PlayClip(_wrongMoveClip);
            }
        }

        public void DeselectVertex()
        {
            if (_selectedVertexController != null)
            {
                _selectedVertexController.Deselect();
                _selectedVertexController = null;
            }
        }

        public void Reset()
        {
            CurrentPath.Reset();
            DeselectVertex();
            _lineRenderer.positionCount = 0;
            _currentIndex = 0;
            _winnerObject.SetActive(false);

            foreach (var vertexController in _vertexControllers)
            {
                vertexController.Reset();
                vertexController.Vertex.Reset();
            }
        }

        private void AddLinePoint(int id, Vector3 position)
        {
            _lineRenderer.positionCount = id + 1;
            StartCoroutine(MoveLineRenderer(id, position));
            _currentIndex++;
            _lastAddedPoint = position;
        }

        private bool IsPuzzleSolved()
        {
            return CurrentPath.Edges.Count == Graph.Edges.Count - 1;
        }

        private IEnumerator MoveLineRenderer(int id, Vector3 targetPosition)
        {
            if (id <= 0)
            {
                _lineRenderer.SetPosition(id, targetPosition);
                yield break;
            }

            Vector3 startPosition = _lineRenderer.GetPosition(id - 1);
            Vector3 currentEndPosition = startPosition;

            while (Vector3.Distance(currentEndPosition, targetPosition) > 0.01f)
            {
                currentEndPosition = Vector3.Lerp(currentEndPosition, targetPosition, _lineMoveSpeed * Time.deltaTime);
                _lineRenderer.SetPosition(id, currentEndPosition);
                yield return null;
            }

            // Set the final position to make sure it reaches the target
            _lineRenderer.SetPosition(id, targetPosition);
        }

        private IEnumerator FinishGame()
        {
            AudioManager.Instance.PlayClip(_winnerClip);
            _winnerObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            Reset();
        }
    }
}