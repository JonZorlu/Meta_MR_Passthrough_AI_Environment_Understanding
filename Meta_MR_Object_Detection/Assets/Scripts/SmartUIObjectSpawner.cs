using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.BuildingBlocks.AIBlocks;

public class SmartUIObjectSpawner : MonoBehaviour
{
    [SerializeField] private ObjectDetectionAgent _agent;
    [SerializeField] private ObjectDetectionVisualizerV2 _visualizer;
    [SerializeField] private GameObject _uiInstance;

    [Tooltip("Target MS COCO labels, ordered by priority. e.g., 'remote', 'cell phone'")]
    [SerializeField] private List<string> _targetLabels = new List<string> { "remote", "cell phone" };

    [Tooltip("Lerp speed to prevent jitter from detection frame discrepancies.")]
    [SerializeField] private float _lerpSpeed = 5f;

    [Tooltip("Position offset relative to the detected object's world center.")]
    [SerializeField] private Vector3 _spawnOffset = new Vector3(0, 0.2f, 0);

    private bool _hasSpawned = false;
    private Vector3 _currentTargetPosition;
    private Quaternion _currentTargetRotation;
    private Coroutine _followCoroutine;

    private void Awake()
    {
        if (_agent == null) _agent = FindFirstObjectByType<ObjectDetectionAgent>(FindObjectsInactive.Include);
        if (_visualizer == null) _visualizer = FindFirstObjectByType<ObjectDetectionVisualizerV2>(FindObjectsInactive.Include);
        if (_uiInstance == null) _uiInstance = GameObject.Find("GalleryCanvas_v3");
    }

    private void OnEnable()
    {
        if (_agent != null)
        {
            _agent.OnBoxesUpdated += OnBoxesUpdated;
        }
    }

    private void OnDisable()
    {
        if (_agent != null)
        {
            _agent.OnBoxesUpdated -= OnBoxesUpdated;
        }
        if (_followCoroutine != null)
        {
            StopCoroutine(_followCoroutine);
            _followCoroutine = null;
        }
    }

    private void OnBoxesUpdated(List<BoxData> boxes)
    {
        if (_visualizer == null || _uiInstance == null)
        {
            return;
        }

        var foundTarget = false;
        var targetPos = Vector3.zero;
        var targetRot = Quaternion.identity;

        foreach (var label in _targetLabels)
        {
            foreach (var box in boxes)
            {
                if (box.label.ToLower().Contains(label.ToLower()))
                {
                    // TryProject computes the 3D world position by raycasting the bounding box center against the environment depth.
                    if (_visualizer.TryProject(box.position.x, box.position.y, box.scale.x, box.scale.y, out var world, out var rot, out var scale))
                    {
                        targetPos = world + _spawnOffset;
                        targetRot = rot;
                        foundTarget = true;
                        break;
                    }
                }
            }
            if (foundTarget) break;
        }

        if (foundTarget)
        {
            _currentTargetPosition = targetPos;
            _currentTargetRotation = targetRot;

            if (!_hasSpawned)
            {
                _uiInstance.transform.position = _currentTargetPosition;
                _uiInstance.transform.rotation = _currentTargetRotation;
                _uiInstance.SetActive(true);
                _hasSpawned = true;

                if (_followCoroutine != null) StopCoroutine(_followCoroutine);
                _followCoroutine = StartCoroutine(SmoothFollowRoutine());
            }
        }
    }

    private IEnumerator SmoothFollowRoutine()
    {
        while (_uiInstance != null)
        {
            if (_hasSpawned)
            {
                _uiInstance.transform.position = Vector3.Lerp(_uiInstance.transform.position, _currentTargetPosition, Time.deltaTime * _lerpSpeed);
                _uiInstance.transform.rotation = Quaternion.Slerp(_uiInstance.transform.rotation, _currentTargetRotation, Time.deltaTime * _lerpSpeed);
            }
            yield return null;
        }
    }
}
