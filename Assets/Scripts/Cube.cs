using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _enabledCoroutine;

    private bool _isReached;
    private float _minDelay = 2f;
    private float _maxDelay = 5f;

    public event Action<Cube> Enabled;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isReached || collision.collider.TryGetComponent<Platform>(out _) == false)
        {
            return;
        }

        _isReached = true;

        ColorChanger.SetRandomColor(_renderer);
        _enabledCoroutine = StartCoroutine(DestroyWithDelay());
    }

    public void SetDefault()
    {
        _isReached = false;
        ColorChanger.SetDefaultColor(_renderer);
        
        if (_enabledCoroutine != null)
        {
            StopCoroutine(_enabledCoroutine);
            _enabledCoroutine = null;
        }
    }

    private IEnumerator DestroyWithDelay()
    {
        float currentDelay = UnityEngine.Random.Range(_minDelay, _maxDelay);

        yield return new WaitForSeconds(currentDelay);

        if (gameObject.activeInHierarchy)
        {
            Enabled?.Invoke(this);
        }
    }
}
