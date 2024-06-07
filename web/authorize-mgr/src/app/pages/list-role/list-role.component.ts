import { LayoutModule } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCommonModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';

@Component({
    selector: 'app-list-role',
    standalone: true,
    imports: [
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        MatTableModule,
        MatDividerModule,
        MatIconModule,
        MatCommonModule,
        LayoutModule
    ],
    templateUrl: './list-role.component.html',
    styleUrl: './list-role.component.scss'
})
export class ListRoleComponent {
    displayedColumns: string[] = ['priority', 'name', 'id'];
    queryForm: FormGroup;
    sampleData = [
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
        {
            id: "1",
            name: "Role1",
            priority: 100
        },
        {
            id: "2",
            name: "Role2",
            priority: 100
        },
    ];

    constructor(
        fb: FormBuilder
    ) {
        this.queryForm = fb.group({
            system: ''
        });
    }
}
