import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';

@Component({
  selector: 'app-create-venues',
  templateUrl: './create-venues.component.html'
})
export class CreateVenuesComponent {
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
    var url = this._baseUrl + 'venues' + '/SubmitNewVenue';
    this._http.post<VenueResult>(url, { Location: this.venue.location, Name: this.venue.name})
      .subscribe(result => {
        if (result.message == "Created new Venue") {
          window.location.reload();
        } else {
          this.ErrorMessageShow = true;
        }
        
    }, error => console.error(error));
  }
}

interface VenueViewModel {
  name: string;
  location: string;
}

interface VenueResult {
  message: string;
}

