using UnityEngine;

public class LoopBackground : MonoBehaviour
{

    public float speed = 1f;
    public float xBound = -47.32f;

    public static Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Vector3 newPos = new Vector3(transform.position.x - speed * Time.deltaTime, 4, 0);
        transform.position = newPos;

        if (transform.position.x <= xBound)
        {
            transform.position = startPos;
        }
    }
}
