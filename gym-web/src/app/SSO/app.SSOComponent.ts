import { Component , OnInit, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MatSnackBar,MatSnackBarConfig,MatSnackBarVerticalPosition ,MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { Location } from '@angular/common';
import { LoginService } from '../Login/Services/app.LoginService';
import { PostSAMLResponseModel } from '../PostSAMLResponse/Model/app.PostSAMLResponseModel';
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
    
    private _loginservice;
    output: any;
    actionButtonLabel: string = 'Retry';
    action: boolean = false;
    setAutoHide: boolean = true;
    autoHide: number = 2000;
    verticalPosition: MatSnackBarVerticalPosition = 'top';
    public appKey:string="";
    horizontalPosition: MatSnackBarHorizontalPosition = 'center';
    postSAMLResponseModel: PostSAMLResponseModel = new PostSAMLResponseModel();
    constructor(private _Route:Router,public snackBar: MatSnackBar,
         private renderer:Renderer2,loginservice: LoginService,private location: Location) 
    {
        this._loginservice = loginservice;
    }
    
    ngOnInit(): void {
         
        // this._Route.queryParams.subscribe(params => {
        //     this.appKey = params['SAMLResponse'];
        //   });
                debugger;
                var saml ="PFJlc3BvbnNlIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiIHhtbG5zOnhzZD0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEiIElEPSJfMDRmYWU4ZGQtZjllNS00ZDM0LWI2YTktNGQ1NjUzM2E4ODI1IiBWZXJzaW9uPSIyLjAiIElzc3VlSW5zdGFudD0iMjAyNC0xMC0wMlQxNTowNjo0My45NTA1NzYyWiIgRGVzdGluYXRpb249Imh0dHA6Ly9kZXNrdG9wLXB2MGEybHAvR1lNV2ViLyIgeG1sbnM9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDpwcm90b2NvbCI+PElzc3VlciB4bWxucz0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmFzc2VydGlvbiI+aHR0cDovL2Rlc2t0b3AtcHYwYTJscC9JZGVudGl0eVN0YW5kYWxvbmVNZmEvPC9Jc3N1ZXI+PFNpZ25hdHVyZSB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyI+PFNpZ25lZEluZm8+PENhbm9uaWNhbGl6YXRpb25NZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAxL1JFQy14bWwtYzE0bi0yMDAxMDMxNSIgLz48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxkc2lnLW1vcmUjcnNhLXNoYTI1NiIgLz48UmVmZXJlbmNlIFVSST0iI18wNGZhZThkZC1mOWU1LTRkMzQtYjZhOS00ZDU2NTMzYTg4MjUiPjxUcmFuc2Zvcm1zPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSIgLz48VHJhbnNmb3JtIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8xMC94bWwtZXhjLWMxNG4jIiAvPjwvVHJhbnNmb3Jtcz48RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2IiAvPjxEaWdlc3RWYWx1ZT5rM3llc1JnYlJuMStyM2FtYXFRNjFoSlJJazhsTEUyRU1rVWMwWjdzTG1jPTwvRGlnZXN0VmFsdWU+PC9SZWZlcmVuY2U+PC9TaWduZWRJbmZvPjxTaWduYXR1cmVWYWx1ZT5jVytBUFFQaFNwQW0rOGJBQjQ4Skg4ZXZNaUFSRnJYd2hLRkZaOUVoM3lDVkJBeldWYVI5dFRJV04zbzJ4bFdPMCt1THplaXpXVFlsUTQyYiswRGdISStnV0ZwVUJod3hGYzM4SGVsK0lSUi9aMUxtc21uUWg5aFlGSmYwYzRyTUJWSDdFUVlpaS9KU1RVSkZVV3B4RmU3VmVJQ1ZHNmoxMjFReER4T1ZSbDJNL0ZPSW54d051YVJIa01YZEN0SmZQcmxYclY2M0FvanVlVDU0eS9FYXRNWEZJNzdNeDZ5QWVJSGdWTDYwQ0RMVUVORnVDR1EvajkrTzdIejVOWU56THlNSi9oZTZ4MHhEcGtNeUxhWlg0U1MySlhlTmJmK0J5bjJRalZVdGFYMWM2N1lQU3dsVU5IMkNnOGRDVUc4RjljMlg0NUJucy94TFBZZkJIanloT3c9PTwvU2lnbmF0dXJlVmFsdWU+PEtleUluZm8+PFg1MDlEYXRhPjxYNTA5Q2VydGlmaWNhdGU+TUlJQzdqQ0NBZGFnQXdJQkFnSVFGTUpVckdhNTNyVkVwU2NwVjBtNFZUQU5CZ2txaGtpRzl3MEJBUVVGQURBek1SZ3dGZ1lEVlFRREV3OWtaWE5yZEc5d0xYQjJNR0V5YkhBeEZ6QVZCZ05WQkFNVERqRTVNaTR4TmpndU1qa3VNVEV6TUI0WERUSTBNRGt5TlRBeE1UYzBPRm9YRFRJMk1Ea3lOVEF4TVRjME9Gb3dNekVZTUJZR0ExVUVBeE1QWkdWemEzUnZjQzF3ZGpCaE1teHdNUmN3RlFZRFZRUURFdzR4T1RJdU1UWTRMakk1TGpFeE16Q0NBU0l3RFFZSktvWklodmNOQVFFQkJRQURnZ0VQQURDQ0FRb0NnZ0VCQUw2NVh1R1RvMjNlTDFvRlZLVEpia1gwQkt2YWhtbVNHVUhVSTR0dkNNNGVoSm11NVF1YnhVK1RuUDlXblRYbGtRV0lBeTU2OEU1ZkZkcTExK0ZydXB1K2NLTUlKZXprV2hDTFpEVzNmbDAvcjBTOFV3Q25BZzRoN0syK0k1MExLSDJCTllWMmlVbUFkRnJVakxFUktxK29qMXRNbmszWEp4S1Jpd2dLbTJSUm9YSVJpRXY1RERHVDYza0tVbnFOOUNQOWhBZlRSWUZMdVNjbE1OQVdnbTFGWERwTmg2emF4TTZQOWsxTTJ2dzJLK2xEZlZFZTZyaWlCakkvb1N1YnBiWjYreVljSXIrNHBMMktCcWorQmROYkI2TjY4YTNnaW1MSUYzL290N2pxVTBzZSs2YXdpbTNjZFA5UkV0aDhncG11RFk2Yjd2QTdPemRBd1M0WHkwa0NBd0VBQVRBTkJna3Foa2lHOXcwQkFRVUZBQU9DQVFFQW0wQ2JLeWpSZGJoMzltNDlJTlB2L0FnM3RFdGN2L2pnYnNiRElHMndkUTgzVk5MSkg3QTVlYklJbXcrVHJBdVFuOG5KYk9Vd1hRbFZHQnBxNllmVVNRbFhBVFNYRVFBcG8yWkNYaE55MXV0cVFQZVV3b0VDYmFmbCtNenByR0xVVmJPZUFLc0NrUHhrUVRrYThZVk5vdEJEakJ1K2NGbWs4MjR0QXJveE4vaTN0Y3N2anJ1L2ROOU91UlJ0UlJZNGNscVMreC9ETkgzWUprSDRhYzFubmU3am9IcFBSK0xPNTZUSmxXdVdSa2tlNmUvWGpiNEVxcndXU2l0YW1ZazNGR2NsaFpmQm9PV3hHUFJKUm9aVkZjMHp1dkxTY0hrZ3ltZVRRa05XTm1GWHVFWjBvZHF0M0w4blNVd2h2VHdnUWNDbDZWZUVDVWE0TkRRTmxFSlJYQT09PC9YNTA5Q2VydGlmaWNhdGU+PC9YNTA5RGF0YT48L0tleUluZm8+PC9TaWduYXR1cmU+PFN0YXR1cz48U3RhdHVzQ29kZSBWYWx1ZT0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOnN0YXR1czpTdWNjZXNzIiAvPjwvU3RhdHVzPjxBc3NlcnRpb24gVmVyc2lvbj0iMi4wIiBJRD0iXzU2ZThmNjM5LTI1ODItNGYyNC1iYmFlLWVlYTNmY2M2MzBhNyIgSXNzdWVJbnN0YW50PSIyMDI0LTEwLTAyVDE1OjA2OjQzLjk1MDcyMjdaIiB4bWxucz0idXJuOm9hc2lzOm5hbWVzOnRjOlNBTUw6Mi4wOmFzc2VydGlvbiI+PElzc3Vlcj5odHRwOi8vZGVza3RvcC1wdjBhMmxwL0lkZW50aXR5U3RhbmRhbG9uZU1mYS88L0lzc3Vlcj48U3ViamVjdD48TmFtZUlEIE5hbWVRdWFsaWZpZXI9ImRlc2t0b3AtcHYwYTJscCI+bG9jYWx1c2VyaWQ8L05hbWVJRD48U3ViamVjdENvbmZpcm1hdGlvbiBNZXRob2Q9InVybjpvYXNpczpuYW1lczp0YzpTQU1MOjIuMDpjbTpiZWFyZXIiPjxTdWJqZWN0Q29uZmlybWF0aW9uRGF0YSBOb3RPbk9yQWZ0ZXI9IjIwMjQtMTAtMDJUMTU6MTE6NDMuOTUxNTAwMloiIFJlY2lwaWVudD0iaHR0cDovL2Rlc2t0b3AtcHYwYTJscC9HWU1XZWIvIiAvPjwvU3ViamVjdENvbmZpcm1hdGlvbj48L1N1YmplY3Q+PENvbmRpdGlvbnMgTm90QmVmb3JlPSIyMDI0LTEwLTAyVDE1OjA2OjQzLjk1MDcyMzVaIiBOb3RPbk9yQWZ0ZXI9IjIwMjQtMTAtMDJUMTU6MTE6NDMuOTUwNzIzOFoiPjxBdWRpZW5jZVJlc3RyaWN0aW9uPjxBdWRpZW5jZT5kZXNrdG9wLXB2MGEybHA8L0F1ZGllbmNlPjwvQXVkaWVuY2VSZXN0cmljdGlvbj48L0NvbmRpdGlvbnM+PEF1dGhuU3RhdGVtZW50IEF1dGhuSW5zdGFudD0iMjAyNC0xMC0wMlQxNTowNjo0My45NTA3MjcxWiI+PEF1dGhuQ29udGV4dD48QXV0aG5Db250ZXh0Q2xhc3NSZWY+QXV0aG5Db250ZXh0Q2xhc3NSZWY8L0F1dGhuQ29udGV4dENsYXNzUmVmPjwvQXV0aG5Db250ZXh0PjwvQXV0aG5TdGF0ZW1lbnQ+PEF0dHJpYnV0ZVN0YXRlbWVudD48QXR0cmlidXRlIE5hbWU9IkVtYWlsIiBOYW1lRm9ybWF0PSJ1cm46b2FzaXM6bmFtZXM6dGM6U0FNTDoyLjA6YXR0cm5hbWUtZm9ybWF0OmJhc2ljIj48QXR0cmlidXRlVmFsdWUgeHNpOnR5cGU9InhzZDpzdHJpbmciPlNyaW5pdmFzdWxhcmVkZHkuaW5AZ21haWwuY29tPC9BdHRyaWJ1dGVWYWx1ZT48L0F0dHJpYnV0ZT48L0F0dHJpYnV0ZVN0YXRlbWVudD48L0Fzc2VydGlvbj48L1Jlc3BvbnNlPg==";
                this.postSAMLResponseModel.PostSAMLResponse=saml;
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
                });
                // var response:any= localStorage.getItem("SSOInfo");
                // if(response==null)
                // {
                //     window.location.href = 'http://desktop-pv0a2lp/IdentityStandaloneMfa/Identity/Account/Login/?AppKey=GymApp';
                // }
                // else
                // {
                //     return;
                //     if (response.Token == null && response.Usertype == "0") 
                //     {
                //         let config = new MatSnackBarConfig();
                //         config.duration = this.setAutoHide ? this.autoHide : 0;
                //         config.verticalPosition = this.verticalPosition;
                //         this.snackBar.open("Invalid SSO Reponse", this.action ? this.actionButtonLabel : undefined, config);
                //         this._Route.navigate(['SSO']);
                //     }
        
                //     if (response.Usertype == "1") 
                //     {
                //         let config = new MatSnackBarConfig();
                //         config.duration = this.setAutoHide ? this.autoHide : 0;
                //         config.verticalPosition = this.verticalPosition;
                //         this.snackBar.open("Logged in Successfully", this.action ? this.actionButtonLabel : undefined, config);
                //         this._Route.navigate(['/Admin/Dashboard']);
                //     }
        
                //     if (response.Usertype == "2") 
                //     {
                //         let config = new MatSnackBarConfig();
                //         config.duration = this.setAutoHide ? this.autoHide : 0;
                //         config.verticalPosition = this.verticalPosition;
                //         this.snackBar.open("Logged in Successfully", this.action ? this.actionButtonLabel : undefined, config);
                //         this._Route.navigate(['/User/Dashboard']);
                //     }
                // }
    }
}