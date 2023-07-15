using Pautik;

public class HighDemandItemsNotificationMessage 
{
    public static string HighDemandItemsAlertTitle(int day)
    {
        return GlobalFunctions.WhiteColorText($"High Demand Items Alert ({day} " + (day <= 1 ? "Day" : "Days") + ")");
    }

    public static string HighDemandItemsAlertMessage(int day)
    {
        return GlobalFunctions.PartiallyTransparentText("Attention valued customers,\n\n") +
               GlobalFunctions.PartiallyTransparentText("Exciting news! We have identified a high demand for various items in our inventory over the next ") +
               GlobalFunctions.WhiteColorText($"{day} " + (day > 1 ? "days." : "day.") + "\n\n") +
               GlobalFunctions.PartiallyTransparentText("Maximize your sales by listing these in-demand items now. Customers are actively seeking them, presenting an excellent opportunity for increased revenue.\n\n") +
               GlobalFunctions.PartiallyTransparentText("Need assistance or have questions? Our support team is here to help. Contact them for guidance.\n\n") +
               GlobalFunctions.PartiallyTransparentText("Thank you for being part of our marketplace. We appreciate your participation and wish you successful sales during this high demand period.\n\n") +
               GlobalFunctions.PartiallyTransparentText("Best regards,\nMarketplace X");
    }
}