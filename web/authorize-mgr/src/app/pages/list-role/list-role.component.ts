import { LayoutModule } from '@angular/cdk/layout';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCommonModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { AuthorizeClient, Role } from '../../services/authorize-client';
import { firstValueFrom, toArray } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { CreateRoleComponent, CreateRoleDialogInput, CreateRoleDialogResult } from '../components/create-role/create-role.component';

interface ListRoleFormData {
    system: string;
}

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
    listView: Role[] = [];
    private readonly _dialog = inject(MatDialog);

    constructor(
        fb: FormBuilder,
        private _client: AuthorizeClient
    ) {
        this.queryForm = fb.group({
            system: ''
        });
    }

    public query() {
        let queryFormData: ListRoleFormData = this.queryForm.value;

        this._client.getRolesBySystem(queryFormData.system)
            .pipe(
                toArray()
            )
            .subscribe(roles => this.listView = roles);
    }

    public showCreateRole() {
        let queryFormData: ListRoleFormData = this.queryForm.value;

        const dialogRef = this._dialog.open<CreateRoleComponent, CreateRoleDialogInput, CreateRoleDialogResult>(
            CreateRoleComponent,
            {
                data: {
                    system: queryFormData.system
                }
            });

        dialogRef.afterClosed().subscribe(async result => {
            if (result !== undefined) {
                await firstValueFrom(this._client.createRole(
                    result.system,
                    {
                        name: result.roleName
                    }));
            }
        })
    }
}
