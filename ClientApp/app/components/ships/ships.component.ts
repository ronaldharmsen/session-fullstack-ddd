import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router'

@Component({
    selector: 'ships',
    templateUrl: './ships.component.html'
})
export class ShipsComponent {
    apiUrl: string = '';

    constructor(activeRoute: ActivatedRoute) {
        this.apiUrl = activeRoute.snapshot.data.url;
    }
}