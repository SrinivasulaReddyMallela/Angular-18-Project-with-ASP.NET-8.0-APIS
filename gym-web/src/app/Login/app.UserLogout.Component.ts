import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { NgxIndexedDBService } from 'ngx-indexed-db';
@Component({
    template:''
})
export class UserLogoutComponent implements OnInit
{
    constructor(private _Route: Router,private dbService: NgxIndexedDBService)
    {

    }

    ngOnInit()
    {

        this.dbService.getAll('Session').subscribe((Sessions) => {
            Sessions.forEach((currentValue:any, index) => {
                if (currentValue.SessionKey!=null) {
                    if(currentValue.SessionKey==localStorage.getItem('AdminUserName'))
                       {
                        this.dbService.deleteByKey('Session', currentValue.id).subscribe((status) => {
                            console.log('Deleted?:', status);
                          });
                       }
                }
              });
          });
        localStorage.removeItem('currentUser');
        this._Route.navigate(['Login']);
    }
}
