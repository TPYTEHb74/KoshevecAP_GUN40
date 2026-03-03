using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 _start;
    [SerializeField] private Vector3 _end;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _delay = 0f;

    private Rigidbody _rb;
    private bool _forward = true; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        _rb.isKinematic = true;

        transform.position = _start;

        StartCoroutine(MoveBetweenPoints());
    }

    private IEnumerator MoveBetweenPoints()
    {
        
        Vector3 from = _start;
        Vector3 to = _end;

        while (true)
        {          
            float t = 0f;
            float dist = Vector3.Distance(from, to);
            float duration = dist / Mathf.Max(_speed, 0.0001f);

            while (t < duration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Clamp01(t / duration);
                Vector3 pos = Vector3.Lerp(from, to, alpha);
                _rb.MovePosition(pos);
                yield return null;
            }

            if (_delay > 0f)
                yield return new WaitForSeconds(_delay);

            Vector3 temp = from;
            from = to;
            to = temp;
        }
    }
}
