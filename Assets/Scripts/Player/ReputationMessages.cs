using UnityEngine;
using Pautik;

public class ReputationMessages : MonoBehaviour
{
    public static string ExcellentReputationMessage
    {
        get => $"{GlobalFunctions.WhiteColorText("Congratulations!")} {GlobalFunctions.PartiallyTransparentText("You have reached an")} {GlobalFunctions.WhiteColorText("Excellent Reputation!")}";
    }
    public static string GoodReputationMessage
    {
        get => $"{GlobalFunctions.PartiallyTransparentText("Your hard work has paid off! You are now at the")} {GlobalFunctions.WhiteColorText("Good Reputation")} {GlobalFunctions.PartiallyTransparentText("level.")}";
    }
    public static string AdvanceNeutralReputationMessage
    {
        get => $"{GlobalFunctions.WhiteColorText("Well done!")} {GlobalFunctions.PartiallyTransparentText("You have advanced to the next reputation tier:")} {GlobalFunctions.WhiteColorText("Neutral Reputation.")}";
    }
    public static string PoorReputationMessage
    {
        get => $"{GlobalFunctions.WhiteColorText("Warning!")} {GlobalFunctions.PartiallyTransparentText("Your reputation has decreased to")} {GlobalFunctions.WhiteColorText("Poor.")} {GlobalFunctions.PartiallyTransparentText("Take action to improve it!")}";
    }
    public static string TerribleReputationMessage
    {
        get => $"{GlobalFunctions.WhiteColorText("Oops!")} {GlobalFunctions.PartiallyTransparentText("Your reputation has dropped to")} {GlobalFunctions.WhiteColorText("Terrible.")} {GlobalFunctions.PartiallyTransparentText("Time to turn things around!")}";
    }
    public static string FallNeutralReputationMessage
    {
        get => $"{GlobalFunctions.PartiallyTransparentText("Your reputation has fallen to")} {GlobalFunctions.WhiteColorText("Neutral.")} {GlobalFunctions.PartiallyTransparentText("Work on enhancing your image.")}";
    }
}