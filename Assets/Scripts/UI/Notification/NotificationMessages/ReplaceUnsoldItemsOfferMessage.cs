using Pautik;

public struct ReplaceUnsoldItemsOfferMessage 
{
    public static string Title { get; private set; } = "Offer to Replace Unsold Items with Random Selection";
    public static string Message { get; private set; } = GlobalFunctions.PartiallyTransparentText("Dear Seller,\n\nWe hope this message finds you well.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("We understand that you have been facing difficulties in selling some of your items. As a gesture of support, we would like to ") +
                                                         GlobalFunctions.WhiteColorText("offer ") +
                                                         GlobalFunctions.PartiallyTransparentText("you the option to ") + GlobalFunctions.WhiteColorText("replace all unsold items ") + GlobalFunctions.PartiallyTransparentText("with a random selection of alternatives from our inventory.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("Our aim is to help you explore new possibilities and increase your chances of successful sales. By offering this ") +
                                                         GlobalFunctions.WhiteColorText("replacement option, ") + GlobalFunctions.PartiallyTransparentText("we hope to assist you in overcoming the challenges you may be experiencing with the current items.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("If you are interested in accepting this offer, please let us know that you would like to proceed, and we will provide you with a random selection of potential alternatives.\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("Your satisfaction and success are of utmost importance to us, and we are here to assist you in any way we can.\n\nThank you for being a valued seller, and we are eager to hear your decision.\n\nBest regards");                                                     
}