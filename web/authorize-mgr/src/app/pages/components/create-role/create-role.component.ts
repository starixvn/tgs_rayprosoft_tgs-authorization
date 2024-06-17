import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface CreateRoleDialogInput {
    system: string;
}

export interface CreateRoleDialogResult {
    system: string;
    roleName: string;
}

@Component({
    selector: 'app-create-role',
    standalone: true,
    imports: [
        FormsModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        CommonModule
    ],
    templateUrl: './create-role.component.html',
    styleUrl: './create-role.component.scss'
})
export class CreateRoleComponent {
    createRoleForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<CreateRoleComponent, CreateRoleDialogResult>,
        fb: FormBuilder
    ) {
        let data = inject<CreateRoleDialogInput>(MAT_DIALOG_DATA);

        this.createRoleForm = fb.group<CreateRoleDialogResult>({
            system: data.system != null && data.system.length > 0
                ? data.system
                : '',
            roleName: ''
        });
    }

    public cancel(): void {
        this.dialogRef.close();
    }

    public apply(): void {
        this.dialogRef.close(this.createRoleForm.value);
    }
}
