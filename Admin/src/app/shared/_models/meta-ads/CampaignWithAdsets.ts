import { AdsetInsight } from "./AdsetInsight";

export interface CampaignWithAdsets {
    id: string;
    name: string;
    status: string;
    effective_status: string;
    start_time?: string;
    adSets: AdsetInsight[];
}