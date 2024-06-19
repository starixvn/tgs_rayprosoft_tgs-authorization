import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface ChangePriorityInput {
    priority: number;
}

export interface ChangePriorityResult {
    priority: number;
}

@Component({
    selector: 'app-change-role-priority',
    standalone: true,
    imports: [
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        CommonModule
    ],
    templateUrl: './change-role-priority.component.html',
    styleUrl: './change-role-priority.component.scss'
})
export class ChangeRolePriorityComponent {
    changePriorityForm: FormGroup;

    constructor(
        public dialogRef: MatDialogRef<ChangeRolePriorityComponent, ChangePriorityResult>,
        fb: FormBuilder
    ) {
        let data = inject<ChangePriorityInput>(MAT_DIALOG_DATA);

        this.changePriorityForm = fb.group<ChangePriorityResult>({
            priority: data.priority
        });
    }

    public cancel(): void {
        this.dialogRef.close();
    }

    public apply(): void {
        this.dialogRef.close(this.changePriorityForm.value);
    }
}
