using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockVisual : MonoBehaviour
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private GameObject selected;
    [SerializeField] private ParticleSystem debrisParticleSystem;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Player.Instance.OnCutEvent += Instance_OncutEvent;
    }

    private void Instance_OncutEvent(object sender, Player.OnCutActionEventArgs e)
    {
        if (e.selectedInteractable == interactable && selected.activeSelf)
        {
            animator.SetTrigger("IsCut");

            debrisParticleSystem.Play();
            StartCoroutine(waitForAnimation());
        }
    }

    private IEnumerator waitForAnimation()
    {
        selected.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        selected.SetActive(true);

    }

}
