using Pautik;

public class ItemsSpoilageSurgeMessage 
{
    public static string Title { get; private set; } = "Urgent Notice: Inventory Items Spoilage Surge";
    public static string Message { get; private set; } = GlobalFunctions.WhiteColorText("Dear Seller,\n\nDue to unfavorable weather conditions, a surge in items spoilage within your inventory has occurred.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("Please take immediate measures to preserve the quality of your remaining items and consider appropriate adjustments to your offerings during this period.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("We are closely monitoring the situation and ") + GlobalFunctions.WhiteColorText("will keep you informed about any updates.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("We appreciate your understanding and cooperation.\n\nBest regards,");

    public static string ResolvedTitle { get; private set; } = "Resolved: Inventory Items Spoilage Surge";
    public static string ResolvedMessage { get; private set; } = GlobalFunctions.PartiallyTransparentText("Dear Seller,\n\nWe hope this message finds you well.\n\nWe are pleased to inform you that the Inventory Items Spoilage Surge, caused by the unfavorable weather conditions, has been ") +
                                                                 GlobalFunctions.WhiteColorText("successfully resolved. ") + GlobalFunctions.PartiallyTransparentText("The spoilage rates have returned to normal, and your inventory should no longer be affected.\n\n") +
                                                                 GlobalFunctions.PartiallyTransparentText("We appreciate your patience and cooperation during this challenging period. Your diligence in preserving the quality of your remaining items is commendable.\n\n") +
                                                                 GlobalFunctions.PartiallyTransparentText("Thank you for your understanding and support.\n\nBest regards,");
}