using Pautik;
using System;

public class NegativeReviews 
{
    // Negative Reviews:
    private static Tuple<string, string> TerribleQualityControl => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Terrible Quality Control: Disappointing Product Experience")}",
         $"{GlobalFunctions.WhiteColorText("Review:\n\n")}{GlobalFunctions.PartiallyTransparentText("I recently purchased a product from this business, and to my dismay, it was spoiled beyond consumption. The lack of quality control is evident, and I would caution others not to waste their hard-earned money here. It's disappointing when you have high expectations and end up with a subpar experience.")}");

    private static Tuple<string, string> UnreliableService => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Unreliable Service: Frustrating Delays and Unhelpful Support")}",
         $"{GlobalFunctions.WhiteColorText("Review:\n\n")}{GlobalFunctions.PartiallyTransparentText("I had the misfortune of placing multiple orders with this establishment, and each time I encountered delays and unhelpful customer support. The consistently late deliveries and the lack of assistance in resolving issues left me highly dissatisfied. If you value promptness and efficient service, I would advise looking elsewhere.")}");

    private static Tuple<string, string> OverpricedItems => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Overpriced Items: Inflated Prices for Subpar Quality")}",
         $"{GlobalFunctions.WhiteColorText("Review:\n\n")}{GlobalFunctions.PartiallyTransparentText("I regretfully admit that I fell victim to the allure of this business, only to be met with disappointment. The prices charged were exorbitant, especially considering the quality of the products offered. I expected better value for my money and was left feeling let down. I caution potential customers to be aware of the inflated prices before making a purchase.")}");

    private static Tuple<string, string> PoorHygieneStandards => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Poor Hygiene Standards: Unsanitary Conditions and Hair in Food")}",
         $"{GlobalFunctions.WhiteColorText("Review\n\n")}{GlobalFunctions.PartiallyTransparentText("During my recent visit to this establishment, I was appalled by the unclean conditions I witnessed. From dirty tables to unsanitary food handling practices, it was evident that hygiene standards were not a priority. To make matters worse, I discovered a hair in my food, leaving me with a sense of disgust and a loss of trust in the establishment's commitment to cleanliness.")}");

    private static Tuple<string, string> LackOfTransparency => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Lack of Transparency: Hidden Fees and Misleading Advertising")}",
         $"{GlobalFunctions.WhiteColorText("Review:\n\n")}{GlobalFunctions.PartiallyTransparentText("My experience with this business left me feeling deceived. I encountered hidden fees and encountered misleading advertising, which misrepresented the true cost of their products. Transparency is crucial in building trust with customers, and unfortunately, this business fell short. I advise potential customers to exercise caution and seek clarity on pricing before making any commitments.")}");

    // Negative Publicity:
    private static Tuple<string, string> ThreatsToReputation => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Controversial Practices Under Scrutiny: Threats to Reputation")}",
         $"{GlobalFunctions.WhiteColorText("News Headline:\n\n")}{GlobalFunctions.PartiallyTransparentText("Recent revelations have shed light on the questionable practices employed by this business. Customers and industry experts are expressing concerns regarding ethical standards and business conduct. The negative publicity surrounding these practices is threatening to tarnish the company's reputation and undermine its credibility.")}");

    private static Tuple<string, string> LegalTroublesLooming => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Legal Troubles Looming: Alleged Violations Draw Attention")}",
         $"{GlobalFunctions.WhiteColorText("News Headline:\n\n")}{GlobalFunctions.PartiallyTransparentText("Reports have emerged that suggest this business is facing potential legal ramifications due to alleged violations of consumer protection laws. Authorities are investigating claims of misleading advertising and deceptive practices, leaving the company's reputation at stake and customers wary of engaging with their services.")}");

    private static Tuple<string, string> CallsForChange => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Public Outcry Over Environmental Impact: Calls for Change")}",
         $"{GlobalFunctions.WhiteColorText("News Headline:\n\n")}{GlobalFunctions.PartiallyTransparentText("Environmental activists and concerned citizens have raised their voices against this business due to its perceived negative impact on the environment. The company's practices and lack of sustainable initiatives have sparked a public outcry, resulting in a wave of negative publicity and a call for greater corporate responsibility.")}");

    private static Tuple<string, string> TrustAtRisk => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Data Breach Exposes Customer Information: Trust at Risk")}",
         $"{GlobalFunctions.WhiteColorText("News Headline:\n\n")}{GlobalFunctions.PartiallyTransparentText("A recent cybersecurity breach has left customers' personal information compromised. The breach has caused significant distress among those affected and has damaged the company's reputation for safeguarding customer data. Swift action and transparency will be necessary to rebuild trust and mitigate the fallout from this incident.")}");

    private static Tuple<string, string> EthicalConcernsRaised => new Tuple<string, string>
        ($"{GlobalFunctions.WhiteColorText("Supplier Controversy Shakes Confidence: Ethical Concerns Raised")}",
         $"{GlobalFunctions.WhiteColorText("News Headline:\n\n")}{GlobalFunctions.PartiallyTransparentText("This business finds itself embroiled in a supplier controversy, with allegations of unethical sourcing and exploitative labor practices surfacing. The negative publicity surrounding these allegations has triggered consumer concerns and raised questions about the company's commitment to social responsibility. The business now faces the challenge of addressing these concerns and regaining public trust.")}");

    public static Tuple<string, string>[] Texts => new Tuple<string, string>[] 
    {
        TerribleQualityControl,
        UnreliableService,
        OverpricedItems,
        PoorHygieneStandards,
        LackOfTransparency,
        ThreatsToReputation,
        LegalTroublesLooming,
        CallsForChange,
        TrustAtRisk,
        EthicalConcernsRaised
    };

    private static string[] ReviewsAndMediaPublicities => new string[]
    {
        $"{GlobalFunctions.WhiteColorText("Terrible Quality Control:")} {GlobalFunctions.PartiallyTransparentText("Bought their product, and it was spoiled! Don't waste your money here.")}",
        $"{GlobalFunctions.WhiteColorText("Unresponsive Customer Support:")} {GlobalFunctions.PartiallyTransparentText("No help when I needed it the most. Very disappointing.")}",
        $"{GlobalFunctions.WhiteColorText("Inconsistent Product Performance:")} {GlobalFunctions.PartiallyTransparentText("Sometimes it works, sometimes it doesn't. Frustrating experience.")}",
        $"{GlobalFunctions.WhiteColorText("Lack of Transparency:")} {GlobalFunctions.PartiallyTransparentText("Hidden fees and surprise charges. Not trustworthy.")}",
        $"{GlobalFunctions.WhiteColorText("Shipping Nightmares:")} {GlobalFunctions.PartiallyTransparentText("Orders constantly delayed or lost. Poor logistics management.")}",
        $"{GlobalFunctions.WhiteColorText("Controversial Practices Uncovered:")} {GlobalFunctions.PartiallyTransparentText("Company accused of exploiting workers and violating labor laws.")}",
        $"{GlobalFunctions.WhiteColorText("Product Safety Concerns:")} {GlobalFunctions.PartiallyTransparentText("Investigation reveals potential hazards and inadequate safety measures.")}",
        $"{GlobalFunctions.WhiteColorText("Data Breach Exposes Customer Information:")} {GlobalFunctions.PartiallyTransparentText("Company faces backlash for failing to secure sensitive data.")}",
        $"{GlobalFunctions.WhiteColorText("Environmental Violations Unveiled:")} {GlobalFunctions.PartiallyTransparentText("Company fined for improper waste disposal and pollution.")}",
        $"{GlobalFunctions.WhiteColorText("Financial Scandal Rocks Company:")} {GlobalFunctions.PartiallyTransparentText("Allegations of embezzlement and fraudulent accounting practices surface.")}",
    };
}