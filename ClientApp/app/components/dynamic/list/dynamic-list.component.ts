import { Component, Input } from '@angular/core';
import { DynamicService, HALResponse, HALLinkDictionary, HALLinkDetails } from '../services/dynamic.service';

@Component({
    selector: 'dynamic-list',
    templateUrl: './dynamic-list.component.html'
})
export class DynamicListComponent {
    private apiUrl: string;
    halData: HALResponse;
    columnTitles: string[];
    listData: any[];
    commandButtons: any[];
    debugMode: boolean = true;
    messages: string[] = [];

    constructor(private service: DynamicService) {
    }

    get url(): string {
        return this.apiUrl;
    }

    @Input()
    set url(url: string) {
        this.apiUrl = url;
    }

    private updateColumnTitles() {
        //Metadata should be available here
        //No just retrieve column names
        let result: string[] = [];

        if (this.halData && this.halData._embedded) {
            let firstRow = this.halData._embedded.data[0];

            for (var prop in firstRow) {
                if (prop !== '_links')
                    result.push(prop);
            }
        }
        this.columnTitles = result;
    }

    private updateListData() {
        this.listData = this.halData._embedded["data"];
    }

    private updateCommands() {
        let btns: any[] = [];

        for (var lnk in this.halData._links) {
            let current = this.halData._links[lnk];
                
            if (current && current.title) {
                let btn:any=current;
                btns.push( current );
            }
        }

        this.commandButtons = btns;
    }

    getItemCommands(item: any) {
        let btns: any[] = [];

        let links = item._links as HALLinkDictionary;

        for (var lnk in links) {
            let current= links[lnk];
            if (!current.title)
                current.title = 'Details';

            if (current && current.title) {
                //ugly, to avoid circular ref
                let btn: any = {
                    title:   current.title,
                    href:    current.href,
                    method:  current.method,
                    context: item
                };
                
                btns.push( btn );
            }
        } 

        return btns;
    }

    execute (cmd: HALLinkDetails) {
        let context: string = 'list';
        if (cmd.context)
            context = cmd.context.id; //fake it a this point
        
        // actually execute the command in the framework
        let msg:string = `Executing ${cmd.title} with context ${context}`;
        console.log(msg)
        this.messages.push(msg);
    }

    ngOnInit(): void {
        this.service.getList(this.apiUrl).then(data => {
            this.halData = data;
            this.updateColumnTitles();
            this.updateListData();
            this.updateCommands();
            console.log('Done loading');
        });
    }
}

