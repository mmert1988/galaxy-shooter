using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;

    private Player _player;

    private AudioManager _audioManager;

    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("Couldn't find Player component");
        }

        _audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        CalculateMovement();
        DestroyIfGone();
    }

    private void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
    }

    private void DestroyIfGone()
    {
        if (transform.position.y >= 8.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Destroy();
            _player.OnKillEnemy();
            Destroy(gameObject);
            _audioManager.PlayExplosionEffect();
        }
        if (other.tag == "Asteroid")
        {
            other.GetComponent<Asteroid>().Destroy();
            Destroy(gameObject);
        }
    }
}
