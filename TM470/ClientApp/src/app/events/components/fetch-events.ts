import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import $ from 'jquery';
import 'bootstrap';
import 'bootstrap-datepicker'

@Component({
  selector: 'fetch-events',
  templateUrl: './fetch-events.html',
  styleUrls: ['./fetch-events.css']
})


export class FetchEventsComponent {
  public events: Events[];
  public selectedEvent: Events;
  public ErrorMessageShow: boolean;

  public eventTypes: EventTypes[];
  public venues: Venues[];
  public selectedEditEventType: EventTypes;
  public selectedEditEventVenue: Venues;


  private _http: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.selectedEvent = <Events>{};
    this.selectedEditEventType = <EventTypes>{};
    this.selectedEditEventVenue = <Venues>{};
    this._baseUrl = baseUrl;;
    this._http = http;

    http.get<Events[]>(baseUrl + 'events' + '/GetEvents').subscribe(result => {
      this.events = result;
    }, error => console.error(error)); 
  }

  editEvent(event: Events) {
    this.selectedEvent = event;
    console.log(this.selectedEvent);


    var url = this._baseUrl + 'events' + '/GetEventTypes';
    this._http.get<EventTypes[]>(url)
      .subscribe(result => {
        this.eventTypes = result
      }, error => console.error(error));
    var url2 = this._baseUrl + 'venues' + '/GetVenues';
    this._http.get<Venues[]>(url2)
      .subscribe(result => {
        this.venues = result;
      }, error => console.error(error));
    this.selectedEditEventType.id = this.selectedEvent.eventType.eventTypeId;
    this.selectedEditEventType.type = this.selectedEvent.eventType.eventTypeName;
    this.selectedEditEventVenue.id = this.selectedEvent.venue.venueId;
    this.selectedEditEventVenue.name = this.selectedEvent.venue.venueName;
    $('#EditEventForm').modal();
    $('.datepicker').datepicker({
      format: "dd/mm/yyyy",
      orientation: "auto left",
      autoclose: true
    });
  }

  public SubmitEditEvent() {
    this.ErrorMessageShow = false;
    var Id = $("#EditVenueId").val();
    var capacity = $("#EditVenueCapacity").val();
    if (parseInt(Id) != NaN) {
      this.selectedEvent.id = parseInt(Id);
      if (parseInt(capacity) != NaN) {
        this.selectedEvent.eventCapacity = parseInt(capacity);
        var url = this._baseUrl + 'events' + '/SubmitEditEvent';
        let data = {
          Id: this.selectedEvent.id,
          Name: this.selectedEvent.name
        }
        this._http.post<EventEditResult>(url, data)
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

  public SelectEditEventType(eventType) {
    this.selectedEditEventType = eventType;
  }

  public SelectEditEventVenue(venue) {
    this.selectedEditEventVenue = venue;
  }

}

interface Events {
  id: number;
  name: string;
  information: string,
  eventCapacity: number;
  eventDate: string;
  eventLiveDate: string;
  venue: Venue;
  eventType: EventType;
  ticketPrice: number;
  peopleIntrested: number;
  ticketsSold: number;
}

interface EventTypes {
  id: number;
  type: string;
}

interface Venues {
  id: number;
  name: string;
  location: string;
  capacity: number;
}


interface EventType {
  eventTypeId: number;
  eventTypeName: string;
}

interface Venue {
  venueId: number;
  venueName: string;
}

interface EventTypeModel {
  id: number;
  EventTypeName: string;
}

interface EventEditResult {
  message: string;
}
