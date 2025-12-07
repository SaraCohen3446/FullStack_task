import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { GarageService } from '../../services/garage.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: true,
  imports: [CommonModule, MatTableModule, MatSelectModule, MatButtonModule]
})
export class HomeComponent implements OnInit {
  garages: any[] = [];
  displayedColumns: string[] = ['shemMosah', 'sugMosah', 'ktovet'];

  selectedGarages: any[] = []; 

  constructor(private garageService: GarageService) {}

  ngOnInit(): void {
    this.garageService.getGarages().subscribe({
      next: (data) => {
        this.garages = data;
        console.log('Garages loaded:', this.garages);
      },
      error: (err) => {
        console.error('Error loading garages:', err);
      }
    });
  }
}
