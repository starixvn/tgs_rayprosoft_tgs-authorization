import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { AuthorizeClient } from '../../services/authorize-client';
import { SystemFunction } from 'authorize-client/dist/models/system-function';
import { firstValueFrom, toArray } from 'rxjs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CreateFunctionComponent, CreateFunctionDialogInput, CreateFunctionDialogResult } from '../components/create-function/create-function.component';
import { MatDialog } from '@angular/material/dialog';
import { RenameFunctionComponent, RenameFunctionDialogInput, RenameFunctionDialogResult } from '../components/rename-function/rename-function.component';
import { ListFeatureComponent, ListFeatureDialogInput } from '../components/list-feature/list-feature.component';

interface ListFunctionFormData {
    system: string;
}

@Component({
    selector: 'app-list-function',
    standalone: true,
    imports: [
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatDividerModule,
        ReactiveFormsModule,
        CommonModule,
        MatIconModule,
        MatTableModule,
        MatButtonModule
    ],
    templateUrl: './list-function.component.html',
    styleUrl: './list-function.component.scss'
})
export class ListFunctionComponent {
    displayedColumns: string[] = ['name', 'id'];
    queryForm: FormGroup;
    listView: SystemFunction[] = [];
    private readonly _dialog = inject(MatDialog);

    constructor(
        fb: FormBuilder,
        private _client: AuthorizeClient
    ) {
        this.queryForm = fb.group<ListFunctionFormData>({
            system: ''
        });
    }

    public query() {
        let queryFormData: ListFunctionFormData = this.queryForm.value;

        this._client.getFunctionsBySystem(queryFormData.system)
            .pipe(
                toArray()
            )
            .subscribe(functions => this.listView = functions);
    }

    public showCreateFunction() {
        let queryFormData: ListFunctionFormData = this.queryForm.value;

        const dialogRef = this._dialog.open<CreateFunctionComponent, CreateFunctionDialogInput, CreateFunctionDialogResult>(
            CreateFunctionComponent,
            {
                data: {
                    system: queryFormData.system
                }
            });

        dialogRef.afterClosed().subscribe(async result => {
            if (result !== undefined) {
                await firstValueFrom(this._client.createFunction(
                    result.system,
                    result.name));
            }
        });
    }

    public showRenameFunction(functionId: string, name: string) {
        const dialogRef = this._dialog.open<RenameFunctionComponent, RenameFunctionDialogInput, RenameFunctionDialogResult>(
            RenameFunctionComponent,
            {
                data: {
                    name
                }
            });

        dialogRef.afterClosed().subscribe(async result => {
            if (result !== undefined) {
                await firstValueFrom(this._client.renameFunction(functionId, result.name));
            }
        });
    }

    public showEditFeatures(functionId: string) {
        const dialogRef = this._dialog.open<ListFeatureComponent, ListFeatureDialogInput>(
            ListFeatureComponent,
            {
                data: {
                    functionId
                }
            }
        );
    }

    public async deleteFunction(id: string) {
        await firstValueFrom(this._client.deleteFunction(id));

        this.query();
    }
}
