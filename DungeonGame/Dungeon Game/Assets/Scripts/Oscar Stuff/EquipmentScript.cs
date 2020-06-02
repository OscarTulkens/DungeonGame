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
        SetStartModels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetArmorModel(object sender, EquipmentManager.OnChangeEquipmentArgs e)
    {
        switch (e.itemtype)
        {
            case ItemType.Default:
                break;
            case ItemType.Helmet:
                SetModel(e.itemModel, _shieldModel, _shieldPoint);
                break;
            case ItemType.Armor:
                SetModel(e.itemModel, _chestplateModel, _chestplatePoint);
                break;
            case ItemType.Weapon:
                SetModel(e.itemModel, _weaponModel, _weaponPoint);
                break;
            case ItemType.Offhand:
                SetModel(e.itemModel, _shieldModel, _shieldPoint);
                break;
            default:
                break;
        }
    }

    private void SetModel(GameObject newModel, GameObject oldModel, Transform transform)
    {
        if (oldModel!=null)
        {
            Destroy(oldModel);
        }
        oldModel = Instantiate(newModel, transform);
    }

    private void SetStartModels()
    {
        SetModel(_startChestplateModel, _chestplateModel, _chestplatePoint);
        SetModel(_startHelmetModel, _helmetModel, _helmetPoint);
        SetModel(_startWeaponModel, _weaponModel, _weaponPoint);
        SetModel(_startShieldModel, _shieldModel, _shieldPoint);
    }
}
