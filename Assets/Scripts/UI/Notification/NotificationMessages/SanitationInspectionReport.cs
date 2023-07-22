using Pautik;

public class SanitationInspectionReport 
{
    public static string IssueTitle { get; private set; } = "Sanitation Inspection Report";
    public static string IssueMessage { get; private set; } = GlobalFunctions.PartiallyTransparentText("Dear Seller,\n\nDuring a recent inspection, our sanitation team identified some issues that require immediate attention:") +
                                                         GlobalFunctions.WhiteColorText("1. Kitchen Hygiene\n2. Waste Disposal\n3. Pest Control\n\n") +
                                                         GlobalFunctions.PartiallyTransparentText("Please address these concerns promptly to ensure compliance with sanitation standards. Once the improvements are made, inform us for a follow-up inspection.\n\nThank you.\n\nSincerely,\nSanitation Department");

    public static string NoIssueTitle { get; private set; } = "Sanitation Inspection Report - No Issues Found";
    public static string NoIssueMessage { get; private set; } = GlobalFunctions.PartiallyTransparentText("Dear Seller,\n\nWe hope this message finds you well.\n\nWe are pleased to inform you that our sanitation inspector recently conducted an inspection of the premises, and we are happy to report that ") +
                                                                GlobalFunctions.WhiteColorText("no issues were found during the evaluation.\n\nCongratulations ") +
                                                                GlobalFunctions.PartiallyTransparentText("on maintaining a high standard of sanitation and cleanliness within your premises. Your dedication to cleanliness is commendable, and it reflects positively on your commitment to providing a safe and hygienic environment.\n\n") +
                                                                GlobalFunctions.WhiteColorText("Thank you ") + GlobalFunctions.PartiallyTransparentText("for your efforts in upholding sanitation standards, and we encourage you to continue the excellent work.\n\n") + GlobalFunctions.WhiteColorText("Keep up the good work!\n\n") +
                                                                GlobalFunctions.PartiallyTransparentText("Sincerely,\nSanitation Department");
}