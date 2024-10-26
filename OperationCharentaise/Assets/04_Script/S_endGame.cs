using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class S_endGame : MonoBehaviour
{
    public GameObject fire;
    public List<GameObject> navMeshObjects;      // Liste d'objets avec NavMeshAgent
    public GameObject endGameImage;              // Image � activer dans l'UI
    public float delayBeforeImage = 1f;          // D�lai avant l'affichage de l'image
    public float delayBeforeReturn = 3f;         // D�lai avant de retourner au menu

    private void Start()
    {
        // D�sactive les particules au d�marrage
        ParticleSystem[] particleSystems = fire.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<S_charaController>().getOrGiveObject(2, false);

            // Active les particules au contact
            ParticleSystem[] particleSystems = fire.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }

            TriggerEndGame();
        }
    }

    public void TriggerEndGame()
    {
        // D�sactive les NavMeshAgent de chaque objet dans la liste
        foreach (GameObject obj in navMeshObjects)
        {
            NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
                agent.gameObject.GetComponent<S_playerChaser>().enabled = false;
            }
        }

        // Lance la coroutine pour afficher l'image de victoire apr�s un d�lai
        StartCoroutine(DisplayEndGameImage());
    }

    private IEnumerator DisplayEndGameImage()
    {
        yield return new WaitForSeconds(delayBeforeImage);

        // Active l'image de fin de partie dans l'UI
        if (endGameImage != null)
        {
            endGameImage.gameObject.SetActive(true);
        }

        // Lance la coroutine pour retourner au menu apr�s un d�lai
        StartCoroutine(ReturnToMenuAfterDelay());
    }

    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeReturn);

        // Charge la sc�ne de menu, supposons que son nom est "Menu"
        SceneManager.LoadScene("Menu");
    }
}
