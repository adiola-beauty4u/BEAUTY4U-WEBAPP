import { Component, EventEmitter, Input, Output, OnInit, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { TablerIconsModule } from 'angular-tabler-icons';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { AuthService } from 'src/app/services/authentication/auth.service';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-topstrip',
  imports: [MatButtonModule, MatMenuModule, TablerIconsModule, MatIconModule, RouterModule, MatBadgeModule,
    CommonModule,
    MatSidenavModule,
    MatListModule
  ],
  templateUrl: './topstrip.component.html',
  styleUrl: './topstrip.component.scss',
  encapsulation: ViewEncapsulation.None,
})

export class TopstripComponent implements OnInit {

  @Input() showToggle = true;
  @Input() toggleChecked = false;
  @Output() toggleMobileNav = new EventEmitter<void>();

  @Output() menuToggle = new EventEmitter<void>();

  username: string | null;

  constructor(private authService: AuthService) {

  }

  ngOnInit(): void {
    this.username = this.authService.getUserName();
  }

  onMenuToggle() {
    this.menuToggle.emit();
  }

  logout(): void {
    this.authService.logout();
  }
}
