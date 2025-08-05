import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { TablerIconsModule } from 'angular-tabler-icons';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-topstrip',
    imports: [TablerIconsModule, MatButtonModule, MatMenuModule, CommonModule],
    templateUrl: './topstrip.component.html',
})
export class AppTopstripComponent implements OnInit {
    constructor() { }
    versions: string[] = ['v1.0', 'v2.0'];
    selectedVersion = 'v1.0';

    ngOnInit(): void {
        const saved = localStorage.getItem('selectedVersion');
        if (saved && this.versions.includes(saved)) {
            this.selectedVersion = saved;
        }
    }

    selectVersion(version: string): void {
        this.selectedVersion = version;
        localStorage.setItem('selectedVersion', version);
    }
}
