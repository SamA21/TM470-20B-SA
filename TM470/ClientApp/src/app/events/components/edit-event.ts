import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';
@Component({
  selector: 'edit-event',
  templateUrl: './edit-event.html'
})
export class EditEventComponent {
  public event: EventEditModel;
  public ErrorMessageShow: boolean;

  private _http: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
    this.event = <EventEditModel>{};
    this.ErrorMessageShow = false;
  }

  public SubmitEditVenue() {
    this.ErrorMessageShow = false;
    var Id = $("#EditVenueId").val();
    this.event.name = $("#EditVenueName").val();
    this.event.location = $("#EditVenueLocation").val();
    var capacity = $("#EditVenueCapacity").val();
    if (parseInt(Id) != NaN) {
      this.event.id = parseInt(Id);
      if (parseInt(capacity) != NaN) {
        this.event.capacity = parseInt(capacity);
        var url = this._baseUrl + 'venues' + '/SubmitEditVenue';
        this._http.post<EventEditResult>(url, { Id: this.event.id, Name: this.event.name, Location: this.event.location, Capacity: this.event.capacity })
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

interface EventEditModel{
  id: number;
  name: string;
  location: string;
  capacity: number;
}


interface EventEditResult {
  message: string;
}
