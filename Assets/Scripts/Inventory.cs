using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InteractableSO> storableInteractabls = new List<InteractableSO>();

    [SerializeField] GameInput gameInput;

    [SerializeField] private GameObject CircleMenu;

    [SerializeField] private CircularMenu circularMenu;

    private Dictionary<InteractableSO, int> inventory = new Dictionary<InteractableSO, int>();

    public event EventHandler<OnResourceChangeEventArgs> OnEquipResourceEvent;

    public class OnResourceChangeEventArgs : EventArgs
    {
        public int availableResource;
        public InteractableSO equippedInteractableSO;
    }

    private void Start()
    {
        Player.Instance.OnStoreInventoryEvent += Instance_OnStoreInventoryEvent;
        gameInput.OnInventoryAction += Inventory_Opened;
        gameInput.OnInventoryClosedAction += Inventory_Closed;
        CircleMenu.SetActive(false);

        foreach (InteractableSO interatactableSO in storableInteractabls)
        {
            inventory.Add(interatactableSO, 0);
        }

        circularMenu.OnChangeSelectionEvent += circularMenu_OnChangeSelectionEvent;
    }

    private void circularMenu_OnChangeSelectionEvent(object sender, CircularMenu.OnChangeSelectionEventAArgs e)
    {
        if (e.selectedInteractableSO != null)
        {
            OnEquipResourceEvent?.Invoke(this, new OnResourceChangeEventArgs
            {
                availableResource = inventory[e.selectedInteractableSO],
                equippedInteractableSO = e.selectedInteractableSO
            }) ;
        }
        else
        {
            OnEquipResourceEvent?.Invoke(this, new OnResourceChangeEventArgs
            {
                availableResource = 0,
                equippedInteractableSO = e.selectedInteractableSO
            });
        }
    }

    private void Inventory_Closed(object sender, EventArgs e)
    {
        CircleMenu.SetActive(false);
    }

    private void Inventory_Opened(object sender, EventArgs e)
    {
        CircleMenu.SetActive(true);

    }

    private void Instance_OnStoreInventoryEvent(object sender, Player.OnStoreInventoryEventArgs e)
    {
        if (inventory.Keys.Contains(e.selectedInteractableSO)){
            inventory[e.selectedInteractableSO] += 1;
        }
        else
        {
            Debug.Log("No such interactableSO in the list");
        }
    }

    public void expandResource(InteractableSO interactableSO)
    {
        inventory[interactableSO] -= 1;
    }

}