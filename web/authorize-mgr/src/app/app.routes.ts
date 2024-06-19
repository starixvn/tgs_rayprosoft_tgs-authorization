import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: "",
        loadComponent: () => import('./pages/list-role/list-role.component').then(mod => mod.ListRoleComponent)
    },
    {
        path: "ListFunction",
        loadComponent: ()=> import('./pages/list-function/list-function.component').then(mod => mod.ListFunctionComponent)
    },
    {
        path: "ListRole",
        loadComponent: () => import('./pages/list-role/list-role.component').then(mod => mod.ListRoleComponent)
    }
];
