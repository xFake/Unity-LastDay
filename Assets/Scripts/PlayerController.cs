using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Fields
    [SerializeField]
    float speed;
    [SerializeField]
    float speedWhenFollowMouse;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    int hitPoints = 100; // Change it to different script?

    GunController gun;
    Animator animator;
    public AudioSource audioSource;

    float horizontal;
    float vertical;

    [SerializeField]
    ParticleSystem bleed1;
    [SerializeField]
    ParticleSystem bleed2;
    [SerializeField]
    ParticleSystem bleed3;

    [Header("Audio")]
    [SerializeField]
    List<AudioClip> stepClips;
    [SerializeField]
    AudioClip gunShootSound;
    [SerializeField]
    AudioClip reloadSound;

    [SerializeField]
    float timeBetweenShoots;

    bool isMoving = false;
    bool isShooting = false;
    bool isReloading = false;

    [SerializeField]
    int maxAmmo = 7;
    int gunAmmo;

    bool followMouse;
    #endregion

    private void Start()
    {
        gun = GetComponentInChildren<GunController>();
        animator = GetComponent<Animator>();
        bleed1.Stop();
        bleed2.Stop();
        bleed3.Stop();
        gunAmmo = maxAmmo;
    }

    void Update()
    {
        if (Time.timeScale > 0 && GameController.GetGameState() == true)
        {
            Move();
            LookAtMousePosition();
            if (Input.GetMouseButtonDown(0) && isMoving == false && isShooting == false && isReloading == false)
            {
                StartCoroutine("Shoot");
            }
            if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
            {
                StartCoroutine("Reload");
            }
        }
        if (hitPoints <= 0)
        {
            StartCoroutine("Die");
            
            // do something here when it happens
        }
        if (hitPoints == 80)
        {
            bleed1.Play();
        }
        if (hitPoints == 50)
        {
            bleed2.Play();
        }
        if (hitPoints == 20)
        {
            bleed3.Play();
        }
    }

    private IEnumerator Die()
    {
        animator.SetBool("isDead", true);
        GameController.SetGameState(false);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    private IEnumerator Reload()
    {
        isReloading = true;
        PlayRealoadSound();
        yield return new WaitForSeconds(reloadSound.length);
        gunAmmo = maxAmmo;
        isReloading = false;
    }

    private IEnumerator Shoot()
    {
        if (gunAmmo <= 0)
        {
            StartCoroutine("Reload");
        }
        else
        {
            gun.Shoot();
            gunAmmo--;

            animator.SetBool("isShooting", true);
            isShooting = true;
            yield return new WaitForSeconds(timeBetweenShoots);
            animator.SetBool("isShooting", false);
            isShooting = false;
        }
    }

    void LookAtMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,9))// if its Int then it ignores this mask, if its LayerMask it hits it.
        {
            Vector3 lookPosition = hit.point;
            lookPosition.y = transform.position.y;
            transform.LookAt(lookPosition);
        }
    }

    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
         
        if (followMouse)
        {
            Vector3 movement = transform.TransformDirection(direction) * speed;
            characterController.SimpleMove(movement * speedWhenFollowMouse);
        }
        else
        {
            characterController.SimpleMove(direction * speed);
        }

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        if (direction != Vector3.zero)
        {
            PlayStepsSound();
            isMoving = true;
            animator.SetBool("isMoving", isMoving);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
        }
    }

    public void SetMovementType()
    {
        followMouse = !followMouse;
    }

    public void TakeDamage(int value)
    {
        hitPoints -= value;
    }

    #region Audio

    void PlayStepsSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = stepClips[Random.Range(0, stepClips.Count)];
            audioSource.Play();
        }
    }

    void PlayShotSound()
    {
        audioSource.clip = gunShootSound;
        audioSource.Play();
    }

    void PlayRealoadSound()
    {
        audioSource.clip = reloadSound;
        audioSource.Play();
    }

    #endregion
}
