using Pautik;

public class ItemRecallAlertMessage 
{
    public static string Title { get; private set; } = "Urgent Item Recall Alert for Sellers";
    public static string Message { get; private set; } = GlobalFunctions.PartiallyTransparentText("Dear Seller,\n\nA recent item safety issue has led to a recall of certain items from your inventory. To ensure consumer safety and compliance, please take immediate action:\n\n") +
                                                         GlobalFunctions.WhiteColorText("1. Discard the Affected Items: ") + GlobalFunctions.PartiallyTransparentText("Immediately remove the recalled items from your inventory and dispose of them properly to prevent any potential harm to consumers.\n") +
                                                         GlobalFunctions.WhiteColorText("2. Return for a Refund: ") + GlobalFunctions.PartiallyTransparentText("If applicable, please contact our support team to arrange for the return of the recalled items for a refund.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("We understand that this situation is concerning, and we appreciate your cooperation in prioritizing the safety and well-being of our customers.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("Thank you for your prompt attention.\n\nBest regards,");

    public static string PopupMessage { get; private set; } = GlobalFunctions.WhiteColorText("Urgent Item Recall Alert! ") + GlobalFunctions.PartiallyTransparentText("Visit your messages for further details.");

    public static string ResolvedPopupMessage { get; private set; } = GlobalFunctions.WhiteColorText("Issue Resolved! ") + GlobalFunctions.PartiallyTransparentText("We are pleased to inform you that the ") +
                                                                      GlobalFunctions.WhiteColorText("item recall issue has been successfully resolved.");
}