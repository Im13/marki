using Core.Entities;

namespace API.Tests.TestData
{
    public static class MockCampaignData
    {
        public static List<CampaignWithAdsets> GetSampleCampaigns()
        {
            return new List<CampaignWithAdsets>
            {
                new CampaignWithAdsets
                {
                    Id = 1,
                    Name = "Set thô boil + CV484 - Mes",
                    Status = "ACTIVE",
                    StartTime = DateTime.Parse("2025-07-05T00:00:00+0700"),
                    EffectiveStatus = "ACTIVE",
                    FacebookId = "120227937996160083",
                    AdSets = new List<AdsetWithMetrics>
                    {
                        new AdsetWithMetrics
                        {
                            Id = 1,
                            Name = "Nhóm quảng cáo Lượt tương tác mới",
                            Status = "ACTIVE",
                            EffectiveStatus = "ACTIVE",
                            DailyBudget = 140000,
                            FacebookId = "120227937996150083",
                            CampaignId = 1,
                            // Metrics data
                            Spend = 152221,
                            Impressions = 3216,
                            Clicks = 325,
                            Ctr = 10.105721m,
                            Cpc = 468.372308m,
                            Reach = 2580,
                            Frequency = 1.246512m,
                            MetricsDateStart = DateTime.Parse("2025-07-18"),
                            MetricsDateStop = DateTime.Parse("2025-07-18")
                        }
                    }
                }
            };
        }
    }
}