using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
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

    public void Destroy()
    {
        _animator.SetTrigger("Explode");
        Destroy(gameObject, _animator.runtimeAnimatorController.animationClips[0].length);
    }
}
