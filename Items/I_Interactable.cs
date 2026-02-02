using UnityEngine;

public interface I_Interactable
{
    public string InteractText { get; }
    public bool Interact(Interactor interactor);
}
