using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Animator _animator;
    private bool _isAlive = true;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
        StartCoroutine(FireCoroutine());
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        float y = transform.position.y;
        if (y <= -5.0f)
        {
            float x = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(x, 8, transform.position.z);
        }
    }

    private IEnumerator FireCoroutine()
    {
        while (_isAlive)
        {
            float nextFire = Random.Range(3.0f, 7.0f);
            Fire();
            yield return new WaitForSeconds(nextFire);
        }
    }

    private void Fire()
    {
        Vector3 offset = new Vector3(0, -0.87f, 0);
        GameObject laser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        laser.AddComponent<EnemyFire>();
    }

    public void Destroy()
    {
        _isAlive = false;
        _animator.SetTrigger("Explode");
        Destroy(GetComponent<Collider2D>());
        Destroy(gameObject, _animator.runtimeAnimatorController.animationClips[0].length);
    }
}
