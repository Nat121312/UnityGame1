using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask MonsterLayer;
    public LayerMask InteractableLayer;
    public event Action OnEncountered;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void HandleUpdate()
    {
        if (isMoving == false) 
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero) 
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos)) {
                    StartCoroutine(Move(targetPos));
                }
                
            };
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.F)) {
            Interact();
        }
    }

    void Interact() {
        var FacingDirection = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var InteractPosition = transform.position + FacingDirection;

        var collider = Physics2D.OverlapCircle(InteractPosition, 0.2f, InteractableLayer);
        if (collider != null) {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos) 
    {
        movementSpeed = Time.deltaTime * (18 / 10);
        isMoving = true;
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            if (Input.GetKey(KeyCode.LeftControl)) {
                movementSpeed = Time.deltaTime * (30 / 10);
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed);
            
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        
        CheckForMonsters();
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | InteractableLayer) != null) {
            return false;
        }
        return true;
    }

    private void CheckForMonsters() {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, MonsterLayer) != null) {
        
            if (Random.Range(1, 101) <= 10) {
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }

    }
}
