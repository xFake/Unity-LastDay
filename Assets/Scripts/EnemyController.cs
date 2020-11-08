using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public delegate void EnemyDiedHandler(EnemyController enemy);

public class EnemyController : MonoBehaviour
{

    #region Fields

    public event EnemyDiedHandler EnemyDied;

    Animator animator;
    NavMeshAgent navAgent;
    PlayerController player;
    AudioSource audioSource;

    [SerializeField]
    float speed;

    [SerializeField]
    int damage = 10; // change it to different script?

    [Header("Audio")]

    [SerializeField]
    AudioClip dyingSound;

    [SerializeField]
    AudioClip moansSound;


    #endregion

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (navAgent)
        {
            navAgent.speed = speed;
        }
    }

    private void Update()
    {
        if (player)
        {
            FollowPlayer();
        }
        else
        {
            animator.SetBool("PlayerAlive", false);
        }

    }

    public void SetPlayer(PlayerController _player)
    {
        player = _player;
    }

    void FollowPlayer()
    {
        animator.SetBool("PlayerAlive", true);
        navAgent.SetDestination(player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            other.GetComponent<ProjectileController>().Destroy();
            navAgent.isStopped = true;
            StartCoroutine(FallsAnimation());
        }

        if (other.tag == "Player")
        {
            animator.SetBool("PlayerInRange", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("PlayerInRange", false);
        }
    }

    private void Attack()
    {
        player.TakeDamage(damage);
    }

    private IEnumerator FallsAnimation()
    {
        animator.SetBool("Dies", true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyDied?.Invoke(this);
    }

    #region Audio

    private void PlayDeadSound()
    {
        audioSource.clip = dyingSound;
        audioSource.Play();
    }

    private void PlayMoansSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = moansSound;
            audioSource.Play();
        }
    }

    #endregion
}
