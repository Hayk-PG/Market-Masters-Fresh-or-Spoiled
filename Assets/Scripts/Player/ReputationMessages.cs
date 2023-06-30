using UnityEngine;
using Pautik;

public class ReputationMessages : MonoBehaviour
{
    public static string ExcellentReputationMessage
    {
        get => $"{WhiteText("Congratulations!")} {PartiallyTransparentText("You have reached an")} {WhiteText("Excellent Reputation!")}";
    }
    public static string GoodReputationMessage
    {
        get => $"{PartiallyTransparentText("Your hard work has paid off! You are now at the")} {WhiteText("Good Reputation")} {PartiallyTransparentText("level.")}";
    }
    public static string AdvanceNeutralReputationMessage
    {
        get => $"{WhiteText("Well done!")} {PartiallyTransparentText("You have advanced to the next reputation tier:")} {WhiteText("Neutral Reputation.")}";
    }
    public static string PoorReputationMessage
    {
        get => $"{WhiteText("Warning!")} {PartiallyTransparentText("Your reputation has decreased to")} {WhiteText("Poor.")} {PartiallyTransparentText("Take action to improve it!")}";
    }
    public static string TerribleReputationMessage
    {
        get => $"{WhiteText("Oops!")} {PartiallyTransparentText("Your reputation has dropped to")} {WhiteText("Terrible.")} {PartiallyTransparentText("Time to turn things around!")}";
    }
    public static string FallNeutralReputationMessage
    {
        get => $"{PartiallyTransparentText("Your reputation has fallen to")} {WhiteText("Neutral.")} {PartiallyTransparentText("Work on enhancing your image.")}";
    }

    private static string PartiallyTransparentText(string text)
    {
        return GlobalFunctions.TextWithColorCode("#FFFFFFC8", text);
    }

    private static string WhiteText(string text)
    {
        return GlobalFunctions.TextWithColorCode("#FFFFFF", text);
    }
}