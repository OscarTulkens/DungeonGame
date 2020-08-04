using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDraggedItemHandlerScript : MonoBehaviour
{
    private InventorySlotScript _linkedSlot;
    private Image _draggedItemImage;
    private BoxCollider2D _boxCollider2D;
    [SerializeField] private float _scaleIncreaseMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        _draggedItemImage = GetComponent<Image>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        SetDraggedItemImage();
        Bounce();
    }

    // Update is called once per frame
    void Update()
    {
        DragHandler();
    }

    public void SetSlotNumber(InventorySlotScript linkedSlot)
    {
        _linkedSlot = linkedSlot;
    }

    private void SetDraggedItemImage()
    {
        _draggedItemImage.sprite = InventoryManager.Instance.PlayerInventoryObject.TreasureList[_linkedSlot.SlotNumber].InventoryImage;
    }

    private void DragHandler()
    {
        if (Input.GetMouseButton(0))
        {
            transform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDragItem();
        }
    }

    private void StopDragItem()
    {
        if (InventoryManager.Instance.InventoryEquipPanel.bounds.Contains(_boxCollider2D.bounds.center))
        {
            Debug.Log("Equip");
            EquipmentManager.Instance.EquipItem(InventoryManager.Instance.PlayerInventoryObject.TreasureList[_linkedSlot.SlotNumber]);
        }
        Destroy(this.gameObject);
        _linkedSlot.StopDragHandler();
    }

    private void Bounce()
    {
        Vector3 _startSize = transform.localScale;
        LeanTween.scale(gameObject, _startSize * _scaleIncreaseMultiplier, 0.5f).setEasePunch();
    }
}
