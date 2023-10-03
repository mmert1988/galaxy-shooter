using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    [SerializeField]
    private float _rotationSpeed = 50.0f;

    [SerializeField]
    private GameObject _explosion;

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        //transform.Translate(_speed * Time.deltaTime * Vector3.down);
        transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.forward);

        float y = transform.position.y;
        if (y <= -5.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Instantiate(_explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject, 0.8f);
    }
}
