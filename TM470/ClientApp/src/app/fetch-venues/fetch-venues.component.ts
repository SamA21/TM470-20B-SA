import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-venues',
  templateUrl: './fetch-venues.component.html'
})
export class FetchVenuesComponent {
  public venues: Venues[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Venues[]>(baseUrl + 'venues' + '/GetVenues').subscribe(result => {
      console.log(result);
      this.venues = result;
    }, error => console.error(error));
  }
}

interface Venues {
  name: string;
  location: string;
}
