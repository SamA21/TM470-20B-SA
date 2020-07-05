import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';

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

  editVenue(event: Events) {
    console.log(event)
    //$("#EditVenueId").val(event.id);
    //$("#EditVenueName").val(event.name);
    //$("#EditVenueLocation").val(event.location);
    //$("#EditVenueCapacity").val(event.capacity);

    //$('#EditVenueForm').modal();
  }
}

interface Events {
  id: number;
  name: string;
  information: string,
  eventCapacity: number;
  venue: VenueModel;
  eventType: EventTypeModel;
  ticketPrice: number;
  peopleIntrested: number;
  ticketsSold: number;
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
