using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;
    private bool _cameraShake = false;
    private float _shakeAmount = 0.05f;

    void Update()
    {
        IfDamaged();
        if (_cameraShake == false)
        {
            Vector3 camPos = _mainCamera.transform.position;
            camPos.x = 0;
            camPos.y = 1;

            _mainCamera.transform.position = camPos;
        }
    }

    public void ShakeCamera()
    {
        _cameraShake = true;

    }

    void IfDamaged()
    {
        if (_cameraShake == true)
        {
            Vector3 camPos = _mainCamera.transform.position;
            float offSetX = Random.value * _shakeAmount * 2 - _shakeAmount;
            float offSetY = Random.value * _shakeAmount * 2 - _shakeAmount;
            camPos.x += offSetX;
            camPos.y += offSetY;

            _mainCamera.transform.position = camPos;

            StartCoroutine(StopShaking());

        }
    }

    IEnumerator StopShaking()
    {
        yield return new WaitForSeconds(1.0f);
        _cameraShake = false;
    }

}
