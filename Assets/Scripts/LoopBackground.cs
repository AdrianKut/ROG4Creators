using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    
    [SerializeField]
    public float xBound = -47.32f;

    [SerializeField]
    private static float speed = 7f;

    [SerializeField]
    private  Vector3 startPos;

    public static void SetSpeed(float _speed) => speed = _speed;
    public static float GetSpeed() => speed;

    private void Start()
    {
        startPos = transform.position;
        speed = 7f;
    }

    private void LateUpdate()
    {
        if (GameManager.gameManagerInstance.isGameOver == false)
        {
            Vector3 newPos = new Vector3(transform.position.x - speed * Time.deltaTime, 4, 0);
            transform.position = newPos;

            if (transform.position.x <= xBound)
                transform.position = startPos;
        }
    }
}
