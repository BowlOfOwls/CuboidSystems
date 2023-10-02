using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private string isWalking = "IsWalking";

    [SerializeField] private Player player;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private Inventory inventory;

    private GameObject display;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(isWalking, false);
    }

    private void Start()
    {
        gameInput.OnCutAction += GameInput_OnCutAction;

        inventory.OnEquipResourceEvent += Inventory_OnEquipResourceEvent;
        //gameInput.OnInventoryAction += Instance_ListInventoryAction;

        player.OnEquipResourceEvent += Inventory_OnEquipResourceEvent;
    }

    private void Inventory_OnEquipResourceEvent(object sender, Player.OnResourceChangeEventArgs e)
    {
        if (e.availableResource == 0)
        {
            if (display != null)
            {
                Destroy(display);
            };
        }
        else
        {
            if (display != null)
            {
                Destroy(display);
                display = Instantiate(e.equippedInteractableSO.visual, holdPoint, transform);
                display.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                display = Instantiate(e.equippedInteractableSO.visual, holdPoint, transform);
                display.transform.localPosition = new Vector3(0f, 0f, 0f);
            }


        }
    }

    private void Inventory_OnEquipResourceEvent(object sender, Inventory.OnResourceChangeEventArgs e)
    {
        if (e.availableResource == 0)
        {
            if (display != null)
            {
                Destroy(display);
            };
        }
        else
        {
            if (display != null)
            {
                Destroy(display);
                display = Instantiate(e.equippedInteractableSO.visual, holdPoint, transform);
                display.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                display = Instantiate(e.equippedInteractableSO.visual, holdPoint, transform);
                display.transform.localPosition = new Vector3(0f, 0f, 0f);
            }


        }
    }


    /*
private void Instance_ListInventoryAction(object sender, EventArgs e)
{
   InteractableSO visual = inventory.GetInventory()[0];
   Debug.Log(visual);
   if (display == null)
   {
       display = Instantiate(visual.visual, holdPoint, transform);
       display.transform.localPosition = new Vector3(0f, 0f, 0f);

   }
   else
   {
       Destroy(display);
   }
}
*/
    private void GameInput_OnCutAction(object sender, System.EventArgs e)
    {
        animator.SetTrigger("Cut");
    }
}
