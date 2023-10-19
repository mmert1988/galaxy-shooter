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
    [SerializeField]
    private UIManager _uiManager;
    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is NULL");
        }

        _uiManager.UpdateLives(_lives);
        _uiManager.UpdateScore(_score);
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

    public void SetThrusterVisualiserActive(bool active)
    {
        _thrusterVisualiser.SetActive(active);
    }

    public bool IsTrippleShotEnabled()
    {
        return _isTripleShotEnabled;
    }

    public float GetFireRate()
    {
        return _fireRate;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void Damage()
    {
        if (_lives == 0)
        {
            return;
        }
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

    public void Fire(float xOffset, float yOffset)
    {
        Vector3 offset = new Vector3(xOffset, yOffset, 0);
        GameObject laser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        laser.AddComponent<PlayerFire>();
    }
}
