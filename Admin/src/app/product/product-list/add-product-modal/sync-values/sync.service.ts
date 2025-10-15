import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SyncService {
  private readonly STORAGE_KEY = 'product_sku_sync_settings';

  // Get sync settings from localStorage
  getSyncSettings(): { [key: string]: boolean } {
    const saved = localStorage.getItem(this.STORAGE_KEY);
    if (saved) {
      try {
        return JSON.parse(saved);
      } catch (e) {
        console.error('Error loading sync settings:', e);
      }
    }
    return {};
  }

  // Check if a field should be synced
  shouldSync(fieldKey: string): boolean {
    const settings = this.getSyncSettings();
    return settings[fieldKey] === true;
  }
}
