using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GameObject _UIPanel;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _mainCamera = Camera.main;
        _UIPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        var rotationCamera = _mainCamera.transform.rotation;
        transform.LookAt(transform.position + rotationCamera * Vector3.forward, rotationCamera * Vector3.up);
    }

    public bool IsActive = false;

    public void SetUp(string prompt)
    {
        _text.text = prompt;
        _UIPanel.SetActive(true);
        IsActive = true;
    }

    public void Disable()
    {
        _UIPanel.SetActive(false);
        IsActive = false;
    }


}
