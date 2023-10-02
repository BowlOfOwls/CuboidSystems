using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Interactable
{
    // Start is called before the first frame update
    private void Start()
    {
        interactable = gameObject.GetComponent<Interactable>();

        // below code for being able to hit this object
        Player.Instance.OnCutEvent += Instance_OncutEvent;
        hittable = true;
        //
    }
}
