using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField]
    private int _score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(other.gameObject);

            _score++;

            Debug.LogFormat("Current Score: {0}", _score);

            if (_score >= 3)
            {
                Debug.Log("You Won!");               
            }
            
        }
    }
    
}
