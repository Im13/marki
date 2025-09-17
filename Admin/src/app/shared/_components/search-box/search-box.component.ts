import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { SearchService } from 'src/app/core/_services/search.service';

@Component({
  selector: 'app-search-box',
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.css']
})
export class SearchBoxComponent implements OnInit {
  searchControl = new FormControl();
  results: any[] = [];
  currentModule: string = 'orders';
  @Output() searchEvent = new EventEmitter<string>();

  constructor(private searchService: SearchService) {}

  ngOnInit(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(300), // Chờ 300ms sau khi gõ xong
        distinctUntilChanged(), // Chỉ trigger nếu giá trị thay đổi
      )
      .subscribe(value => this.searchService.setSearchQuery(value));
  }

  // // Hàm đổi module khi người dùng chọn danh mục khác
  // changeModule(module: string) {
  //   this.currentModule = module;
  //   this.results = []; // Reset kết quả khi đổi module
  // }
}
