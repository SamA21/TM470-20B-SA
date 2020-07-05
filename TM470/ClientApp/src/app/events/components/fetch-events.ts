import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';
import 'bootstrap-datepicker'
import { EditEventComponent } from './edit-event';

@Component({
  selector: 'fetch-events',
  templateUrl: './fetch-events.html',
  styleUrls: ['./fetch-events.css']
})

export class FetchEventsComponent {
  public events: Events[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

    http.get<Events[]>(baseUrl + 'events' + '/GetEvents').subscribe(result => {
      console.log(result);
      this.events = result;
    }, error => console.error(error));
  }

  editEvent(event: Events) {
    console.log(event)
    $("#EditEventId").val(event.id);
    $("#EditEventName").val(event.name);
    $("#EditEventInformation").val(event.information);
    $("#EditEventCapacity").val(event.eventCapacity);
    $("#EditEventPrice").val(event.ticketPrice);
    
    $("#EditEventDate").val(event.eventDate);
    $("#EditEventLiveDate").val(event.eventLiveDate);

    $('#EditEventForm').modal();
    $('.datepicker').datepicker({
      format: "dd/mm/yyyy",
      orientation: "auto left",
      autoclose: true
    });
  }

}

interface Events {
  id: number;
  name: string;
  information: string,
  eventCapacity: number;
  eventDate: string;
  eventLiveDate: string;
  venue: VenueModel;
  eventType: EventTypeModel;
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
  numberOfEvents: number;
}

interface VenueModel {
  id: number;
  name: string;
  location: string;
  capacity: number;
}

interface EventTypeModel {
  id: number;
  EventTypeName: string;
}
