using Core.Enums;

namespace Core.Extensions
{
    public static class CampaignObjectiveExtensions
    {
        public static string ToShortString(this CampaignObjective objective)
        {
            return objective switch
            {
                CampaignObjective.OUTCOME_APP_PROMOTION => "OAP",
                CampaignObjective.OUTCOME_AWARENESS => "OA",
                CampaignObjective.OUTCOME_ENGAGEMENT => "OE",
                CampaignObjective.OUTCOME_LEADS => "OL",
                CampaignObjective.OUTCOME_SALES => "OS",
                CampaignObjective.OUTCOME_TRAFFIC => "OT",
                CampaignObjective.APP_INSTALLS => "AI",
                CampaignObjective.BRAND_AWARENESS => "BA",
                CampaignObjective.CONVERSIONS => "C",
                CampaignObjective.EVENT_RESPONSES => "ER",
                CampaignObjective.LEAD_GENERATION => "LG",
                CampaignObjective.LINK_CLICKS => "LC",
                CampaignObjective.LOCAL_AWARENESS => "LA",
                CampaignObjective.MESSAGES => "M",
                CampaignObjective.OFFER_CLAIMS => "OC",
                CampaignObjective.PAGE_LIKES => "PL",
                CampaignObjective.POST_ENGAGEMENT => "PE",
                CampaignObjective.PRODUCT_CATALOG_SALES => "PCS",
                CampaignObjective.REACH => "R",
                CampaignObjective.STORE_VISITS => "SV",
                CampaignObjective.VIDEO_VIEWS => "VV",
                _ => string.Empty
            };
        }

        public static CampaignObjective ToCampaignObjective(this string objective)
        {
            return objective switch
            {
                "OUTCOME_APP_PROMOTION" => CampaignObjective.OUTCOME_APP_PROMOTION,
                "OUTCOME_AWARENESS" => CampaignObjective.OUTCOME_AWARENESS,
                "OUTCOME_ENGAGEMENT" => CampaignObjective.OUTCOME_ENGAGEMENT,
                "OUTCOME_LEADS" => CampaignObjective.OUTCOME_LEADS,
                _ => throw new ArgumentException($"Invalid objective: {objective}")
            };
        }
    }
}