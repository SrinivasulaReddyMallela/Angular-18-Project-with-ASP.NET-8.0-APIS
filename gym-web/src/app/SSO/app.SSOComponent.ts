import { Component , OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar,MatSnackBarConfig,MatSnackBarVerticalPosition ,MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { Location } from '@angular/common';
@Component({
    templateUrl: './app.sso.html',
    styleUrls: ['../Content/vendor/bootstrap/css/bootstrap.min.css',
        '../Content/vendor/metisMenu/metisMenu.min.css',
        '../Content/dist/css/sb-admin-2.css',
        '../Content/vendor/font-awesome/css/font-awesome.min.css'
    ]
})
export class SSOComponent implements OnInit
{
    
    
    output: any;
    actionButtonLabel: string = 'Retry';
    action: boolean = false;
    setAutoHide: boolean = true;
    autoHide: number = 2000;
    verticalPosition: MatSnackBarVerticalPosition = 'top';
    public appKey:string="";
    horizontalPosition: MatSnackBarHorizontalPosition = 'center';
    constructor(private _Route:Router,public snackBar: MatSnackBar, private renderer:Renderer2,private location: Location) 
    {
   
    }
    ngOnInit(): void {
        // console.log(this.location.path());
         
        // this._Route.queryParams.subscribe(params => {
        //     this.appKey = params['SAMLResponse'];
        //   });
                debugger;
                var response:any= localStorage.getItem("SSOInfo");
                if(response==null)
                {
                    window.location.href = 'http://desktop-pv0a2lp/IdentityStandaloneMfa/Identity/Account/Login/?AppKey=GymApp';
                }
                else{
                if (response.Token == null && response.Usertype == "0") 
                {
                    let config = new MatSnackBarConfig();
                    config.duration = this.setAutoHide ? this.autoHide : 0;
                    config.verticalPosition = this.verticalPosition;
                    this.snackBar.open("Invalid SSO Reponse", this.action ? this.actionButtonLabel : undefined, config);
                    this._Route.navigate(['SSO']);
                }
    
                if (response.Usertype == "1") 
                {
                    let config = new MatSnackBarConfig();
                    config.duration = this.setAutoHide ? this.autoHide : 0;
                    config.verticalPosition = this.verticalPosition;
                    this.snackBar.open("Logged in Successfully", this.action ? this.actionButtonLabel : undefined, config);
                    this._Route.navigate(['/Admin/Dashboard']);
                }
    
                if (response.Usertype == "2") 
                {
                    let config = new MatSnackBarConfig();
                    config.duration = this.setAutoHide ? this.autoHide : 0;
                    config.verticalPosition = this.verticalPosition;
                    this.snackBar.open("Logged in Successfully", this.action ? this.actionButtonLabel : undefined, config);
                    this._Route.navigate(['/User/Dashboard']);
                }
            }
    }
}