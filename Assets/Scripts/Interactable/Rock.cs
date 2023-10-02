using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Interactable
{
    private void Start()
    {
        interactable = gameObject.GetComponent<Interactable>();

        // below code for being able to hit this object
        Player.Instance.OnCutEvent += Instance_OncutEvent;
        hittable = true;
    }
}
