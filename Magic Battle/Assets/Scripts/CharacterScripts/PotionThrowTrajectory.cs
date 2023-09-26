using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrowTrajectory : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Camera _camera;

    public GameObject playerFollowCamera;
    public GameObject playerTrajectoryCamera;
    public GameObject playerAimingCamera;

    [SerializeField]
    private GameObject potionObject;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private Transform releasePosition;
    [Header("Potion Controls")]
    [SerializeField]
    [Range(1, 100)]
    private float throwStrength = 10f;
    [SerializeField]
    [Range(1, 10)]
    private float throwDelay = 0.8f;
    [Header("Display Controls")]
    [SerializeField]
    [Range(10, 100)]
    private int linePoints = 25;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float timeBetweenPoints = 0.1f;
    [SerializeField]
    private StarterAssetsInputs _input;

    private Transform InitialParent;
    private Vector3 InitialLocalPosition;
    private Quaternion InitialRotation;

    private bool _isPotionActive = true;
    private LayerMask PotionCollisionMask;
    private GameObject _mainCamera;

    private void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        InitialParent = potionObject.transform.parent;
        InitialRotation = potionObject.transform.localRotation;
        InitialLocalPosition = potionObject.transform.localPosition;
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();    

        int grenadeLayer = potionObject.gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                PotionCollisionMask |= 1 << i;
            }
        }
    }

    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Standing Death Right 02"))
        {
            return;
        }
        if (Application.isFocused && _input.throwPotion)
        {
            if (!_input.aiming)
            {
                _animator.transform.rotation = Quaternion.Euler(
                    _animator.transform.eulerAngles.x,
                    _camera.transform.rotation.eulerAngles.y,
                    _animator.transform.eulerAngles.z
                );
                playerFollowCamera.SetActive(false);
                playerTrajectoryCamera.SetActive(true);
                DrawProjection();

                if (_input.atack && _isPotionActive)
                {
                    if (_mainCamera.GetComponent<PlayerDataController>().explosivePotionsQuant > 0)
                    {
                        _animator.SetBool("Throw", _input.throwPotion);
                        _isPotionActive = false;
                        GameObject newPotion = Instantiate(potionObject, releasePosition.position, _camera.transform.rotation);
                        newPotion.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * throwStrength, ForceMode.Impulse);
                        StartCoroutine(GenerateNewPotion());
                    }
                }
                else
                {
                    _animator.SetBool("Throw", false);
                }
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }
        else if (_input.aiming)
        {
            playerTrajectoryCamera.SetActive(false);
            playerAimingCamera.SetActive(true);
        }
        else
        {
            playerTrajectoryCamera.SetActive(false);
            playerFollowCamera.SetActive(true);
            _lineRenderer.enabled = false;
        }
    }

    public void DrawProjection()
    {
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = releasePosition.position;
        Vector3 startVelocity = throwStrength * _camera.transform.forward / potionObject.GetComponent<Rigidbody>().mass;
        int i = 0;
        _lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            _lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = _lineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                (point - lastPosition).normalized,
                out RaycastHit hit,
                (point - lastPosition).magnitude,
                PotionCollisionMask))
            {
                _lineRenderer.SetPosition(i, hit.point);
                _lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }

    private IEnumerator GenerateNewPotion()
    {
        yield return new WaitForSeconds(throwDelay);
        _isPotionActive = true;
        _mainCamera.GetComponent<PlayerDataController>().explosivePotionsQuant--;
    }
}
