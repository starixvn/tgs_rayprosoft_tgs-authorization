import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { AuthorizeClient } from '../../services/authorize-client';
import { SystemFunction } from 'authorize-client/dist/models/system-function';
import { toArray } from 'rxjs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

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

    }

    public showRenameFunction() {

    }

    public deleteFunction(id: string) {

    }
}
