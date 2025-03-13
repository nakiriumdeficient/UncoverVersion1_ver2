using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    public int upgradeAmount; // Amount of XP this pickup gives
    private Collider upgradeCollider;

    // Start is called before the first frame update
    void Start()
    {
        upgradeCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreyPlayer")) // Ensure the player has the "Player" tag
        {
            GameManager.Instance.GainUpgrade(upgradeAmount);
            upgradeCollider.enabled = false;
            StartCoroutine(DestroyAfterSound()); // Wait before destroying
        }
    }
    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

