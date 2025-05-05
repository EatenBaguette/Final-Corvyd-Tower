using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine _player;
        
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _player._gameOver = true;
        }
    }
}
