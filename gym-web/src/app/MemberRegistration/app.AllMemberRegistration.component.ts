
import { Component, OnInit, ViewChild, Input, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MemberRegistrationModel } from './Models/app.memberRegistrationModel';
import { MemberRegistrationService } from './Services/app.MemberRegistration.service';
import { DatePipe } from '@angular/common';
import { MemberRegistrationGridModel } from './Models/app.MemberRegistrationGridModel';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { PaginationService } from '../Shared/PaginationService';

@Component({
    templateUrl: './app.AllMemberRegistration.html',
    styleUrls: ['../Content/vendor/bootstrap/css/bootstrap.min.css',
        '../Content/vendor/metisMenu/metisMenu.min.css',
        '../Content/dist/css/sb-admin-2.css',
        '../Content/vendor/font-awesome/css/font-awesome.min.css',
        './app.memberComponent.css'
    ]
})


export class AllMemberRegistration implements OnInit {
    private _memberregistration;
    data: MemberRegistrationGridModel[]=[];
    errorMessage: any;
    @ViewChild(MatSort) sort: MatSort=new MatSort();
    @ViewChild(MatPaginator) paginator: MatPaginator;

    displayedColumns: string[] = ['MemberId', 'MemberNo', 'MemberName', 'Contactno','PlanName', 'SchemeName', 'JoiningDate', 'EditAction', 'DeleteAction'];
    dataSource = new MatTableDataSource<MemberRegistrationGridModel>();;
    @Input() totalCount: number=0;
    @Output() onPageSwitch = new EventEmitter();

    constructor(private _Route: Router,private memberregistration: MemberRegistrationService, public paginationService: PaginationService) {
        this._memberregistration = memberregistration;
    }

    ngOnInit(): void {
        this._memberregistration.GetAllMember().subscribe(
            allMember => {
                this.data = allMember
                this.dataSource = new MatTableDataSource(this.data);
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            },
            error => this.errorMessage = <any>error
        );
    }

    Delete(MemberId:number=0): void 
    {
        console.log(MemberId);
        if (confirm("Are you sure to delete Member ?")) 
        {
          this._memberregistration.DeleteMember(MemberId).subscribe
            (
            response => {
              if (response.StatusCode == "200") 
              {
                alert('Deleted Member Successfully');
                location.reload();
              }
              else {
                alert('Something Went Wrong');
                this._Route.navigate(['/Member/All']);
              }
            }
            )
        }
      }
      applyFilter(filterValue: string) {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      }
}