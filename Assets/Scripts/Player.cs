using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = 0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private bool _isTripleShotEnabled = false;
    [SerializeField]
    private bool _isShieldEnabled = false;
    [SerializeField]
    private GameObject _shieldVisualiser;
    [SerializeField]
    private GameObject _thrusterVisualiser;
    [SerializeField]
    private GameObject _rightEngineDamageVisualiser;
    [SerializeField]
    private GameObject _leftEngineDamageVisualiser;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioManager _audioManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is NULL");
        }

        _uiManager.UpdateLives(_lives);
        _uiManager.UpdateScore(_score);
    }

    void Update()
    {
        CalculateMovement();
        HandleFire();
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

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

        _thrusterVisualiser.SetActive(horizontalInput != 0 || verticalInput != 0);
    }

    private void HandleFire()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            HandleSingleFire(0, 0.65f);
            if (_isTripleShotEnabled)
            {
                HandleSingleFire(0.55f, -0.3f);
                HandleSingleFire(-0.55f, -0.3f);
            }
        }
    }

    private void HandleSingleFire(float xOffset, float yOffset)
    {
        Vector3 offset = new Vector3(xOffset, yOffset, 0);
        GameObject laser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Damage();
            other.GetComponent<Enemy>().Destroy();
        }
        if (other.tag == "Asteroid")
        {
            Damage();
        }
    }

    private void Damage()
    {
        if (!_isShieldEnabled)
        {
            Instantiate(_explosionPrefab, gameObject.transform.position, Quaternion.identity);
            _lives--;
            _isTripleShotEnabled = false;
            _speed = 3.5f;
            _uiManager.UpdateLives(_lives);
        }
        _isShieldEnabled = false;
        _shieldVisualiser.transform.gameObject.SetActive(false);
        _rightEngineDamageVisualiser.transform.gameObject.SetActive(_lives <= 2);
        _leftEngineDamageVisualiser.transform.gameObject.SetActive(_lives <= 1);
        if (_lives == 0)
        {
            PlayerDieRoutine();
        }
    }

    private void PlayerDieRoutine()
    {
        _spawnManager.OnPlayerDead();
        Destroy(gameObject, 0.8f);
    }

    public void PowerUp(string powerUpTag)
    {
        switch (powerUpTag)
        {
            case "TripleShotPowerUp":
                _isTripleShotEnabled = true;
                StartCoroutine(PowerDown(powerUpTag));
                break;
            case "SpeedPowerUp":
                _speed = 7.0f;
                StartCoroutine(PowerDown(powerUpTag));
                break;
            case "ShieldPowerUp":
                _isShieldEnabled = true;
                _shieldVisualiser.transform.gameObject.SetActive(true);
                break;
        }
        _audioManager.PlayPowerUpEffect();
    }

    IEnumerator PowerDown(string powerUpTag)
    {
        yield return new WaitForSeconds(5.0f);
        switch (powerUpTag)
        {
            case "TripleShotPowerUp":
                _isTripleShotEnabled = false;
                break;
            case "SpeedPowerUp":
                _speed = 3.5f;
                break;
            case "ShieldPowerUp":
                _isShieldEnabled = false;
                _shieldVisualiser.transform.gameObject.SetActive(false);
                break;
        }
    }

    public void OnKillEnemy()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}
