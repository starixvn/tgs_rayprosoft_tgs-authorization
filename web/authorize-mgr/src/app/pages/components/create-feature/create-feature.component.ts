import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { HttpFeatureEditorComponent } from '../http-feature-editor/http-feature-editor.component';

export interface CreateFeatureFormData {
    name: string;
    type: string;
    settings: {};
}

@Component({
    selector: 'app-create-feature',
    standalone: true,
    imports: [
        MatInputModule,
        MatSelectModule,
        MatChipsModule,
        MatIconModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        CommonModule,
        HttpFeatureEditorComponent
    ],
    templateUrl: './create-feature.component.html',
    styleUrl: './create-feature.component.scss'
})
export class CreateFeatureComponent {
    public createFeatureForm: FormGroup;

    constructor(
        fb: FormBuilder
    ) {
        this.createFeatureForm = fb.group<CreateFeatureFormData>({
            name: '',
            type: '',
            settings: {}
        });
    }
}
