import { Component , OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar,MatSnackBarConfig,MatSnackBarVerticalPosition ,MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
 
import { LoginService } from '../Login/Services/app.LoginService';
import { PostSAMLResponseModel } from './Model/app.PostSAMLResponseModel';
@Component({
    selector: 'app-PostSAMLResponseComponent',
    //templateUrl: './app.PostSAMLResponse.html',
    // styleUrls: ['../Content/vendor/bootstrap/css/bootstrap.min.css',
    //     '../Content/vendor/metisMenu/metisMenu.min.css',
    //     '../Content/dist/css/sb-admin-2.css',
    //     '../Content/vendor/font-awesome/css/font-awesome.min.css'
    // ],
    template: '<button (click)="navigate()">Navigate</button>'
})
export class PostSAMLResponseComponent implements OnInit
{
    postSAMLResponseModel: PostSAMLResponseModel = new PostSAMLResponseModel();
    private _loginservice;
    output: any;
    actionButtonLabel: string = 'Retry';
    action: boolean = false;
    setAutoHide: boolean = true;
    autoHide: number = 2000;
    verticalPosition: MatSnackBarVerticalPosition = 'top';
    public appKey:string="";
    horizontalPosition: MatSnackBarHorizontalPosition = 'center';
    receivedData: any=[];
    constructor(private _Route:ActivatedRoute,public snackBar: MatSnackBar, 
        loginservice: LoginService,
        private _Route1:Router,) 
    {
        this._loginservice = loginservice;
    }
    navigate() {
        let token = this.getUrlParameter('SAMLResponse');
        this._Route1.navigate(['/SSO'], {
            queryParams: { SAMLResponse: token }
          });
      }
      ngDoCheck() {debugger; }
      ngAfterContentInit() {debugger;}
      ngAfterContentChecked() { debugger;}
      ngAfterViewInit() {  debugger;}
      ngAfterViewChecked() { debugger;}
      ngOnDestroy() {debugger; }



    ngOnInit(): void {
        debugger;
        let token = this.getUrlParameter('SAMLResponse');
        this._Route1.navigate(['/SSO'], {
            queryParams: { SAMLResponse: token }
          });
        
        localStorage.setItem("samlResponsetoken",token);
        console.log(token);
       // console.log(this.location.path());
        this._Route.queryParams.subscribe(params => {
            const samlResponse = params['SAMLResponse'];
            localStorage.setItem("samlResponse",samlResponse);
            if (samlResponse) {
              this.handleSamlResponse(samlResponse);
            } else {
              this._Route1.navigate(['/login']);
            }
          });
           
        }
        private getUrlParameter(name: string): string {
            const url = window.location.href;
            const param = url.split('?')[1].split('&').find((p) => p.startsWith(name + '='));
            return param ? param.split('=')[1] : '';
          }
        getParameterByName(name: any) {
            let url = window.location.href;
            name = name.replace(/[[]]/g, "\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2]);
            }
        handleSamlResponse(samlResponse: string): void {
            this.postSAMLResponseModel.PostSAMLResponse=samlResponse;
            this.postSAMLResponseModel.Username=" ";
            this._loginservice.validatePostSAMLResponse(this.postSAMLResponseModel).subscribe(
            response => 
            {     
                localStorage.setItem("SSOInfo",response);
                if (response.Token == null && response.Usertype == "0") 
                {
                    let config = new MatSnackBarConfig();
                    config.duration = this.setAutoHide ? this.autoHide : 0;
                    config.verticalPosition = this.verticalPosition;
                    this.snackBar.open("Invalid SSO Reponse", this.action ? this.actionButtonLabel : undefined, config);
                    this._Route1.navigate(['SSO']);
                }
    
                if (response.Usertype == "1") 
                {
                    let config = new MatSnackBarConfig();
                    config.duration = this.setAutoHide ? this.autoHide : 0;
                    config.verticalPosition = this.verticalPosition;
                    this.snackBar.open("Logged in Successfully", this.action ? this.actionButtonLabel : undefined, config);
                    this._Route1.navigate(['/Admin/Dashboard']);
                }
    
                if (response.Usertype == "2") 
                {
                    let config = new MatSnackBarConfig();
                    config.duration = this.setAutoHide ? this.autoHide : 0;
                    config.verticalPosition = this.verticalPosition;
                    this.snackBar.open("Logged in Successfully", this.action ? this.actionButtonLabel : undefined, config);
                    this._Route1.navigate(['/User/Dashboard']);
                }
            });
        }
    }
