import { SocialAdSet } from "./SocialAdSet";

export interface SocialCampaign {
    id: string;
    name: string;
    status: string;
    effective_status: string;
    start_time?: string;
    objective?: string;
    adSets: SocialAdSet[];
}