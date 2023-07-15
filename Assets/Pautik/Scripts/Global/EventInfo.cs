
public static class EventInfo
{
    public static byte Code_SetPlayerIndex { get; private set; } = 0;
    public static object[] Content_SetPlayerIndex { get; set; }

    public static byte Code_DemandDrivenItemsId { get; private set; } = 1;
    public static object Content_DemandDrivenItemsId { get; set; }

    public static byte Code_InventoryPublisher { get; private set; } = 2;
    public static object Content_InventoryPublisher { get; set; }
}
