import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NgxIndexedDBService } from 'ngx-indexed-db';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./Content/vendor/bootstrap/css/bootstrap.min.css',
    './Content/vendor/metisMenu/metisMenu.min.css',
    './Content/dist/css/sb-admin-2.css',
    './Content/vendor/font-awesome/css/font-awesome.min.css'
  ]
})
export class AppComponent {
  title = 'gym-web';
  // constructor(private dbService: NgxIndexedDBService){
  // }
}
