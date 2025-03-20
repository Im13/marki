import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { SlideImage } from '../shared/_models/slideImages';

@Injectable({
  providedIn: 'root'
})
export class WebsiteServiceService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addBanner(slideImage: SlideImage) {
    return this.http.post(this.baseUrl + 'websettings/create-slide', slideImage);
  }
}
