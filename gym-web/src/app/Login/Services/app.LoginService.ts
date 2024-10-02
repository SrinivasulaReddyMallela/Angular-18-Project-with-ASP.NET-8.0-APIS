import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs'
import { catchError, tap } from 'rxjs/operators'
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { LoginModel } from '../Models/app.LoginModel';
import { Router } from '@angular/router';
import{environment} from '../../../environments/environment';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { PostSAMLResponseModel } from '../../PostSAMLResponse/Model/app.PostSAMLResponseModel';

@Injectable({
    providedIn: 'root'
})

export class LoginService {
    public token: string="";
    constructor(private _http: HttpClient, private _Route: Router,private dbService: NgxIndexedDBService)
    {

    }
    private apiUrl = environment.apiEndpoint+"/api/Authenticate/";
    private PostSAMLResponseapiUrl = environment.apiEndpoint+"/api/Authenticate/PostSAMLResponse/";
public validatePostSAMLResponse(postSAMLResponseModel:PostSAMLResponseModel)
{
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this._http.post<any>(this.PostSAMLResponseapiUrl, postSAMLResponseModel, { headers: headers })
        .pipe(tap(data =>
        {
           debugger;
            console.log(data);

            if (data.Token != null)
            {
                if (data.Usertype == "2") {
                    localStorage.setItem('AdminUserName',"currentUser");
                    //https://www.npmjs.com/package/ngx-indexed-db
                    //We can store tockens using ngx-indexed-db. Local storage is not suggestable.
                    // store username and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify({ username: postSAMLResponseModel.Username, token: data.Token }));
                    this.dbService
                    .add('Session', {
                        SessionKey: 'currentUser',
                        SessionValue: JSON.stringify({ username: postSAMLResponseModel.Username, token: data.Token }),
                    })
                    .subscribe((key) => {
                      console.log('key: ', key);
                    });
                }
                else if (data.Usertype == "1") {
                    // store username and jwt token in local storage to keep user logged in between page refreshes
                   localStorage.setItem('AdminUserName',"AdminUser");
                    localStorage.setItem('AdminUser', JSON.stringify({ username: postSAMLResponseModel.Username, token: data.Token }));
                    this.dbService
                    .add('Session', {
                        SessionKey: 'AdminUser',
                        SessionValue: JSON.stringify({ username: postSAMLResponseModel.Username, token: data.Token }),
                    })
                    .subscribe((key) => {
                      console.log('key: ', key);
                    });
                }
                return data;
            } else {
                return null;
            }
        }),
            catchError(this.handleError)
        );
}
    public validateLoginUser(loginmodel: LoginModel)
    {
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this._http.post<any>(this.apiUrl, loginmodel, { headers: headers })
            .pipe(tap(data =>
            {
               // debugger;
                console.log(data);

                if (data.Token != null)
                {
                    if (data.Usertype == "2") {
                        localStorage.setItem('AdminUserName',"currentUser");
                        //https://www.npmjs.com/package/ngx-indexed-db
                        //We can store tockens using ngx-indexed-db. Local storage is not suggestable.
                        // store username and jwt token in local storage to keep user logged in between page refreshes
                        localStorage.setItem('currentUser', JSON.stringify({ username: loginmodel.Username, token: data.Token }));
                        this.dbService
                        .add('Session', {
                            SessionKey: 'currentUser',
                            SessionValue: JSON.stringify({ username: loginmodel.Username, token: data.Token }),
                        })
                        .subscribe((key) => {
                          console.log('key: ', key);
                        });
                    }
                    else if (data.Usertype == "1") {
                        // store username and jwt token in local storage to keep user logged in between page refreshes
                       localStorage.setItem('AdminUserName',"AdminUser");
                        localStorage.setItem('AdminUser', JSON.stringify({ username: loginmodel.Username, token: data.Token }));
                        this.dbService
                        .add('Session', {
                            SessionKey: 'AdminUser',
                            SessionValue: JSON.stringify({ username: loginmodel.Username, token: data.Token }),
                        })
                        .subscribe((key) => {
                          console.log('key: ', key);
                        });

                        
//bulk add
//                         this.dbService
//   .bulkAdd('people', [
//     {
//       name: `charles number ${Math.random() * 10}`,
//       email: `email number ${Math.random() * 10}`,
//     },
//     {
//       name: `charles number ${Math.random() * 10}`,
//       email: `email number ${Math.random() * 10}`,
//     },
//   ])
//   .subscribe((result) => {
//     console.log('result: ', result);
//   });


                    }
                    // return true to indicate successful login
                    return data;
                } else {
                    // return false to indicate failed login
                    return null;
                }
            }),
                catchError(this.handleError)
            );
    }

    LogoutUser() {
        localStorage.removeItem('currentUser');
    }

    private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            console.error('An error occurred:', error.error.message);
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
        }
        // return an observable with a user-facing error message
        return throwError('Something bad happened; please try again later.');
    };
}
