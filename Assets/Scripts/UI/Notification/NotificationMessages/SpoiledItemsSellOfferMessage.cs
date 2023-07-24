using Pautik;

public class SpoiledItemsSellOfferMessage 
{
    public static string Title { get; private set; } = "Limited Time Offer: Sell Spoiled Items at Half Price!";
    public static string Message { get; private set; } = GlobalFunctions.PartiallyTransparentText("Dear Seller,\n\nGreat news! For a ") + GlobalFunctions.WhiteColorText("limited time only, ") +
                                                         GlobalFunctions.PartiallyTransparentText("we are offering a ") + GlobalFunctions.WhiteColorText("special deal: ") +
                                                         GlobalFunctions.PartiallyTransparentText("we will buy your spoiled items at half of their regular price.\n\nDon't miss out on this opportunity to turn unsold or damaged items into cash. Take advantage of the offer before the time runs out!\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("Act quickly and reach out to us now to benefit from this limited-time offer! Our team is waiting to assist you with all the details.\n\nThank you.\n\nBest regards,");
}