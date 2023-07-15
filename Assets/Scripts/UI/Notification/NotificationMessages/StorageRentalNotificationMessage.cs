using Pautik;

public class StorageRentalNotificationMessage
{
    public static string StorageRentDiscountTitle(int days)
    {
        return $"Storage Rent Discount Applied for {days} Days";
    }

    public static string StorageRentDiscountMessage(int days)
    {
        return GlobalFunctions.PartiallyTransparentText("We have exciting news for you! We want to inform you that a special discount has been applied to your storage rent for the next ") +
               GlobalFunctions.WhiteColorText($"{days} days.\n\n") +
               GlobalFunctions.PartiallyTransparentText("This means that you can continue to safely store your items while enjoying reduced rental fees. We appreciate your loyalty and hope this discount helps you in your selling endeavors.\n\n") +
               GlobalFunctions.PartiallyTransparentText("If you have any questions or need further assistance, please feel free to contact our customer support team. We are here to help.\n\n") +
               GlobalFunctions.PartiallyTransparentText("Thank you for choosing our storage services. We value your business.\n\n") +
               GlobalFunctions.WhiteColorText("Best regards,\nSafeguard Storage Solutions");
    }
}