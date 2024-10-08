import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { AppRoutingModule } from './app.routes';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppAdminLayoutComponent } from './_layout/app-admin-layout.component';
import { AppUserLayoutComponent } from './_layout/app-userlayout.component';
import { DatePipe } from '@angular/common';
import { BsDatepickerModule, } from 'ngx-bootstrap/datepicker';
import { RouterModule,Routes  } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatAutocomplete, MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSortModule } from '@angular/material/sort';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SchemeComponent } from './SchemeMasters/app.Scheme.Component';
import { AllSchemeComponent } from './SchemeMasters/app.AllScheme.Component';
import { EditSchemeComponent } from './SchemeMasters/app.EditScheme.Component';
import { PlanMasterComponent } from './PlanMaster/app.planmaster.component';
import { AllPlanMasterComponent } from './PlanMaster/app.allplanmaster.component';
import { EditPlanComponent } from './PlanMaster/app.EditPlan.component';
import { RoleComponent } from './RoleMaster/app.Role.component';
import { AllRoleComponent } from './RoleMaster/app.AllRole.component';
import { EditRoleComponent } from './RoleMaster/app.EditRole.component';
import { MemberRegistrationComponent } from './MemberRegistration/app.MemberRegistration.component';
import { MemberListComponent } from './MemberRegistration/List/app.MemberListComponent ';
import { MemberViewComponent } from './MemberRegistration/List/app.MemberViewComponent';
import { EditMemberRegistrationComponent } from './MemberRegistration/app.EditMemberRegistration.component';
import { UserRegistrationComponent } from './CreateUsers/app.UserRegistration.component';
import { AllUserRegistrationComponent } from './CreateUsers/app.AllUserRegistration.component';
import { EditUserRegistrationComponent } from './CreateUsers/app.EditUserRegistration.component';
import { AssignRoleComponent } from './AssignRole/app.AssignRole.component';
import { AllAssignRoleComponent } from './AssignRole/app.AllAssignRole.component';
import { PaymentOverviewComponent } from './Payment/List/app.PaymentOverviewComponent';
import { PaymentListComponent } from './Payment/List/app.PaymentListComponent';
import { RenewalComponent } from './Renewal/app.Renewal.Component';
import { LoginComponent } from './Login/app.LoginComponent';
import { AdminLogoutComponent } from './Login/app.AdminLogout.Component';
import { UserLogoutComponent } from './Login/app.UserLogout.Component';
import { UserDashboardComponent } from './UserDashboard/app.UserDashboardComponent';
import { AdminDashboardComponent } from './AdminDashboard/app.AdminDashboardComponent';
import { MemberDetailsReportComponent } from './Reports/app.MemberDetailsReport.Component';
import { YearwiseReportComponent } from './Reports/app.YearwiseReport.Component';
import { MonthwiseReportComponent } from './Reports/app.MonthwiseReport.Component';
import { RenewalReportComponent } from './Reports/app.RenewalReport.Component';
import { GenerateRecepitComponent } from './Recepit/app.generateRecepit.Component';
import { AdminAuthGuardService } from './AuthGuard/AdminAuthGuardService';
import { UserAuthGuardService } from './AuthGuard/UserAuthGuardService';
import { NgxIndexedDBModule, DBConfig } from 'ngx-indexed-db';
import { SSOComponent } from './SSO/app.SSOComponent';
import { PostSAMLResponseComponent } from './PostSAMLResponse/app.PostSAMLResponseComponent';

  

const dbConfig: DBConfig = {
  name: 'GymWebDB',
  version: 1,
  objectStoresMeta: [
    {
      store: 'Session',
      storeConfig: { keyPath: 'id', autoIncrement: true },
      storeSchema: [
        { name: 'SessionKey', keypath: 'SessionKey', options: { unique: false } },
        { name: 'SessionValue', keypath: 'SessionValue', options: { unique: false } }
      ]
    }
  ]
};



