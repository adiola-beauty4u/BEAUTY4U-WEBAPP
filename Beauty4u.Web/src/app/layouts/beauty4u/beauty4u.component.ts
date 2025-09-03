import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TopstripComponent } from './topstrip/topstrip.component';
import { MatSidenav, MatSidenavContent, MatSidenavModule } from '@angular/material/sidenav';
import { CoreService } from 'src/app/services/core.service';
import { BreakpointObserver } from '@angular/cdk/layout';
import { filter, Subscription } from 'rxjs';
import { SidebarComponent } from '../full/sidebar/sidebar.component';
import { AppNavItemComponent } from '../full/sidebar/nav-item/nav-item.component';
import { navItems } from '../full/sidebar/sidebar-data';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';

const MOBILE_VIEW = 'screen and (max-width: 768px)';
const TABLET_VIEW = 'screen and (min-width: 769px) and (max-width: 1024px)';

@Component({
  selector: 'app-beauty4u',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    // Angular Material (standalone-friendly)
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,
    // Your components
    TopstripComponent,
    SidebarComponent,
    AppNavItemComponent,
    // 3rd-party
    NgScrollbarModule,
  ],
  templateUrl: './beauty4u.component.html',
  styleUrl: './beauty4u.component.scss',
})
export class Beauty4uComponent implements OnInit, OnDestroy {
  darkMode = false;

  @ViewChild('leftsidenav', { static: false }) sidenav!: MatSidenav;
  @ViewChild('content', { static: true }) content!: MatSidenavContent;

  resView = false;
  navItems = navItems;

  options = this.settings.getOptions();
  private layoutChangesSubscription = Subscription.EMPTY;
  private isMobileScreen = false;
  private isContentWidthFixed = true;
  private isCollapsedWidthFixed = false;
  private htmlElement!: HTMLHtmlElement;

  constructor(
    private settings: CoreService,
    private router: Router,
    private breakpointObserver: BreakpointObserver,
  ) {
    this.htmlElement = document.querySelector('html')!;

    this.layoutChangesSubscription = this.breakpointObserver
      .observe([MOBILE_VIEW, TABLET_VIEW])
      .subscribe((state) => {
        // Reset open state on layout change
        this.options.sidenavOpened = true;
        this.isMobileScreen = state.breakpoints[MOBILE_VIEW];

        if (this.options.sidenavCollapsed === false) {
          this.options.sidenavCollapsed = state.breakpoints[TABLET_VIEW];
        }
      });

    // Scroll to top on route change (if you want)
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        // this.content.scrollTo({ top: 0 });
      });
  }

  /** Use breakpoint result for over/side mode */
  get isOver(): boolean {
    return this.isMobileScreen;
  }

  ngOnInit(): void {
    const saved = localStorage.getItem('darkMode');
    this.darkMode = saved === 'true';
  }

  toggleDarkMode() {
    this.darkMode = !this.darkMode;
    localStorage.setItem('darkMode', String(this.darkMode));
  }

  ngOnDestroy() {
    this.layoutChangesSubscription.unsubscribe();
  }

  toggleCollapsed() {
    this.isContentWidthFixed = false;
    this.options.sidenavCollapsed = !this.options.sidenavCollapsed;
    this.resetCollapsedState();
  }

  resetCollapsedState(timer = 400) {
    setTimeout(() => this.settings.setOptions(this.options), timer);
  }

  onSidenavClosedStart() {
    this.isContentWidthFixed = false;
  }

  onSidenavOpenedChange(isOpened: boolean) {
    this.isCollapsedWidthFixed = !this.isOver;
    this.options.sidenavOpened = isOpened;
  }
}
