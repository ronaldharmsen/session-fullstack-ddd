import { Injectable }    from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class DynamicService {

  private headers = new Headers({'Accept': 'application/hal+json'});

  constructor(private http: Http) { }

  getList(apiUrl: string): Promise<any> {
    return this.http.get(apiUrl, {headers: this.headers})
               .toPromise()
               .then(response => response.json() as HALResponse)
               .catch(this.handleError);
  }

  executeCommand(cmd: HALLinkDetails) {
      return this.http.post(cmd.href, cmd, {headers: this.headers})
               .toPromise()
               .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }
}

export interface HALResponse {
    resourceType: string;
    count: number;
    _links: HALLinkDictionary;
    _embedded: HALEmbeddedCollection;
}

export interface HALLinkDictionary {
    [name:string] : HALLinkDetails
}

export interface HALLinkDetails {
    href: string;
    method: string;
    title: string;
    context: any;
}

export interface HALEmbeddedCollection {
    [name:string] : any
}

