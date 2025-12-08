import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { GarageService } from '../../services/garage.service';

@Component({
    selector: 'app-home', // selector for this component
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    standalone: true,
    imports: [CommonModule, FormsModule, MatTableModule, MatSelectModule, MatButtonModule]
})
export class HomeComponent implements OnInit {

    garagesFromGov: any[] = []; // list of garages fetched from gov API
    selectedGarages: any[] = []; // garages selected in multi-select
    savedGarages: any[] = []; // garages already saved in DB
    message: string = ''; // message to show user
    isLoading: boolean = false; // loading indicator for async actions

    // columns to display in table
    displayedColumns: string[] = [
        'misparMosah', 'shemMosah', 'codSugMosah', 'sugMosah',
        'ktovet', 'yishuv', 'telephone', 'mikud', 'codMiktzoa',
        'miktzoa', 'menahelMiktzoa', 'rashamHavarot', 'testime'
    ];

    constructor(private garageService: GarageService) { }

    ngOnInit(): void {
        this.loadSavedGarages(); // load garages already in DB
        this.loadGaragesFromGov(); // load garages from gov API
    }


    // fetch garages from gov API and remove duplicates name
    loadGaragesFromGov() {
        this.isLoading = true;
        this.garageService.getGaragesFromGov().subscribe({
            next: (data) => {
                const unique: { [key: string]: any } = {};
                data.forEach(g => { if (!unique[g.shemMosah]) unique[g.shemMosah] = g; });
                this.garagesFromGov = Object.values(unique);
                this.isLoading = false;
            },
            error: (err) => {
                console.error('Error loading garages from gov:', err);
                this.isLoading = false;
            }
        });
    }

    // fetch garages already saved in DB
    loadSavedGarages(): void {
        this.isLoading = true;
        this.garageService.getSavedGarages().subscribe({
            next: (data) => {
                this.savedGarages = data;
                this.isLoading = false;

                // optionally remove saved garages from multi-select list
                this.garagesFromGov = this.garagesFromGov.filter(g =>
                    !this.savedGarages.some(s => s.misparMosah === g.misparMosah)
                );
            },
            error: (err) => {
                console.error('Error loading saved garages:', err);
                this.isLoading = false;
            }
        });
    }

    // add selected garages to DB
    addSelectedGarages(): void {
        if (!this.selectedGarages.length) {
            this.message = 'No garages selected';
            return;
        }

        // filter out garages that are already saved
        const garagesToSend = this.selectedGarages.filter(g =>
            !this.savedGarages.some(s => s.misparMosah === g.misparMosah)
        );

        if (!garagesToSend.length) {
            this.message = 'selected garages already exist';
            this.selectedGarages = [];
            return;
        }

        this.isLoading = true;
        this.garageService.addGarages(garagesToSend).subscribe({
            next: (res: any) => {
                this.savedGarages.push(...garagesToSend); 
                this.selectedGarages = []; 
                this.message = res?.message || 'Garages added successfully';
                this.isLoading = false;
            },
            error: (err) => {
                console.error('Error adding garages:', err);
                this.message = 'Error adding garages';
                this.isLoading = false;
            }
        });
    }
}
