import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { CreateFunctionComponent, CreateFunctionDialogResult } from '../create-function/create-function.component';
import { AuthorizeClient } from '../../../services/authorize-client';
import { toArray } from 'rxjs';
import { SystemFeatureListItem } from 'authorize-client/dist/models/system-feature';

export interface ListFeatureDialogInput {
    functionId: string;
}

@Component({
    selector: 'app-list-feature',
    standalone: true,
    imports: [
        MatDialogModule,
        ReactiveFormsModule,
        CommonModule,
        MatTableModule
    ],
    templateUrl: './list-feature.component.html',
    styleUrl: './list-feature.component.scss'
})
export class ListFeatureComponent {
    private _functionId: string;
    displayedColumns: string[] = ['name', 'type', 'id'];
    listView: SystemFeatureListItem[] = [];

    constructor(
        public dialogRef: MatDialogRef<CreateFunctionComponent, CreateFunctionDialogResult>,
        private _client: AuthorizeClient
    ) {
        var data = inject<ListFeatureDialogInput>(MAT_DIALOG_DATA);

        this._functionId = data.functionId;

        this._client.getFunctionFeatures(this._functionId)
            .pipe(
                toArray()
            )
            .subscribe(list => {
                this.listView = list;
            });
    }
}
