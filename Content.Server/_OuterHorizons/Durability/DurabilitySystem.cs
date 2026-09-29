using Content.Shared._OuterHorizons.Durability;
using Content.Shared.Examine;
using Robust.Shared.Utility;

namespace Content.Server._OuterHorizons.Durability;

public sealed class DurabilitySystem : SharedDurabilitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DurabilityComponent, ExaminedEvent>(OnExamine);
    }

    private void OnExamine(Entity<DurabilityComponent> ent, ref ExaminedEvent args)
    {
        if (ent.Comp.UseEnergy)
            return;

        if (ent.Comp.MaxDurability == 0)
            return;
        var damagePercent = (int)Math.Round(((float)ent.Comp.Damage / (float)ent.Comp.MaxDurability * 4f));

        var state = Loc.GetString("durability-component-damage-" + damagePercent);

        var message = new FormattedMessage();
        message.AddMarkupPermissive(Loc.GetString("durability-component-damage-message", ("state", state)));

        args.PushMessage(message);
    }

    protected override void Break(EntityUid uid)
    {
        base.Break(uid);

        QueueDel(uid);
    }
}
