using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform _camTransform;

    [SerializeField]
    public float _shakeDuration = 0f;
    [SerializeField]
    public float _shakeAmount = 0.7f;
    [SerializeField]
    public float _decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (_camTransform == null)
        {
            _camTransform = GetComponent(typeof(Transform)) as Transform;
            //NEED CLARIFICATION ON TYPE OF ... 
        }
    }

    private void OnEnable()
    {
        originalPos = _camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeDuration > 0)
        {
            _camTransform.localPosition = originalPos + Random.insideUnitSphere * _shakeAmount;

            _shakeDuration = Time.deltaTime * _decreaseFactor;
        }
        else
        {
            _shakeDuration = 0f;
            _camTransform.localPosition = originalPos;
        }

    }
}
