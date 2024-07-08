import { OnInit, Component } from '@angular/core';
import { Router } from '@angular/router';
import { RenewalService } from './Services/app.renewal.Service';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBar,MatSnackBarConfig,MatSnackBarVerticalPosition ,MatSnackBarHorizontalPosition } from '@angular/material/snack-bar';
import { RequestMemberModel } from './Models/app.RequestMemberModel';
import { ResponseMemberModel } from './Models/app.ResponseMemberModel';
import { RenewalModel } from './Models/app.RenewalModel';
import { SchemeDropdownModel } from '../SchemeMasters/Models/app.SchemeDropdownModel';
import { ActivePlanModel } from '../PlanMaster/Models/app.ActivePlanModel';
import { PlanService } from '../PlanMaster/Services/app.planmaster.service';
import { SchemeService } from '../SchemeMasters/Services/app.Scheme.Service';
import { RequestMemberNoModel } from './Models/app.RequestMemberNoModel';
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './app.renewalcomponent.html',
    styleUrls: ['../Content/vendor/bootstrap/css/bootstrap.min.css',
        '../Content/vendor/metisMenu/metisMenu.min.css',
        '../Content/dist/css/sb-admin-2.css',
        '../Content/vendor/font-awesome/css/font-awesome.min.css'
    ]
})

export class RenewalComponent implements OnInit {

    private _renewalService;
    //private _schemeService;
    private _planService;

    errorMessage: any;
    ResponseMemberModel: ResponseMemberModel[]|any={};
    AllActiveSchemeList: SchemeDropdownModel[]|any={};
    AllActivePlanModel: ActivePlanModel[]|any={};
    displayflag: boolean=false;
    RequestMemberModel: RequestMemberModel |any={};
    RequestMemberNoModel: RequestMemberNoModel |any={};
    RenewalModel: RenewalModel |any={};
    output: any;


    actionButtonLabel: string = 'Retry';
    action: boolean = false;
    setAutoHide: boolean = true;
    autoHide: number = 2000;
    verticalPosition: MatSnackBarVerticalPosition = 'top';
    horizontalPosition: MatSnackBarHorizontalPosition = 'center';

    ngOnInit(): void {
        this.displayflag = false;
    }
    constructor(private _Route: Router, 
                public snackBar: MatSnackBar,
                private renewalService: RenewalService,
                private planService: PlanService,
        ) 
        {
        this._renewalService = renewalService
        this._planService = planService
    }

    onInputChanged(searchStr: string): void {
        this.RequestMemberModel.MemberName = searchStr;
        this.ResponseMemberModel = [];
        this._renewalService.GetMemberNo(this.RequestMemberModel).subscribe((result) => {
            this.ResponseMemberModel = result;
            console.log(result);
        });
    }

    onSubmit(buttonType:any): void {
        if (buttonType === "onsearch") {
            if (this.RenewalModel.SearchMemberNo == undefined) {
                let config = new MatSnackBarConfig();
                config.duration = this.setAutoHide ? this.autoHide : 0;
                config.verticalPosition = this.verticalPosition;
                this.snackBar.open("Enter MemberName", this.action ? this.actionButtonLabel : undefined, config);
            }
            else {
                this.displayflag = true;
                console.log(this.RenewalModel);

                this._renewalService.GetAllActiveSchemeList().subscribe(
                    allActiveScheme => {
                        this.AllActiveSchemeList = allActiveScheme
                    },
                    error => this.errorMessage = <any>error
                );

                this.RequestMemberNoModel.MemberNo = this.RenewalModel.SearchMemberNo;

                this._renewalService.GetRenewalDetailsbyMemberNo(this.RequestMemberNoModel).subscribe(
                    response => {
                        this._renewalService.GetAllActivePlans(parseInt(response.SchemeID)).subscribe(
                            allplanModel => {
                                console.log(allplanModel);
                                this.AllActivePlanModel = allplanModel
                            },
                            error => this.errorMessage = <any>error
                        );
                        this.RenewalModel = response
                        this.RenewalModel.NewDate = "";

                    }
                );

            }


        }

        if (buttonType === "onrenew") {
            if (this.RenewalModel.NewDate == null) {
                let config = new MatSnackBarConfig();
                config.duration = this.setAutoHide ? this.autoHide : 0;
                this.snackBar.open("Choose NewDate", this.action ? this.actionButtonLabel : undefined, config);
            }
            else {

                this._renewalService.SaveRenew(this.RenewalModel).subscribe(
                    response => {
                        if (response.body.StatusCode == "400") {

                            let config = new MatSnackBarConfig();
                            config.duration = this.setAutoHide ? this.autoHide : 0;
                            this.snackBar.open(response.body.ReasonPhrase, this.action ? this.actionButtonLabel : undefined, config);

                        }
                        else if (response.body.StatusCode == "200") {
                            let config = new MatSnackBarConfig();
                            config.duration = this.setAutoHide ? this.autoHide : 0;
                            this.snackBar.open(response.body.ReasonPhrase, this.action ? this.actionButtonLabel : undefined, config);

                        }
                        else {
                            let config = new MatSnackBarConfig();
                            config.duration = this.setAutoHide ? this.autoHide : 0;
                            this.snackBar.open(response.body.ReasonPhrase, this.action ? this.actionButtonLabel : undefined, config);

                        }
                    }
                );
            }


        }

    }

    OnChange(schemeId:any) {

        if (schemeId != null) {
            this._renewalService.GetAllActivePlans(schemeId).subscribe(
                allactivePlans => {
                    this.AllActivePlanModel = allactivePlans
                },
                error => this.errorMessage = <any>error
            );
        }
    }


    GetAmount(PlanID: number, SchemeID: number) {
        if (PlanID != null && SchemeID != null) {
            this._planService.GetAmount(PlanID.toString(), SchemeID.toString()).subscribe(
               amount => {
                    this.RenewalModel.Amount = parseInt(amount)
                },
                error => this.errorMessage = <any>error
            );
        }
    }



}