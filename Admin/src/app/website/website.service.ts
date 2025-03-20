import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { SlideImage } from '../shared/_models/slideImages';

@Injectable({
  providedIn: 'root'
})
export class WebsiteService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addBanner(slideImage: SlideImage) {
    return this.http.post(this.baseUrl + 'websettings/create-slide', slideImage);
  }

  getSlides() {
    return this.http.get<SlideImage[]>(this.baseUrl + 'websettings/get-slides');
  }

  updateSlide(slideImage: SlideImage) {
    return this.http.put(this.baseUrl + 'websettings/update-slide', slideImage);
  }
}
