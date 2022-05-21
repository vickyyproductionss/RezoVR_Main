using UnityEngine;

public class FollowGyro : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] private Quaternion baseRotation = new Quaternion(0, 0, 1, 0);

    private Gyroscope gyro;
    private bool gyroActive;

    private void Start()
    {
        GyroManager.Instance.EnableGyro();
    }

    private void Update()
    {
        transform.localRotation = GyroManager.Instance.GetGyroRotation() * baseRotation;
        //GyroModifyCamera();
    }
    void GyroModifyCamera()
    {
        transform.rotation = GyroToUnity(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
