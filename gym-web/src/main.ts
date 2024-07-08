import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
  
bootstrapApplication(AppModule, appConfig)
  .catch((err) => console.error(err));
// Bootstrap the application using the bootstrapApplication function
// bootstrapApplication(AppModule, () => {
//   // Optional callback after bootstrapping
// });