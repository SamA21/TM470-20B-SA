import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';

@Component({
  selector: 'create-venue',
  templateUrl: './create-venue.html'
})
export class CreateVenueComponent {
  public venue: VenueViewModel;
  public ErrorMessageShow: boolean;

  private _http: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
    this.venue = <VenueViewModel>{};
    this.ErrorMessageShow = false;
  }

  public CreateVenue() {
    $('#CreateVenueForm').modal();
  }

  public SubmitVenue() {
    this.ErrorMessageShow = false;

    this.venue.name = $("#VenueName").val();
    this.venue.location = $("#VenueLocation").val();
    var capacity = $("#VenueCapacity").val();
    if (parseInt(capacity) != NaN) {
      this.venue.capacity = parseInt(capacity);
      var url = this._baseUrl + 'venues' + '/SubmitNewVenue';
      this._http.post<VenueResult>(url, { Name: this.venue.name, Location: this.venue.location,  Capacity: this.venue.capacity })
        .subscribe(result => {
          if (result.message == "Created new Venue") {
            window.location.reload();
          } else {
            this.ErrorMessageShow = true;
          }

        }, error => console.error(error));
    }
    else {
      this.ErrorMessageShow = true;
    }
  }
}

interface VenueViewModel {
  name: string;
  location: string;
  capacity: number;
}

interface VenueResult {
  message: string;
}

