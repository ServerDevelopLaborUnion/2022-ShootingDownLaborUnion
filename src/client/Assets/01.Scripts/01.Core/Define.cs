using UnityEngine;
using Cinemachine;

public static class Define
{
    public static Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }

    }

    private static Camera _mainCam;

    public static CinemachineVirtualCamera VCam{
        get{
            if(_vCam == null){
                _vCam = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
            }
            return _vCam;
        }
    }

    private static CinemachineVirtualCamera _vCam;

    public static Vector2 MousePos => MainCam.ScreenToWorldPoint(Input.mousePosition);

    public static Transform FadeParent
    {
        get
        {
            if (_fadeParent == null)
            {
                _fadeParent = GameObject.Find("FadeParent").transform;
            }
            return _fadeParent;
        }
    }

    private static Transform _fadeParent;

}
