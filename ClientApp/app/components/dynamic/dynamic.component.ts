import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute } from '@angular/router'

@Component({
    selector: 'dynamic-form',
    templateUrl: './dynamic.component.html'
})
export class DynamicComponent {
    apiUrl: string = '';

    constructor(activeRoute: ActivatedRoute) {
        this.apiUrl = activeRoute.snapshot.data.url;
    }
}