import { Component, OnInit } from '@angular/core';
import { endOfMonth, startOfMonth, subDays, subMonths } from 'date-fns';
import { SocialCampaign } from 'src/app/shared/_models/meta-ads/SocialCampaign';
import { StatisticsService } from '../statistics.service';

export interface TreeNodeInterface {
  key: string;
  name: string;
  objective?: string;
  daily_budget?: string;
  spend?: string;
  impressions?: string;
  clicks?: string;
  ctr?: string;
  cpc?: string;
  reach?: string;
  date_start?: string;
  date_stop?: string;
  id?: string;
  status?: string;
  effective_status?: string;
  frequency?: string;
  level?: number;
  expand?: boolean;
  children?: TreeNodeInterface[];
  parent?: TreeNodeInterface;
}

@Component({
  selector: 'app-meta-ads-dashboard',
  templateUrl: './meta-ads-dashboard.component.html',
  styleUrls: ['./meta-ads-dashboard.component.css']
})

export class MetaAdsDashboardComponent implements OnInit {
  ranges = {
    'Hôm nay': [new Date(), new Date()],
    'Hôm qua': [subDays(new Date(), 1), subDays(new Date(), 1)],
    'Tháng này': [startOfMonth(new Date()), endOfMonth(new Date())],
    'Tháng trước': [startOfMonth(subMonths(new Date(), 1)), endOfMonth(subMonths(new Date(), 1))]
  };
  selectedRange = [new Date(), new Date()];
  mapOfExpandedData: { [key: string]: TreeNodeInterface[] } = {};

  //Refactor zone
  metaMapData: TreeNodeInterface[] = [];
  instaMapData: TreeNodeInterface[] = [];

  metaExpandedData: { [key: string]: TreeNodeInterface[] } = {};
  instaExpandedData: { [key: string]: TreeNodeInterface[] } = {};

  totalMetaAdsSpend = 0;
  totalInstagramAdsSpend = 0;

  metaCampaigns: SocialCampaign[] = [
    {
      id: "120227937996160083",
      name: "Set thô boil + CV484 - Mes",
      status: "ACTIVE",
      effective_status: "ACTIVE",
      start_time: "2025-07-05T00:00:00+0700",
      objective: "OUTCOME_ENGAGEMENT",
      adSets: [
        {
          id: "120227937996150083",
          name: "Nhóm quảng cáo Lượt tương tác mới",
          status: "ACTIVE",
          effective_status: "ACTIVE",
          daily_budget: "140000",
          spend: "152221",
          impressions: "3216",
          clicks: "325",
          ctr: "10.105721",
          cpc: "468.372308",
          reach: "2580",
          frequency: "1.246512",
          date_start: "2025-07-18",
          date_stop: "2025-07-18"
        },
        {
          id: "120227937996150083",
          name: "Nhóm quảng cáo Lượt tương tác 1",
          status: "ACTIVE",
          effective_status: "ACTIVE",
          daily_budget: "140000",
          spend: "152221",
          impressions: "3216",
          clicks: "325",
          ctr: "10.105721",
          cpc: "468.372308",
          reach: "2580",
          frequency: "1.246512",
          date_start: "2025-07-18",
          date_stop: "2025-07-18"
        }
      ]
    },
  ];

  instaCampaigns: SocialCampaign[] = [
    {
      id: "120227937996160083",
      name: "Instagram Campaign",
      status: "ACTIVE",
      effective_status: "ACTIVE",
      start_time: "2025-07-05T00:00:00+0700",
      objective: "OUTCOME_ENGAGEMENT",
      adSets: [
        {
          id: "120227937996150083",
          name: "Nhóm quảng cáo Lượt tương tác mới",
          status: "ACTIVE",
          effective_status: "ACTIVE",
          daily_budget: "140000",
          spend: "352221",
          impressions: "3216",
          clicks: "325",
          ctr: "10.105721",
          cpc: "468.372308",
          reach: "2580",
          frequency: "1.246512",
          date_start: "2025-07-18",
          date_stop: "2025-07-18"
        },
        {
          id: "120227937996150083",
          name: "Nhóm quảng cáo Lượt tương tác 1",
          status: "ACTIVE",
          effective_status: "ACTIVE",
          daily_budget: "140000",
          spend: "252221",
          impressions: "3216",
          clicks: "124",
          ctr: "5.23",
          cpc: "468.372308",
          reach: "2580",
          frequency: "1.246512",
          date_start: "2025-07-18",
          date_stop: "2025-07-18"
        }
      ]
    },
  ];

  //End refactor zone
  loading = false;

  ngOnInit(): void {
    this.initializeCampaignData('meta');
    this.initializeCampaignData('insta');

    // this.listOfMapData = this.convertCampaignsToTree(this.metaCampaigns);
    // this.listOfMapData.forEach(item => {
    //   this.mapOfExpandedData[item.key] = this.convertTreeToList(item);
    // });
    // this.totalMetaAdsSpend = this.calculateTotalSpend();
    // this.fetchCampaigns();
  }

