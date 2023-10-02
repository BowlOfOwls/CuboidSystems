using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected bool hittable;
    protected int health = 3;
    [SerializeField] protected InteractableSO interactableSO;
    protected Interactable interactable;

    protected void Instance_OncutEvent(object sender, Player.OnCutActionEventArgs e)
    {
        if (e.selectedInteractable == interactable && hittable)
        {
            Debug.Log(health);
            health--;
        }
    }

    public bool getHittable()
    {
        return hittable;
    }

    public int getHealth()
    {
        return health;
    }

    public InteractableSO getInteractableSO()
    {
        return interactableSO;
    }
}
