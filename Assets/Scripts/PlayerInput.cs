using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private string _horizontalAxisName;
    [SerializeField]
    private string _verticalAxisName;
    [SerializeField]
    private KeyCode _fireKeyCode;
    private Player _player;
    private float _nextFire = 0f;

    private void Start()
    {
        _player = gameObject.GetComponent<Player>();
    }

    void Update()
    {
        CalculateMovement();
        HandleFire();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis(_horizontalAxisName);
        float verticalInput = Input.GetAxis(_verticalAxisName);
        float speed = _player.GetSpeed();
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        float y = transform.position.y;
        float boundaryTop = 5.8f;
        float boundaryBottom = -3.8f;
        if (y <= boundaryBottom)
        {
            transform.position = new Vector3(transform.position.x, boundaryBottom, transform.position.z);
        }
        else if (y >= boundaryTop)
        {
            transform.position = new Vector3(transform.position.x, boundaryTop, transform.position.z);
        }

        float x = transform.position.x;
        float horizontalBound = 11.5f;
        if (x <= -horizontalBound)
        {
            transform.position = new Vector3(horizontalBound, transform.position.y, transform.position.z);
        }
        else if (x >= horizontalBound)
        {
            transform.position = new Vector3(-horizontalBound, transform.position.y, transform.position.z);
        }

        _player.SetThrusterVisualiserActive(horizontalInput != 0 || verticalInput != 0);
    }

    private void HandleFire()
    {
        if (Input.GetKey(_fireKeyCode) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _player.GetFireRate();
            _player.Fire(0, 0.65f);
            if (_player.IsTrippleShotEnabled())
            {
                _player.Fire(0.55f, -0.3f);
                _player.Fire(-0.55f, -0.3f);
            }
        }
    }
}
