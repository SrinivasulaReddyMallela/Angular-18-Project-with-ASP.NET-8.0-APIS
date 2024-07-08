import { NgxIndexedDBService } from 'ngx-indexed-db';

export class IndexedDBService {
  constructor(private dbService: NgxIndexedDBService) {}

  save(obj: any, curstore: string) {
    this.dbService.add(curstore, obj).subscribe(
      () => {
        console.log(curstore);
        console.log(obj);
        console.log('Data saved successfully');
      },
      error => {
        console.error('Error saving data:', error);
      }
    );
  }
}
