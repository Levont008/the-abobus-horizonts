namespace Content.Shared._OuterHorizons.Durability;

[RegisterComponent]
public sealed partial class DurabilityComponent : Component
{
    [DataField]
    public int MaxDurability = 65;

    [DataField]
    public int Damage = 0;

    [DataField]
    public bool UseEnergy = false;

    [DataField]
    public int ChargePerUse = 0;
}
