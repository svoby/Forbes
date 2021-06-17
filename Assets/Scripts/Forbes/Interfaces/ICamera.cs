using UnityEngine;

namespace Forbes.Cameras
{
    public interface ICamera
    {
        void FollowTarget(GameObject _tar, Camera _camera);
    }
}