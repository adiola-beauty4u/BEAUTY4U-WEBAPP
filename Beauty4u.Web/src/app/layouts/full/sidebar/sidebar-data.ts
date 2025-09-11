import { NavItem } from './nav-item/nav-item';

export const navItems: NavItem[] = [
  {
    navCap: 'Promotions',
    iconName: 'solar:round-alt-arrow-right-line-duotone',
  },
  {
    displayName: 'Search Product',
    iconName: 'solar:widget-4-line-duotone',
    route: '/promotions/search-products',
    chip: true,
    children: [],
  }, 
  {
    displayName: 'Promotions',
    iconName: 'solar:widget-4-line-duotone',
    route: '/promotions/search-promotions',
    chip: true,
    children: [],
  },  
  {
    navCap: 'Products',
  },
  {
    displayName: 'Bulk Actions',
    iconName: 'solar:widget-4-line-duotone',
    route: 'apps/Bulk',
    //chipClass: 'bg-secondary text-white',
    //chipContent: 'PRO',
    children: [
      {
        displayName: 'New Products',
        subItemIcon: true,
        iconName: 'solar:round-alt-arrow-right-line-duotone',
        route: '/products/bulk-registration',
        chip: true,
        //chipClass: 'bg-secondary text-white',
        //chipContent: 'PRO',
      },
      {
        displayName: 'Update Existing Products',
        subItemIcon: true,
        iconName: 'solar:round-alt-arrow-right-line-duotone',
        route: '/products/bulk-update',
        chip: true,
        //external: true,
        //chipClass: 'bg-secondary text-white',
        //chipContent: 'PRO',
      },
    ],
  },
  {
    displayName: 'Transfer to Stores',
    iconName: 'solar:widget-4-line-duotone',
    route: '/products/transfer',
    chip: true,
    //chipClass: 'bg-secondary text-white',
    //chipContent: 'PRO',
    children: [],
  },
  {
    navCap: 'Jobs',
    route: 'scheduledJobs'
  },
];
