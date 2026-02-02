using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private InteractionUI _interactionUI;
    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _colliderCount;

    private I_Interactable _interactable;

    private void Update()
    {
        _colliderCount = Physics.OverlapSphereNonAlloc(_point.position, _radius, _colliders, _layerMask);
        if (_colliderCount > 0)
        {
            _interactable = _colliders[0].GetComponent<I_Interactable>();
            if (_interactable != null)
            {
                if (!_interactionUI.IsActive)
                {
                    _interactionUI.SetUp(_interactable.InteractText);
                }
                if (Keyboard.current.fKey.wasPressedThisFrame)
                {
                    _interactable.Interact(this);
                    _interactionUI.Disable();
                }
            }
        }
        else
        {
            // set null
            if (_interactable != null) _interactable = null;

            // disable UI
            if (_interactionUI.IsActive) _interactionUI.Disable();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_point.position, _radius);
    }
}
