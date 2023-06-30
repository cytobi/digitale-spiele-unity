using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    private float _lastChangeTime = 0.0f;
    private float _changeInterval = 1.0f;
    private bool _isPlayerCamera = false;

    public GameObject aboveCamera;
    public GameObject playerCamera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && Time.time > _lastChangeTime + _changeInterval)
        {
            _lastChangeTime = Time.time;
            _isPlayerCamera = !_isPlayerCamera;
            if (_isPlayerCamera)
            {
                aboveCamera.SetActive(false);
                playerCamera.SetActive(true);
            }
            else
            {
                aboveCamera.SetActive(true);
                playerCamera.SetActive(false);
            }
        }
    }
}
