import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface CreateFunctionDialogInput {
    system: string;
}

export interface CreateFunctionDialogResult {
    system: string;
    name: string;
}

@Component({
    selector: 'app-create-function',
    standalone: true,
    imports: [
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        CommonModule,
        MatInputModule,
        MatButtonModule
    ],
    templateUrl: './create-function.component.html',
    styleUrl: './create-function.component.scss'
})
export class CreateFunctionComponent {
    createFunctionForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<CreateFunctionComponent, CreateFunctionDialogResult>,
        fb: FormBuilder
    ) {
        let data = inject<CreateFunctionDialogInput>(MAT_DIALOG_DATA);

        this.createFunctionForm = fb.group<CreateFunctionDialogResult>({
            system: data.system,
            name: ''
        });
    }

    public cancel(): void {
        this.dialogRef.close();
    }

    public apply(): void {
        this.dialogRef.close(this.createFunctionForm.value);
    }
}
