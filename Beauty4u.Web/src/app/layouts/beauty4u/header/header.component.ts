import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})

export class HeaderComponent {
  darkMode = false;

  ngOnInit(): void {
    const saved = localStorage.getItem('darkMode');
    this.darkMode = saved === 'true';
  }

  toggleDarkMode() {
    this.darkMode = !this.darkMode;
    localStorage.setItem('darkMode', String(this.darkMode));
    document.body.classList.toggle('dark-mode', this.darkMode);
  }
}
