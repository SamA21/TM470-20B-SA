import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';

@Component({
  selector: 'fetch-venues',
  templateUrl: './fetch-venues.html',
  styleUrls: ['./fetch-venues.css']
})

export class FetchVenuesComponent {
  public venues: Venues[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Venues[]>(baseUrl + 'venues' + '/GetVenues').subscribe(result => {
      console.log(result);
      this.venues = result;
    }, error => console.error(error));
  }

  editVenue(venue: Venues) {
    $("#EditVenueId").val(venue.id);
    $("#EditVenueName").val(venue.name);
    $("#EditVenueLocation").val(venue.location);
    $("#EditVenueCapacity").val(venue.capacity);

    $('#EditVenueForm').modal();
  }
}

interface Venues {
  id: number;
  name: string;
  location: string;
  capacity: number;
  numberOfEvents: number;
}
