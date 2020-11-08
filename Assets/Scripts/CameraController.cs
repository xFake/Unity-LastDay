using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController : MonoBehaviour
{

    #region Fields
    Transform player;
    Vector3 offset;

    [SerializeField]
    LayerMask leafMask;

    RaycastHit[] previousHits;
    #endregion

    private void Awake()
    {
        //offset = _player.position - transform.position;// old version
        offset = new Vector3(0.2f, -22.7f, 13.2f); // i dont like it but at least it knows position no matter where player is spawned
    }

    void Update()
    {
        if (player != null)
        {
            FollowPlayer();
            ClearView();
        }
    }

    public void SetPlayer(Transform _player)
    {
        player = _player;
    }

    void FollowPlayer()
    {
        transform.position = player.position - offset;
    }

    void ClearView()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity, leafMask);
        if (previousHits != null)
        {
            foreach (RaycastHit previousHit in previousHits)
            {
                if (!hits.Contains(previousHit))
                {
                    previousHit.collider.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }

        if (hits != null)
        {
            previousHits = hits;
            foreach (RaycastHit hit in hits)
            {
                hit.collider.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
