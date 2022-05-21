using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public static CameraFollow2 instance;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private Transform target;
    public float translationSpeed;
    public Vector3 positionToOrbitAround;
    public float rotationSpeed;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void setTargetTransfrom(Transform t, Vector3 _offset)
    {
        Offset = _offset;
        target = t;
        translationSpeed = 200;
    }
    public void setOffset(Vector3 offset)
    {
        Offset = offset;
    }
    private void Update()
    {
        HandleTranslation();
        HandleRotation();
    }
    public void HandleTranslation()
    {
        Vector3 targetPosition = target.TransformPoint(Offset);
        float delta = (0.1f)/((targetPosition - transform.position).magnitude);
        if((targetPosition - transform.position).magnitude < 3000)
        {
            delta = Time.deltaTime;
            translationSpeed = 1f;
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, translationSpeed * delta);
        //0.0015808 , (b-a)*delta time should be contant for all planets and will be
    }
    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        //var rotation = Quaternion.LookRotation(direction, Vector3.up);
        //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        if(GameManager.instance.activePlanetIndex != 0)
        {
            transform.LookAt(GameManager.instance.planets[GameManager.instance.activePlanetIndex].transform.Find("camPoint").transform);
        }
        else
        {
            transform.LookAt(target.transform);
        }
        
    }
}


