import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import $ from 'jquery';
import 'bootstrap';
import 'bootstrap-datepicker'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.css']
})

export class HomeComponent {
  public events: Events[];
  public selectedEvent: Events;
  public ErrorMessageShow: boolean;
  public personInterested: PersonInterested;
  public interestLevels: InterestLevel[];
  public selectedInterestLevel: InterestLevel;
  private _http: HttpClient;
  private _baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;;
    this._http = http;
    this.selectedInterestLevel = <InterestLevel>{};
    this.personInterested = <PersonInterested>{};

    http.get<Results>(baseUrl + 'events' + '/GetEvents').subscribe(result => {
      console.log(result);
      this.events = result.events;
      let now = new Date();
      this.events.forEach(function (value, index) {
        // Do stuff with value or index
        let eventString = value.eventDate.split("/");
        var eventDate = new Date(parseInt(eventString[2]), parseInt(eventString[1]) - 1, parseInt(eventString[0]));
        var diff = Math.abs(now.getTime() -eventDate.getTime());
        var diffDays = Math.ceil(diff / (1000 * 3600 * 24));
        value.daysUntilEvent = diffDays;
      });
    }, error => console.error(error));


  }

  selectEvent(event: Events) {
    this.selectedEvent = event;
    console.log(this.selectedEvent);
    $('#selectedEvent').modal();
    var url = this._baseUrl + 'home' + '/GetInterestLevels';
    this._http.get<FetchLevelsResult>(url)
      .subscribe(result => {
        console.log(result);
        this.interestLevels = result.levels;
      }, error => console.error(error));
    this.selectedInterestLevel.level = "Select Event Type";
  }

  public SubmitInterest() {
    this.ErrorMessageShow = false;

    this.personInterested.name = $("#InterestName").val();
    if ($("#InterestEmail").val() == $("#InterestConfirmEmail").val()) {
      this.personInterested.email = $("#InterestEmail").val();
      if (this.selectedInterestLevel != null) {
        var url = this._baseUrl + 'home' + '/SubmitNewInterest';
        this._http.post<InterestResult>(url, { EventId: this.selectedEvent.id, Name: this.personInterested.name, Email: this.personInterested.email, Level: this.selectedInterestLevel })
          .subscribe(result => {
            if (result.message == "Created new Interest") {
              window.location.reload();
            } else {
              this.ErrorMessageShow = true;
            }

          }, error => console.error(error));
      } else {
        this.ErrorMessageShow = true;
      }
    }
    else {
      this.ErrorMessageShow = true;
    }
  }


  public SelectInterestLevel(level) {
    this.selectedInterestLevel = level;
  }
}


interface PersonInterested {
  email: string;
  name: string;
  level: InterestLevel;  
}

interface FetchLevelsResult {
  levels: InterestLevel[];
}

interface InterestLevel {
  id: number;
  level: string;
}

interface InterestResult {
  message: string;
}

interface Results {
  events: Events[];
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
  company: Company
  daysUntilEvent: number;
}


interface EventType {
  type: string;
}
interface Company {
  companyName: string;
}
interface Venue {
  name: string;
  location: string;
  capacity: number;
}
