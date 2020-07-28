using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentScript : MonoBehaviour
{
    [Header("START MODELS")]
    [SerializeField] private GameObject _startChestplateModel;
    [SerializeField] private GameObject _startHelmetModel;
    [SerializeField] private GameObject _startWeaponModel;
    [SerializeField] private GameObject _startShieldModel;

    private GameObject _chestplateModel;
    private GameObject _helmetModel;
    private GameObject _weaponModel;
    private GameObject _shieldModel;

    [Space]
    [Header("SPAWNPOINTS")]

    [SerializeField] private Transform _chestplatePoint;
    [SerializeField] private Transform _helmetPoint;
    [SerializeField] private Transform _weaponPoint;
    [SerializeField] private Transform _shieldPoint;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnChangeEquipment += SetArmorModel;
        SetStartModels();
    }

    private void SetArmorModel(object sender, EventManager.OnChangeEquipmentArgs e)
    {
        switch (e.ItemType)
        {
            case ItemType.Default:
                break;
            case ItemType.Helmet:
                Destroy(_helmetModel);
                _helmetModel = SetModel(e.ItemModel, _helmetPoint);
                break;
            case ItemType.Armor:
                Destroy(_chestplateModel);
                _chestplateModel = SetModel(e.ItemModel, _chestplatePoint);
                break;
            case ItemType.Weapon:
                Destroy(_weaponModel);
                _weaponModel = SetModel(e.ItemModel, _weaponPoint);
                break;
            case ItemType.Offhand:
                Destroy(_shieldModel);
                _shieldModel = SetModel(e.ItemModel, _shieldPoint);
                break;
            default:
                break;
        }
    }

    private GameObject SetModel(GameObject newModel, Transform transform)
    {
        return Instantiate(newModel, transform);
    }

    private void SetStartModels()
    {
        _chestplateModel = SetModel(_startChestplateModel, _chestplatePoint);
        _helmetModel = SetModel(_startHelmetModel, _helmetPoint);
        _weaponModel = SetModel(_startWeaponModel, _weaponPoint);
        _shieldModel = SetModel(_startShieldModel, _shieldPoint);
    }
}