  private initializeCampaignData(platform: 'meta' | 'insta'): void {
    const campaigns = platform === 'meta' ? this.metaCampaigns : this.instaCampaigns;
    const mapData = this.convertCampaignsToTree(campaigns);

    if (platform === 'meta') {
      this.metaMapData = mapData;
      this.metaMapData.forEach(item => {
        this.metaExpandedData[item.key] = this.convertTreeToList(item);
      });
      this.totalMetaAdsSpend = this.calculateTotalSpend(this.metaCampaigns);
    } else {
      this.instaMapData = mapData;
      this.instaMapData.forEach(item => {
        this.instaExpandedData[item.key] = this.convertTreeToList(item);
      });
      this.totalInstagramAdsSpend = this.calculateTotalSpend(this.instaCampaigns);
    }
  }

  constructor(private statisticsService: StatisticsService) {
  }

  fetchCampaigns() {
    this.loading = true;
    // Lấy ngày từ selectedRange, format yyyy-MM-dd
    const since = this.formatDate(this.selectedRange[0]);
    const until = this.formatDate(this.selectedRange[1]);
    
    // Fetch Meta Campaigns
    this.statisticsService.getMetaCampaignsWithAdsets(since, until).subscribe({
      next: (data) => {
        this.metaCampaigns = data;
        this.initializeCampaignData('meta');
      },
      error: (err) => {
        console.log('Meta fetch error:', err);
      }
    });

    // Fetch Instagram Campaigns
    this.statisticsService.getMetaCampaignsWithAdsets(since, until).subscribe({
      next: (data) => {
        this.instaCampaigns = data;
        this.initializeCampaignData('insta');
      },
      error: (err) => {
        console.log('Instagram fetch error:', err);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  private calculateTotalSpend(campaigns: SocialCampaign[]): number {
    return campaigns.reduce((total, campaign) => {
      return total + campaign.adSets.reduce((sum, adset) => {
        return sum + Number(adset.spend || 0);
      }, 0);
    }, 0);
  }

  formatDate(date: Date): string {
    return date.toISOString().slice(0, 10);
  }

  onChange(dates: Date[]) {
  }

  dateRangeSelected() {
  }

  collapse(array: TreeNodeInterface[], data: TreeNodeInterface, $event: boolean): void {
    if (!$event) {
      if (data.children) {
        data.children.forEach(d => {
          const target = array.find(a => a.key === d.key)!;
          target.expand = false;
          this.collapse(array, target, false);
        });
      } else {
        return;
      }
    }
  }

  convertCampaignsToTree(campaigns: SocialCampaign[]): TreeNodeInterface[] {
    return campaigns.map((campaign, i) => ({
      key: (i + 1).toString(),
      name: campaign.name,
      id: campaign.id,
      status: campaign.status,
      effective_status: campaign.effective_status,
      objective: campaign.objective,
      // Tính tổng hoặc trung bình cho campaign level
      spend: campaign.adSets.reduce((sum, ad) => sum + Number(ad.spend || 0), 0).toString(),
      impressions: campaign.adSets.reduce((sum, ad) => sum + Number(ad.impressions || 0), 0).toString(),
      clicks: campaign.adSets.reduce((sum, ad) => sum + Number(ad.clicks || 0), 0).toString(),
      // Các trường khác để trống ở cấp campaign
      daily_budget: '',
      ctr: '',
      cpc: '',
      reach: '',
      frequency: '',
      date_start: '',
      date_stop: '',
      level: 0,
      expand: false,
      children: campaign.adSets.map((adset, j) => ({
        key: `${i + 1}-${j + 1}`,
        name: adset.name,
        id: adset.id,
        status: adset.status,
        effective_status: adset.effective_status,
        daily_budget: adset.daily_budget,
        spend: adset.spend,
        impressions: adset.impressions,
        clicks: adset.clicks,
        ctr: adset.ctr,
        cpc: adset.cpc,
        reach: adset.reach,
        frequency: adset.frequency,
        date_start: adset.date_start,
        date_stop: adset.date_stop,
        level: 1,
        expand: false
      }))
    }));
  }

  convertTreeToList(root: TreeNodeInterface): TreeNodeInterface[] {
    const stack: TreeNodeInterface[] = [];
    const array: TreeNodeInterface[] = [];
    const hashMap = {};
    stack.push({ ...root, level: 0, expand: false });

    while (stack.length !== 0) {
      const node = stack.pop()!;
      this.visitNode(node, hashMap, array);
      if (node.children) {
        for (let i = node.children.length - 1; i >= 0; i--) {
          stack.push({ ...node.children[i], level: node.level! + 1, expand: false, parent: node });
        }
      }
    }

    return array;
  }

  visitNode(node: TreeNodeInterface, hashMap: { [key: string]: boolean }, array: TreeNodeInterface[]): void {
    if (!hashMap[node.key]) {
      hashMap[node.key] = true;
      array.push(node);
    }
  }
}
