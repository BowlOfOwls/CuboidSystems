using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedResource : MonoBehaviour
{
    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private Interactable interactable;

    private void Start()
    {
        Player.Instance.OnSelectedInteractableChanged += Instance_OnSelectedCounterChanged;
        selectedVisual.SetActive(false);

    }

    private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedInteractableChangeEventArgs e)
    {
        if (interactable == null)
        {
            return;
        }
        if (e.selectedInteractable == interactable)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }

    }
}
