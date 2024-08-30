using UnityEngine;

public class Armor : Equipment
{
    [SerializeField] private ArmorInfo _armorInfo;
    [SerializeField] private GameObject _destroyVFX;
    public ArmorInfo ArmorInfo { get { return _armorInfo; } }
    public override EquipmentType Type { get { return EquipmentType.Armor; } }

    public void DestroyArmor(bool isGame) {
        Destroy(this.gameObject);
        if (isGame) {
            Instantiate(_destroyVFX, this.transform.position, Quaternion.identity);
        }
    }
}
