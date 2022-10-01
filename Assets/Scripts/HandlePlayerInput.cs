using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;

public class HandlePlayerInput : MonoBehaviour
{
    public Camera currentCamera;
    public LayerMask layerMask;

    private Collider2D[] selectedObjects = new Collider2D[0];
    private int selectedIndex = 0;
    private bool firstSelection = false;
    private Vector3 startPosition;

    public float threshold = 0.5f;

    public UnityEvent<GameObject> OnHandleMouseClick;
    public UnityEvent<Vector3> OnHandleMouseFinishDragging;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            HandleMouseClick();
        }
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseClick()
    {
        startPosition = GetMousePosition();
        firstSelection = selectedObjects.Length == 0;
        if (firstSelection)
        {
            PerformSelection();
        }
    }

    private void PerformSelection()
    {
        Collider2D collider = HandleMultipleObjectSelection(startPosition);
        GameObject selectedGameObject = collider == null ? null : collider.gameObject;
     
        OnHandleMouseClick?.Invoke(selectedGameObject);
        if (selectedGameObject != null)
        {
            Debug.Log($"the selected object is {selectedGameObject.name}");
        }
    }

    private Collider2D HandleMultipleObjectSelection(Vector3 clickPosition)
    {
        Collider2D[] tempSelectedObjects = Physics2D.OverlapPointAll(clickPosition, layerMask);
        Collider2D selectedCollider = null;
        if (tempSelectedObjects.Length == 0)
        {
            selectedObjects = new Collider2D[0];
        }
        else
        {
            if (CheckTheSameSelection(tempSelectedObjects))
            {
                selectedIndex++;
                selectedIndex = selectedIndex >= selectedObjects.Length ? 0 : selectedIndex;
            }
            else
            {
                selectedObjects = tempSelectedObjects;
                selectedIndex = 0;
            }

            return selectedObjects[selectedIndex];
        }
         return selectedCollider;
    }

    private bool CheckTheSameSelection(Collider2D[] tempSelectedObjects)
    {
        if (selectedObjects == null || tempSelectedObjects.Length ==0)
        {
            return false;
        }
        return (tempSelectedObjects.Length == selectedObjects.Length) &&
            (tempSelectedObjects.Intersect(selectedObjects).Count()== selectedObjects.Length);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mouseInput = currentCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseInput.z = 0;
        return mouseInput;
    }

    private void HandleMouseUp()
    {
        Vector3 endposition = GetMousePosition();
        if (Vector3.Distance(startPosition,endposition)> threshold)
        {
            OnHandleMouseFinishDragging?.Invoke(endposition);
            return;
        }
       if (firstSelection== false)
        {
            PerformSelection();
        }
        return;
    }
}
