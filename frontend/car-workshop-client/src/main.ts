import { bootstrapApplication } from '@angular/platform-browser';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HomeComponent } from './app/pages/home/home.component';

bootstrapApplication(HomeComponent, {
  providers: [provideAnimations()]
})
.catch(err => console.error(err));