@NgModule({
  declarations: [
    
    AppAdminLayoutComponent,
    AppUserLayoutComponent,
    AllSchemeComponent,
    EditSchemeComponent,
    PlanMasterComponent,
    AllPlanMasterComponent,
    EditPlanComponent,
    RoleComponent,
    AllRoleComponent,
    EditRoleComponent,
    MemberRegistrationComponent,
    MemberListComponent,
    MemberViewComponent,
    EditMemberRegistrationComponent,
    UserRegistrationComponent,
    AllUserRegistrationComponent,
    EditUserRegistrationComponent,
    AssignRoleComponent,
    AllAssignRoleComponent,
    PaymentOverviewComponent,
    PaymentListComponent,
    SchemeComponent,
    RenewalComponent,
    LoginComponent,
    AdminLogoutComponent,
    UserLogoutComponent,
    UserDashboardComponent,
    AdminDashboardComponent,
    MemberDetailsReportComponent,
    YearwiseReportComponent,
    MonthwiseReportComponent,
    RenewalReportComponent,
    GenerateRecepitComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BsDatepickerModule.forRoot(),
    MatTableModule,
    MatAutocompleteModule,
    MatSortModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    NgxIndexedDBModule.forRoot(dbConfig),
    RouterModule.forRoot([
      {
        path: 'Scheme',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Add', component: SchemeComponent , canActivate: [AdminAuthGuardService] },
          { path: 'Edit/:schemeId', component: EditSchemeComponent , canActivate: [AdminAuthGuardService] },
          { path: 'All', component: AllSchemeComponent, canActivate: [AdminAuthGuardService]  }
        ]
      },
      {
        path: 'Plan',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Add', component: PlanMasterComponent , canActivate: [AdminAuthGuardService] },
          { path: 'Edit/:PlanID', component: EditPlanComponent , canActivate: [AdminAuthGuardService] },
          { path: 'All', component: AllPlanMasterComponent , canActivate: [AdminAuthGuardService] }
        ]
      },
      {
        path: 'Role',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Add', component: RoleComponent , canActivate: [AdminAuthGuardService] },
          { path: 'Edit/:RoleID', component: EditRoleComponent , canActivate: [AdminAuthGuardService] },
          { path: 'All', component: AllRoleComponent , canActivate: [AdminAuthGuardService] }
        ]
      },
      {
        path: 'Member',
        component: AppUserLayoutComponent,
        children: [
          { path: 'Add', component: MemberRegistrationComponent ,canActivate: [UserAuthGuardService]},
          { path: 'Edit/:MemberId', component: EditMemberRegistrationComponent ,canActivate: [UserAuthGuardService]},
          { path: 'All', component: MemberViewComponent ,canActivate: [UserAuthGuardService]}
        ]
      },
      {
        path: 'User',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Add', component: UserRegistrationComponent , canActivate: [AdminAuthGuardService] },
          { path: 'Edit/:UserId', component: EditUserRegistrationComponent , canActivate: [AdminAuthGuardService] },
          { path: 'All', component: AllUserRegistrationComponent, canActivate: [AdminAuthGuardService]  }
        ]
      },
      {
        path: 'Assign',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Role', component: AssignRoleComponent , canActivate: [AdminAuthGuardService] },
          { path: 'AllRole', component: AllAssignRoleComponent , canActivate: [AdminAuthGuardService] }
        ]
      },
      {
        path: 'Payment',
        component: AppUserLayoutComponent,
        children: [
          { path: 'Details', component: PaymentOverviewComponent,canActivate: [UserAuthGuardService] }
        ]
      },
      {
        path: 'Renewal',
        component: AppUserLayoutComponent,
        children: [
          { path: 'Renew', component: RenewalComponent ,canActivate: [UserAuthGuardService]  }
        ]
      },

      {
        path: 'Admin',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Dashboard', component: AdminDashboardComponent , canActivate: [AdminAuthGuardService]  }

        ]
      },
      {
        path: 'User',
        component: AppUserLayoutComponent,
        children: [
          { path: 'Dashboard', component: UserDashboardComponent,canActivate: [UserAuthGuardService] },
          { path: 'Recepit/:PaymentID', component: GenerateRecepitComponent,canActivate: [UserAuthGuardService] }
        ]
      },
      {
        path: 'Report',
        component: AppAdminLayoutComponent,
        children: [
          { path: 'Member', component: MemberDetailsReportComponent, canActivate: [AdminAuthGuardService]  },
          { path: 'Yearwise', component: YearwiseReportComponent , canActivate: [AdminAuthGuardService] },
          { path: 'Monthwise', component: MonthwiseReportComponent, canActivate: [AdminAuthGuardService]  },
          { path: 'Renewal', component: RenewalReportComponent, canActivate: [AdminAuthGuardService]  }
          
        ]
      },
      { path: 'Login', component: LoginComponent },
      { path: 'AdminLogout', component: AdminLogoutComponent },
      { path: 'UserLogout', component: UserLogoutComponent },
      { path:"SSO",component:SSOComponent},
     // { path:"PostSAMLResponse",component:PostSAMLResponseComponent},
    //  {
    //   path: 'PostSAMLResponse',
    //   loadChildren: () => import('./PostSAMLResponse/post-saml-response.module').then(m => m.PostSAMLResponseModule)
    // },
      { path: '', redirectTo: "SSO", pathMatch: 'full' },
      //{ path: '**', redirectTo: "PostSAMLResponse", pathMatch: 'full' },
      // { path: '**', redirectTo: "Login", pathMatch: 'full' },
       //{ path: 'SSO', redirectTo: 'http://desktop-pv0a2lp/IdentityStandaloneMfa/Identity/Account/Login/?AppKey=GymApp', pathMatch: 'full' }
    ], { useHash: true })
  ],
  exports: [BsDatepickerModule],
  providers: [DatePipe, AdminAuthGuardService,UserAuthGuardService],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA] 
})
export class AppModule { }


