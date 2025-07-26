import { Component, OnInit } from '@angular/core';
import { endOfMonth, startOfMonth, subDays, subMonths } from 'date-fns';
import { CampaignWithAdsets } from 'src/app/shared/_models/meta-ads/CampaignWithAdsets';
import { StatisticsService } from '../statistics.service';

export interface TreeNodeInterface {
  key: string;
  name: string;
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

  campaigns: CampaignWithAdsets[] = [
    {
      id: "120227937996160083",
      name: "Set thô boil + CV484 - Mes",
      status: "ACTIVE",
      effective_status: "ACTIVE",
      start_time: "2025-07-05T00:00:00+0700",
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
  listOfMapData: TreeNodeInterface[] = [];
  loading = false;

  ngOnInit(): void {
    this.listOfMapData = this.convertCampaignsToTree(this.campaigns);
    this.listOfMapData.forEach(item => {
      this.mapOfExpandedData[item.key] = this.convertTreeToList(item);
    });
    // this.fetchCampaigns();
  }

  constructor(private statisticsService: StatisticsService) {
  }

  fetchCampaigns() {
    this.loading = true;
    // Lấy ngày từ selectedRange, format yyyy-MM-dd
    const since = this.formatDate(this.selectedRange[0]);
    const until = this.formatDate(this.selectedRange[1]);
    this.statisticsService.getCampaignsWithAdsets(since, until).subscribe({
      next: (data) => {
        this.campaigns = data;
        this.listOfMapData = this.convertCampaignsToTree(this.campaigns);
        this.listOfMapData.forEach(item => {
          this.mapOfExpandedData[item.key] = this.convertTreeToList(item);
        });
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        console.log(err);
      }
    });
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

  convertCampaignsToTree(campaigns: CampaignWithAdsets[]): TreeNodeInterface[] {
    return campaigns.map((campaign, i) => ({
      key: (i + 1).toString(),
      name: campaign.name,
      id: campaign.id,
      status: campaign.status,
      effective_status: campaign.effective_status,
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
