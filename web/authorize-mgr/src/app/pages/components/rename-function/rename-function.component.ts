import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface RenameFunctionDialogInput {
    name: string;
}

export interface RenameFunctionDialogResult {
    name: string;
}

@Component({
    selector: 'app-rename-function',
    standalone: true,
    imports: [
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        CommonModule
    ],
    templateUrl: './rename-function.component.html',
    styleUrl: './rename-function.component.scss'
})
export class RenameFunctionComponent {
    renameFunctionForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<RenameFunctionComponent, RenameFunctionDialogResult>,
        fb: FormBuilder
    ) {
        let data = inject<RenameFunctionDialogInput>(MAT_DIALOG_DATA);

        this.renameFunctionForm = fb.group<RenameFunctionDialogResult>({
            name: data.name
        });
    }

    public cancel(): void {
        this.dialogRef.close();
    }

    public apply(): void {
        this.dialogRef.close(this.renameFunctionForm.value);
    }
}
