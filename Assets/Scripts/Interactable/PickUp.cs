using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    private Rigidbody myRigidBody;
    private bool stopped;


    private void Start()
    {
        hittable = false;
        launchIntoAir();
        stopped = false;
        StartCoroutine(letSpin());

        Player.Instance.OnInteractEvent += Instance_OnInteractEvent;

        interactable = gameObject.GetComponent<Interactable>();
    }

    private void Instance_OnInteractEvent(object sender, Player.OnInteractEventArgs e)
    {
        if (e.selectedInteractable == interactable)
        {
            Destroy(gameObject);
        }
    }

    private void launchIntoAir()
    {
        float launchPower = 1f;
        myRigidBody = gameObject.AddComponent<Rigidbody>();
        myRigidBody.velocity += new Vector3(UnityEngine.Random.Range(-launchPower, launchPower), launchPower * 6, UnityEngine.Random.Range(-launchPower, launchPower));

    }

    private void Update()
    {
        StopSpin();
    }

    private IEnumerator letSpin()
    {
        yield return new WaitForSeconds(0.5f);
        stopped = true;
    }

    private void StopSpin()
    {
        if (transform.position.y <= 0.2f && stopped)
        {
            Destroy(myRigidBody);
            transform.rotation = Quaternion.identity;
        }
    }

}
