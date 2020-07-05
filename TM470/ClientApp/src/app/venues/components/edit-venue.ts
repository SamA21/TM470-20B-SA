import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';
@Component({
  selector: 'edit-venue',
  templateUrl: './edit-venue.html'
})
export class EditVenueComponent {
  public venue: VenueEditModel;
  public ErrorMessageShow: boolean;

  private _http: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
    this.venue = <VenueEditModel>{};
    this.ErrorMessageShow = false;
  }

  public SubmitEditVenue() {
    this.ErrorMessageShow = false;
    var Id = $("#EditVenueId").val();
    this.venue.name = $("#EditVenueName").val();
    this.venue.location = $("#EditVenueLocation").val();
    var capacity = $("#EditVenueCapacity").val();
    if (parseInt(Id) != NaN) {
      this.venue.id = parseInt(Id);
      if (parseInt(capacity) != NaN) {
        this.venue.capacity = parseInt(capacity);
        var url = this._baseUrl + 'venues' + '/SubmitEditVenue';
        this._http.post<VenueEditResult>(url, { Id: this.venue.id, Name: this.venue.name, Location: this.venue.location, Capacity: this.venue.capacity })
          .subscribe(result => {
            if (result.message == "Edited new Venue") {
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
    else {
      this.ErrorMessageShow = true;
    }
  }
}

interface VenueEditModel{
  id: number;
  name: string;
  location: string;
  capacity: number;
}


interface VenueEditResult {
  message: string;
}
