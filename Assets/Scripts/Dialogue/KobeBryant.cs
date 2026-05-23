using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Individual NPC script for detecing player proximity and then 
/// HANDING over its SO to the dialogue Handler.
/// </summary>
public class KobeBryant : MonoBehaviour
{
    [SerializeField] DialogueSO dialogue;
    public DialogueHandler dialogueHandler;
    CircleCollider2D circleCollider;
    [SerializeField] LayerMask playerLayer;
    public bool isInPlayerProximity;

    private void Awake()
    {
        dialogueHandler = FindFirstObjectByType<DialogueHandler>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (InputManager.Instance.eKeyPressed && isInPlayerProximity)
        {
            if (!dialogueHandler.isDialogueActive)
            {
                dialogueHandler.StartDialogue(dialogue);
            }
            else if (dialogueHandler.isDialogueActive && isInPlayerProximity)
            {
                dialogueHandler.AdvanceDialogue();
            }
        }

        if (dialogueHandler.isDialogueActive && !isInPlayerProximity)
        {//IF PLAYER WALKS AWAY FROM NPC
            dialogueHandler.EndDialogue();
            dialogueHandler.isDialogueActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInPlayerProximity = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInPlayerProximity = false;
        }
    }
}
