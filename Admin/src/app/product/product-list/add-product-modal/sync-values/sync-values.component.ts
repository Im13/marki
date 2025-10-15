import { Component, Input, OnInit } from '@angular/core';

interface SyncField {
  key: string;
  label: string;
  checked: boolean;
}

@Component({
  selector: 'app-sync-values',
  templateUrl: './sync-values.component.html',
  styleUrls: ['./sync-values.component.css']
})
export class SyncValuesComponent implements OnInit {
  private readonly STORAGE_KEY = 'product_sku_sync_settings';

  fields: SyncField[] = [
    { key: 'importPrice', label: 'Giá nhập cuối', checked: false },
    { key: 'weight', label: 'Trọng lượng(g)', checked: false },
    { key: 'price', label: 'Giá bán', checked: false },
    { key: 'quantity', label: 'Tồn kho', checked: false },
    { key: 'photos', label: 'Hình ảnh', checked: false },
  ];

  ngOnInit(): void {
    this.loadSettings();
  }

  // Load settings from localStorage
  private loadSettings(): void {
    const saved = localStorage.getItem(this.STORAGE_KEY);
    if (saved) {
      try {
        const settings = JSON.parse(saved);
        this.fields.forEach(field => {
          if (settings[field.key] !== undefined) {
            field.checked = settings[field.key];
          }
        });
      } catch (e) {
        console.error('Error loading sync settings:', e);
      }
    }
  }

  // Save settings to localStorage when checkbox changes
  onCheckboxChange(): void {
    const settings: { [key: string]: boolean } = {};
    this.fields.forEach(field => {
      settings[field.key] = field.checked;
    });
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(settings));
  }
}
