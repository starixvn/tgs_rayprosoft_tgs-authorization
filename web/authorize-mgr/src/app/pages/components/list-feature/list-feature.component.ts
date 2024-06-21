import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, Input, inject, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { AuthorizeClient } from '../../../services/authorize-client';
import { toArray } from 'rxjs';
import { SystemFeatureListItem } from 'authorize-client/dist/models/system-feature';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-list-feature',
    standalone: true,
    imports: [
        MatDialogModule,
        ReactiveFormsModule,
        CommonModule,
        MatTableModule,
        MatButtonModule
    ],
    templateUrl: './list-feature.component.html',
    styleUrl: './list-feature.component.scss'
})
export class ListFeatureComponent implements AfterViewInit {
    @Input() functionId: string | null = null;
    displayedColumns: string[] = ['name', 'type', 'id'];
    listView: SystemFeatureListItem[] = [];

    constructor(
        private _client: AuthorizeClient
    ) {

    }
    ngAfterViewInit(): void {
        console.log(this.functionId);
        this._client.getFunctionFeatures(this.functionId!)
            .pipe(
                toArray()
            )
            .subscribe(list => {
                this.listView = list;
            });
    }
}
