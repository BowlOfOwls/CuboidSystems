using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Inventory;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    // Start is called before the first frame update
    [SerializeField] float playerSpeed = 8f;
    [SerializeField] GameInput gameInput;

    private bool isWalking;
    private Vector3 faceDirection;

    [SerializeField] MaterialSO[] materialSOList;

    public event EventHandler<OnCutActionEventArgs> OnCutEvent;
    public class OnCutActionEventArgs : EventArgs
    {
        public Interactable selectedInteractable;
    }

    public event EventHandler<OnSelectedInteractableChangeEventArgs> OnSelectedInteractableChanged;
    public class OnSelectedInteractableChangeEventArgs : EventArgs
    {
        public Interactable selectedInteractable;
    }

    public event EventHandler<OnInteractEventArgs> OnInteractEvent;
    public class OnInteractEventArgs : EventArgs
    {
        public Interactable selectedInteractable;
    }

    public event EventHandler<OnStoreInventoryEventArgs> OnStoreInventoryEvent;
    public class OnStoreInventoryEventArgs : EventArgs
    {
        public InteractableSO selectedInteractableSO;
    }

    private InteractableSO equippedInteractable = null;

    [SerializeField] private Inventory inventory;

    [SerializeField] Transform holdPosition;

    public event EventHandler<OnResourceChangeEventArgs> OnEquipResourceEvent;

    public class OnResourceChangeEventArgs : EventArgs
    {
        public int availableResource;
        public InteractableSO equippedInteractableSO;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnCutAction += GameInput_OnCutAction;
        gameInput.OnInteractAction += Interact_performed;

        inventory.OnEquipResourceEvent += Inventory_OnEquipResourceEvent;
    }

    private void Inventory_OnEquipResourceEvent(object sender, Inventory.OnResourceChangeEventArgs e)
    {
        if (e.availableResource > 0)
        {
            equippedInteractable = e.equippedInteractableSO;
        }
    }

    private void Update()
    {
        handleMovement();
        handleInteractions();

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool block(Vector3 moveDir, float maxDistance)
    {
        float playerHeight = 2f;
        float playerRadius = 0.5f;
        return Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, maxDistance);
    }

    public void handleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float rotateSpeed = 10f;

        isWalking = moveDir != Vector3.zero;

        if (!block(moveDir, playerSpeed * Time.deltaTime))
        {
            transform.position += moveDir * playerSpeed * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
        else
        {
            Vector3 attemptLeftRightMove = new Vector3(moveDir.x, 0f, 0f);
            Vector3 attemptUpDownMove = new Vector3(0f, 0f, moveDir.z);

            if (!block(attemptLeftRightMove, playerSpeed * Time.deltaTime))
            {
                transform.position += attemptLeftRightMove.normalized * playerSpeed * Time.deltaTime;
                transform.forward = Vector3.Slerp(transform.forward, attemptLeftRightMove, Time.deltaTime * rotateSpeed);
            }
            if (!block(attemptUpDownMove, playerSpeed * Time.deltaTime))
            {
                transform.position += attemptUpDownMove.normalized * playerSpeed * Time.deltaTime;
                transform.forward = Vector3.Slerp(transform.forward, attemptUpDownMove, Time.deltaTime * rotateSpeed);
            }
        }
    }

    public void handleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            faceDirection = moveDir;
        }

        float interactDistance = 1f;

        float playerHeight = 1f;
        float playerRadius = 0.35f;
        if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, faceDirection, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out Interactable interactable))
            {
                InteractableChange(interactable);
            }
        }
        else
        {
            InteractableChange(null);
        }
    }

    public void InteractableChange(Interactable interactable)
    {
        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangeEventArgs
        {
            selectedInteractable = interactable
        });
    }
    private void GameInput_OnCutAction(object sender, System.EventArgs e)
    {


        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            faceDirection = moveDir;
        }

        float interactDistance = 1f;

        float playerHeight = 1f;
        float playerRadius = 0.35f;
        if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, faceDirection, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out Interactable interactable))
            {
                if (interactable.getHittable())
                {
                    OnCutEvent?.Invoke(this, new OnCutActionEventArgs
                    {
                        selectedInteractable = interactable
                    });


                    if (interactable.getHealth() <= 0)
                    {
                        Debug.Log(interactable.gameObject.transform);
                        for (int i = 0; i < ratioForInPut(interactable.getInteractableSO()); i++)
                        {
                            Instantiate(inputForOutPut(interactable.getInteractableSO()).prefab, new Vector3(interactable.gameObject.transform.position.x, i * 1f, interactable.gameObject.transform.position.z), Quaternion.identity);
                        }
                        Destroy(interactable.gameObject);
                    }
                }
            }
        }
        else
        {
        }
    }

    private InteractableSO inputForOutPut(InteractableSO interactableSO)
    {
        foreach (MaterialSO materialSO in materialSOList)
        {
            if (interactableSO == materialSO.input)
            {
                return materialSO.output;
            }
        }
        return null;
    }

    private int ratioForInPut(InteractableSO interactableSO)
    {
        foreach (MaterialSO materialSO in materialSOList)
        {
            if (interactableSO == materialSO.input)
            {
                return materialSO.spawnRatio;
            }
        }
        return 0;
    }


    private void Interact_performed(object sender, EventArgs e)
    {
        if (equippedInteractable == null)
        {

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();

            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            if (moveDir != Vector3.zero)
            {
                faceDirection = moveDir;
            }

            float interactDistance = 1f;

            float playerHeight = 1f;
            float playerRadius = 0.35f;
            if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, faceDirection, out RaycastHit hit, interactDistance))
            {
                if (hit.transform.TryGetComponent(out PickUp pickUp))
                {
                    OnStoreInventoryEvent?.Invoke(this, new OnStoreInventoryEventArgs
                    {
                        selectedInteractableSO = pickUp.getInteractableSO()
                    });
                    OnInteractEvent?.Invoke(this, new OnInteractEventArgs
                    {
                        selectedInteractable = pickUp
                    });
                }
            }
        }

        else
        {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();

            Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            if (moveDir != Vector3.zero)
            {
                faceDirection = moveDir;
            }

            float interactDistance = 1f;

            float playerHeight = 1f;
            float playerRadius = 0.35f;
            if (Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, faceDirection, out RaycastHit hit, interactDistance))
            {
                if (hit.transform.TryGetComponent(out Counter counter))
                {
                    Debug.Log("Interacting with counter");
                }

            }
            else
            {
                Instantiate(equippedInteractable.prefab, holdPosition.position, Quaternion.identity);
                inventory.expandResource(equippedInteractable);
                equippedInteractable = null;
                OnEquipResourceEvent?.Invoke(this, new OnResourceChangeEventArgs
                {
                    availableResource = 0,
                    equippedInteractableSO = null
                });
            }


        }



    }
}