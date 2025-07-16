using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Coroutine _enabledCoroutine;
    private ColorChanger _colorChanger = new()                                                                                                      ;

    private bool _isTouched;
    private float _minDelay = 2f;
    private float _maxDelay = 5f;

    public Renderer Renderer { get; private set; }
    public event Action<Cube> ReturnToPool;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched || collision.collider.TryGetComponent<Platform>(out _) == false)
            return;

        _isTouched = true;

        _colorChanger.SetRandomColor(Renderer);
        _enabledCoroutine = StartCoroutine(DestroyWithDelay());
    }

    public void SetDefault()
    {
        _isTouched = false;
        _colorChanger.SetDefaultColor(Renderer);

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
            ReturnToPool?.Invoke(this);
    }
}
