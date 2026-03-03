using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Tooltip("Поворот за кадр (типа Vector3). Настраивается в инспекторе.")]
    [SerializeField]
    private Vector3 _rotate = Vector3.zero;

    private Rigidbody _rb;


    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null )
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }

        _rb.isKinematic = true;

        StartCoroutine(RotateContinuously());
    }

    private IEnumerator RotateContinuously()
    {
        while (true)
        {
            Vector3 delta = _rotate * Time.deltaTime;
            _rb.MoveRotation(_rb.rotation * Quaternion.Euler(delta));

            yield return null;
        }
    }


    void Update()
    {
        
    }
}
