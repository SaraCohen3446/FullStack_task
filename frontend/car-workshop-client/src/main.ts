import { bootstrapApplication } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideHttpClient } from '@angular/common/http';
import { HomeComponent } from './app/pages/home/home.component';

bootstrapApplication(HomeComponent, {
  providers: [
    provideAnimations(),
    provideHttpClient()  
  ]
})
.catch(err => console.error(err));
