using UnityEngine;

public class WorkerBehaviourScript : MonoBehaviour
{
    public Motion position;
    public Vector3 origin;
    public Vector3 target;

    void Start()
    {
        position = gameObject.AddComponent<Motion>();
        position.MoveTo(transform.position);

        origin = transform.position;
        target = transform.position;
    }

    void Update()
    {
        transform.position = position;
        if (target != origin)
        {
            position.MoveTo(target, 10f);
            origin = target;
        }
    }
}
