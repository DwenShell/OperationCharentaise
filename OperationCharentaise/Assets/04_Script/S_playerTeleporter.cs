using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_playerTeleporter : MonoBehaviour
{
    public Transform teleportDestination; // Destination de la téléportation
    public float teleportDuration = 1f;   // Durée de la téléportation

    private bool isTeleporting = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    IEnumerator TeleportPlayer(Transform player)
    {
        isTeleporting = true;
        Vector3 startPosition = player.position;
        Vector3 endPosition = teleportDestination.position;
        float elapsedTime = 0f;
        player.GetComponent<Rigidbody>().isKinematic = true;
        // Désactiver tous les enfants du joueur
        SetPlayerChildrenActive(player, false);

        // Lerp de la position du joueur vers la destination
        while (elapsedTime < teleportDuration)
        {
            player.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / teleportDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;

        SetPlayerChildrenActive(player, true);
        player.GetComponent<Rigidbody>().isKinematic = false;
        isTeleporting = false;
    }

    void SetPlayerChildrenActive(Transform player, bool isActive)
    {
        foreach (Transform child in player)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
