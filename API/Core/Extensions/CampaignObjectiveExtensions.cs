using Core.Enums;

namespace Core.Extensions
{
    public static class CampaignObjectiveExtensions
    {
        public static string ToShortString(this CampaignObjective objective)
        {
            return objective switch
            {
                CampaignObjective.OUTCOME_TRAFFIC => "Lưu lượng truy cập",
                CampaignObjective.OUTCOME_AWARENESS => "Mức độ nhận biết",
                CampaignObjective.OUTCOME_ENGAGEMENT => "Lượt tương tác",
                CampaignObjective.LEAD_GENERATION => "Khách hàng tiềm năng",
                CampaignObjective.OUTCOME_APP_PROMOTION => "Quảng cáo ứng dụng",
                CampaignObjective.OUTCOME_SALES => "Doanh số",
                CampaignObjective.BRAND_AWARENESS => "Mức độ nhận biết thương hiệu",
                CampaignObjective.REACH => "Số người tiếp cận",
                CampaignObjective.APP_INSTALLS => "Lượt cài đặt ứng dụng",
                CampaignObjective.VIDEO_VIEWS => "Lượt xem video",
                CampaignObjective.OUTCOME_LEADS => "Tìm kiếm khách hàng tiềm năng",
                CampaignObjective.MESSAGES => "Tin nhắn",
                CampaignObjective.CONVERSIONS => "Chuyển đổi",
                CampaignObjective.PRODUCT_CATALOG_SALES => "Doanh số theo danh mục",
                CampaignObjective.STORE_VISITS => "Lưu lượng khách đến cửa hàng",
                CampaignObjective.EVENT_RESPONSES => "Phản hồi sự kiện",
                CampaignObjective.LINK_CLICKS => "Lượt click liên kết",
                CampaignObjective.LOCAL_AWARENESS => "Nhận thức lân cận",
                CampaignObjective.OFFER_CLAIMS => "Nhận ưu đãi",
                CampaignObjective.PAGE_LIKES => "Lượt thích trang",
                CampaignObjective.POST_ENGAGEMENT => "Tương tác bài viết",
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
                "OUTCOME_SALES" => CampaignObjective.OUTCOME_SALES,
                "OUTCOME_TRAFFIC" => CampaignObjective.OUTCOME_TRAFFIC,
                "APP_INSTALLS" => CampaignObjective.APP_INSTALLS,
                "BRAND_AWARENESS" => CampaignObjective.BRAND_AWARENESS,
                "CONVERSIONS" => CampaignObjective.CONVERSIONS,
                "EVENT_RESPONSES" => CampaignObjective.EVENT_RESPONSES,
                "LEAD_GENERATION" => CampaignObjective.LEAD_GENERATION,
                "LINK_CLICKS" => CampaignObjective.LINK_CLICKS,
                "LOCAL_AWARENESS" => CampaignObjective.LOCAL_AWARENESS,
                "MESSAGES" => CampaignObjective.MESSAGES,
                "OFFER_CLAIMS" => CampaignObjective.OFFER_CLAIMS,
                "PAGE_LIKES" => CampaignObjective.PAGE_LIKES,
                "POST_ENGAGEMENT" => CampaignObjective.POST_ENGAGEMENT,
                "PRODUCT_CATALOG_SALES" => CampaignObjective.PRODUCT_CATALOG_SALES,
                "REACH" => CampaignObjective.REACH,
                "STORE_VISITS" => CampaignObjective.STORE_VISITS,
                "VIDEO_VIEWS" => CampaignObjective.VIDEO_VIEWS,
                _ => throw new ArgumentException($"Invalid objective: {objective}")
            };
        }
    }
}