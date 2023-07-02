using Pautik;

public class SaleRestrictionMessages 
{
    public static string FiveTurns => $"{GlobalFunctions.WhiteColorText("5-Turns Sale Restriction:")} {GlobalFunctions.PartiallyTransparentText("Due to unforeseen circumstances, a temporary sale restriction will be imposed for the next")} " +
                                     $"{GlobalFunctions.WhiteColorText("5 turns.")} {GlobalFunctions.PartiallyTransparentText("We apologize for any inconvenience caused and appreciate your understanding.")}";

    public static string FourTurns => $"{GlobalFunctions.WhiteColorText("4-Turns Sale Restriction: Attention!")} {GlobalFunctions.PartiallyTransparentText("A sale restriction will be in effect for the upcoming")} " +
                                     $"{GlobalFunctions.WhiteColorText("4 turns.")} {GlobalFunctions.PartiallyTransparentText("We are working diligently to resolve the issue and appreciate your patience during this period.")}";

    public static string TwoTurns => $"{GlobalFunctions.WhiteColorText("2-Turns Sale Restriction: Important Announcement!")} {GlobalFunctions.PartiallyTransparentText("Prepare for a")} " +
                                     $"{GlobalFunctions.WhiteColorText("2-turn")} {GlobalFunctions.PartiallyTransparentText("sale restriction starting immediately. We apologize for the temporary inconvenience and thank you for your cooperation.")}";
}