import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import $ from 'jquery';
import 'bootstrap';
import 'bootstrap-datepicker'

@Component({
  selector: 'create-event',
  templateUrl: './create-event.html'
})
export class CreateEventComponent {
  public newEvent: CreateEventViewModel;
  public ErrorMessageShow: boolean;
  public eventTypes: EventTypes[];
  public venues: Venues[];
  public selectedEventType: EventTypes;
  public selectedEventVenue: Venues;

  private _http: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._http = http;
    this._baseUrl = baseUrl;
    this.newEvent = <CreateEventViewModel>{};
    this.ErrorMessageShow = false;
    this.selectedEventType = <EventTypes>{};
    this.selectedEventType.type = "Select Event Type";
    this.selectedEventVenue = <Venues>{};
    this.selectedEventVenue.name = "Select Event Venue";  
  }

  private yyyymmdd(dateIn) {
    var yyyy = dateIn.getFullYear();
    var mm = dateIn.getMonth() + 1; // getMonth() is zero-based
    var dd = dateIn.getDate();
    return String(10000 * yyyy + "-" + 100 * mm + "-" +  dd); // Leading zeros for mm and dd
  }


  public CreateEvent() {
    $('#CreateEventForm').modal();
    $('.datepicker').datepicker({
      format: "dd/mm/yyyy",
      orientation: "auto left",
      autoclose: true
    });
    var url = this._baseUrl + 'events' + '/GetEventTypes';
    this._http.get<EventTypes[]>(url)
      .subscribe(result => {
        this.eventTypes = result
      }, error => console.error(error));
    var url2 = this._baseUrl + 'events' + '/GetVenues';
    this._http.get<Venues[]>(url2)
      .subscribe(result => {
        this.venues = result;
      }, error => console.error(error));
  }

  public SelectEventType(eventType) {
    this.selectedEventType = eventType;
  }

  public SelectEventVenue(venue) {
    this.selectedEventVenue = venue;
  }

  public SubmitEvent() {
    this.ErrorMessageShow = false;

    this.newEvent.name = $("#EventName").val();
    this.newEvent.information = $("#EventInformation").val();
    this.newEvent.venueId = this.selectedEventVenue.id;
    this.newEvent.eventTypeId = this.selectedEventType.id;
    let capacity = $("#EventCapacity").val();
    let price = $("#EventPrice").val();
    let eventDate = $("#EventDate").val()
    let eventLiveDate = $("#EventLiveDate").val();

    if (parseInt(capacity) != NaN && capacity.trim() != "") {
      this.newEvent.eventCapacity = capacity;
    }
    else {
      this.newEvent.eventCapacity = this.selectedEventVenue.capacity;
    }

    if (parseFloat(price) != NaN && parseFloat(price) > 0) {
      this.newEvent.ticketPrice = parseFloat(price);
    } else {
      this.newEvent.ticketPrice = 0;
    }

    if (Date.parse(eventDate) != NaN) {
      if (Date.parse(eventLiveDate) != NaN) {

        let data = {
          Name: this.newEvent.name,
          Information: this.newEvent.information,
          VenueId: this.newEvent.venueId,
          EventTypeId: this.newEvent.eventTypeId,
          EventCapacity: this.newEvent.eventCapacity,
          TicketPrice: this.newEvent.ticketPrice,
          EventDate: eventDate,
          EventLiveDate: eventLiveDate
        }

        let url = this._baseUrl + 'events' + '/SubmitNewEvent';
        this._http.post<EventResult>(url, data)
          .subscribe(result => {
            if (result.message == "Created new Event") {
              window.location.reload();
            } else {
              this.ErrorMessageShow = true;
            }

          }, error => console.error(error));
      }
    }
  }
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


interface CreateEventViewModel {
  name: string;
  information: string,
  eventCapacity: number;
  venueId: number;
  eventTypeId: number;
  ticketPrice: number;
}

interface EventResult {
  message: string;
}

