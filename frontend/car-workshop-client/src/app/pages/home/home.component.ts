import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { GarageService } from '../../services/garage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, MatTableModule, MatSelectModule, MatButtonModule]
})
export class HomeComponent implements OnInit {
  garagesFromGov: any[] = [];
  selectedGarages: any[] = [];
  savedGarages: any[] = [];
  message: string = '';

  displayedColumns: string[] = [
    'misparMosah', 'shemMosah', 'codSugMosah', 'sugMosah',
    'ktovet', 'yishuv', 'telephone', 'mikud', 'codMiktzoa',
    'miktzoa', 'menahelMiktzoa', 'rashamHavarot', 'testime'
  ];

  constructor(private garageService: GarageService) {}

  ngOnInit(): void {
    this.garageService.getGaragesFromGov().subscribe({
      next: (data) => this.garagesFromGov = data,
      error: (err) => console.error('Error loading garages from gov:', err)
    });

    this.loadSavedGarages();
  }

  loadSavedGarages() {
    this.garageService.getSavedGarages().subscribe({
      next: (data) => this.savedGarages = data,
      error: (err) => console.error('Error loading saved garages:', err)
    });
  }

  addSelectedGarages() {
    if (!this.selectedGarages.length) {
      this.message = 'No garages selected';
      return;
    }

    const garagesToSend = this.selectedGarages.filter(g =>
      !this.savedGarages.some(s => s.misparMosah === g.misparMosah)
    );

    if (!garagesToSend.length) {
      this.message = 'All selected garages already exist';
      this.selectedGarages = [];
      return;
    }

    this.garageService.addGarages(garagesToSend).subscribe({
      next: (res: any) => {
        this.message = res?.message || 'Garages added successfully';
        this.savedGarages.push(...garagesToSend);
        this.selectedGarages = [];
      },
      error: (err) => {
        console.error('Error adding garages:', err);
        this.message = 'Error adding garages';
      }
    });
  }
}
