import { Component , OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar,MatSnackBarConfig,MatSnackBarVerticalPosition ,MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { Location } from '@angular/common';
import { LoginService } from '../Login/Services/app.LoginService';
import { PostSAMLResponseModel } from './Model/app.PostSAMLResponseModel';
@Component({
    templateUrl: './app.PostSAMLResponse.html',
    styleUrls: ['../Content/vendor/bootstrap/css/bootstrap.min.css',
        '../Content/vendor/metisMenu/metisMenu.min.css',
        '../Content/dist/css/sb-admin-2.css',
        '../Content/vendor/font-awesome/css/font-awesome.min.css'
    ]
})
export class PostSAMLResponseComponent implements OnInit
{
    
    private _loginservice;
    output: any;
    actionButtonLabel: string = 'Retry';
    action: boolean = false;
    setAutoHide: boolean = true;
    autoHide: number = 2000;
    verticalPosition: MatSnackBarVerticalPosition = 'top';
    public appKey:string="";
    horizontalPosition: MatSnackBarHorizontalPosition = 'center';
    constructor(private _Route:ActivatedRoute,public snackBar: MatSnackBar, 
        private renderer:Renderer2,private location: Location,loginservice: LoginService,
        private _Route1:Router,) 
    {
        this._loginservice = loginservice;
    }
    postSAMLResponseModel: PostSAMLResponseModel = new PostSAMLResponseModel();
    ngOnInit(): void {
        debugger;
       // console.log(this.location.path());
        this._Route.queryParams.subscribe(params => {
            const samlResponse = params['SAMLResponse'];
            if (samlResponse) {
              this.handleSamlResponse(samlResponse);
            } else {
              this._Route1.navigate(['/sso']);
            }
          });
           
        }
        handleSamlResponse(samlResponse: string): void {
            this.postSAMLResponseModel.PostSAMLResponse=samlResponse;
            this.postSAMLResponseModel.PostSAMLResponse=" ";
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
