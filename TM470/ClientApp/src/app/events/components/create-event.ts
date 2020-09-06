import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest, HttpEventType} from '@angular/common/http';
import $ from 'jquery';
import 'jquery-ui/ui/core';
import 'jquery-ui/ui/widgets/datepicker';
import 'bootstrap';

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

  uploadingImage: File;

  public FileChanged(event) {
    let fileList: FileList = event.target.files;
    if (fileList.length > 0) {
      this.uploadingImage = fileList[0];
    }
  }


  public CreateEvent() {
    $('#CreateEventForm').modal();
    $('.datepicker').datepicker({
      format: "dd/mm/yy"
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
        let formData: CreateEventViewModel = <CreateEventViewModel>{};

        formData.name = this.newEvent.name;
        formData.information = this.newEvent.information;
        formData.venueId = this.newEvent.venueId;
        formData.eventTypeId = this.newEvent.eventTypeId;
        formData.eventCapacity =  this.newEvent.eventCapacity;
        formData.ticketPrice = this.newEvent.ticketPrice;
        formData.eventDate = eventDate;
        formData.eventLiveDate = eventLiveDate;
        console.log(this.uploadingImage);
        formData.imageName = this.uploadingImage.name;

        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        let url = this._baseUrl + 'events' + '/SubmitNewEvent';
        let data = JSON.stringify(formData);
        this._http.post<EventResult>(url, data, { headers: headers })
          .subscribe(result => {
            if (result.message == "Created new Event") {
              console.log(this.uploadingImage);
              if (this.uploadingImage === undefined) {
                window.location.reload();//reload as no images and event created
              }

              const imageData = new FormData();
              imageData.append(this.uploadingImage.name, this.uploadingImage);

              let imageUrl = this._baseUrl + 'api' + '/FileUpload';
              console.log(imageUrl);

              const uploadReq = new HttpRequest('POST', imageUrl, imageData, { reportProgress: true });

              let progress = 0;
              console.log(event);

              this._http.request(uploadReq).subscribe(event => {
                if (event.type === HttpEventType.UploadProgress) {
                  progress = Math.round(100 * event.loaded / event.total);
                  console.log(progress);
                }
              });

            } else {
              this.ErrorMessageShow = true;
            }
          }, error => {
            console.error(error)
          });
        
      }
    }
  }

  public SelectEventType(eventType) {
    this.selectedEventType = eventType;
  }

  public SelectEventVenue(venue) {
    this.selectedEventVenue = venue;
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
  eventDate: string;
  eventLiveDate: string;
  imageName: string;
}

interface EventResult {
  message: string;
}

