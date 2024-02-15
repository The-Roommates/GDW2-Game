using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefinition : MonoBehaviour
{
    //Camera Definition+Zoom
    public Camera _camera;
    public float _cameraFOV = 5f;

    //Info to follow Player
    public Transform _playerPos;

    //DebugInfo
    //public float _playerPosX = _playerPos.position.x;
    //float _playerPosY;

    void Update()
    {
        ZoomIn();

        //Follow Player always
        //_camera.transform.position = new Vector2(_playerPos.position.x, _playerPos.position.y);
        transform.position = new Vector3(_playerPos.position.x, _playerPos.position.y, -10f);

        

    }

    public void ZoomIn()
    {
        _camera.orthographicSize = _cameraFOV;

    }

}
