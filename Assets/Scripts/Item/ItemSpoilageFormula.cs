
public class ItemSpoilageFormula 
{
    public static int SpoilageLevel(ItemDurabilityLevel itemDurabilityLevel)
    {
        return itemDurabilityLevel == ItemDurabilityLevel.Perishable_Food ? 10 :
               itemDurabilityLevel == ItemDurabilityLevel.LowDurability ? 15 :
               itemDurabilityLevel == ItemDurabilityLevel.ShortShelfLife_Food ? 20 :
               itemDurabilityLevel == ItemDurabilityLevel.MediumDurability ? 25 :
               itemDurabilityLevel == ItemDurabilityLevel.UseByDate_Food ? 30 :
               itemDurabilityLevel == ItemDurabilityLevel.ExtendedShelfLife_Food ? 35 : 50;
    }
}