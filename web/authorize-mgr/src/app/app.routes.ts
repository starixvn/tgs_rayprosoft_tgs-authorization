import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: "",
        loadComponent: () => import('./pages/list-role/list-role.component').then(mod => mod.ListRoleComponent)
    }
];
