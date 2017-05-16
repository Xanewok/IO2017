using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GameSettings
{
    public CameraSettings cameraSettings;
}

[System.Serializable]
public class CameraSettings
{
    public enum CameraType
    {
        TopDown,
        SlightlyAngled,
        Isometric,
    }

    public const CameraType DefaultCameraType = CameraType.SlightlyAngled;

    [System.Serializable]
    public struct CameraPreset
    {
        public float followSpeed;
        public float distance;
        public Vector3 rotation;
    }

    static Dictionary<CameraType, CameraPreset> presets = new Dictionary<CameraType, CameraPreset>()
    {
        { CameraType.TopDown, new CameraPreset { followSpeed = 0.035f, distance = 10.0f, rotation = Vector3.zero } },
        { CameraType.SlightlyAngled, new CameraPreset { followSpeed = 0.035f, distance = 10.0f, rotation = new Vector3(-15, -15, 0) } },
        { CameraType.Isometric, new CameraPreset { followSpeed = 0.035f, distance = 15.0f, rotation = new Vector3(-45, -45, 0) } },
    };

    [SerializeField]
    private CameraType m_currentType = DefaultCameraType;
    public CameraType currentType
    {
        get
        {
            return m_currentType;
        }
        set
        {
            m_currentType = value;
            cameraPreset = presets[value];
        }
    }
    public CameraPreset cameraPreset = presets[DefaultCameraType];
}
