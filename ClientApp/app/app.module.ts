import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';

/* Static components */
import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { ShipsComponent } from './components/ships/ships.component';

/* Stuff necessary for dynamic */
import { COMPILER_PROVIDERS } from '@angular/compiler';
import { DynamicModule } from './components/dynamic/dynamic.module';

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,    
        ShipsComponent    
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: 'ships', component: ShipsComponent, data: { url: 'api/ships'} },
            { path: '**', redirectTo: 'home' }
        ]),
        DynamicModule
    ]
})
export class AppModule {
}
