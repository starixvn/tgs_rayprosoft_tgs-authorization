import { Component, computed, signal } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatChipInputEvent, MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { CommonModule } from '@angular/common';

export interface HttpFeatureData {
    methods: string[];
    pathTemplate: string;
}

@Component({
    selector: 'app-http-feature-editor',
    standalone: true,
    imports: [
        MatInputModule,
        MatChipsModule,
        MatIconModule,
        MatAutocompleteModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        FormsModule,
        CommonModule
    ],
    templateUrl: './http-feature-editor.component.html',
    styleUrl: './http-feature-editor.component.scss'
})
export class HttpFeatureEditorComponent {
    public httpFeatureForm: FormGroup;
    public readonly httpMethods = ["GET", "POST", "PUT", "PATCH", "DELETE"];
    public currentMethod = signal('');
    public readonly separatorKeysCodes: number[] = [ENTER, COMMA];
    public filteredMethods = computed(() => {
        let currentInput = this.currentMethod();
        let originMethods = this.httpFeatureForm.controls['methods'].value as string[] ?? [];

        let methodOptions = this.httpMethods.filter(m => originMethods.indexOf(m) < 0);

        return currentInput
            ? methodOptions.filter(m => m.toLowerCase().includes(currentInput.toLowerCase()))
            : methodOptions;
    });

    constructor(
        fb: FormBuilder
    ) {
        this.httpFeatureForm = fb.group<HttpFeatureData>({
            methods: [],
            pathTemplate: ''
        });
    }

    public add(event: MatChipInputEvent) {
        const value = (event.value || '').trim();

        if (value) {
            const originMethods = this.httpFeatureForm.controls['methods'].value as string[] ?? [];

            this.httpFeatureForm.controls['methods'].setValue([...originMethods, value]);

            this.currentMethod.set('');
        }
    }

    public remove(method: string) {
        const originMethods = this.httpFeatureForm.controls['methods'].value as string[];
        const index = originMethods.indexOf(method);

        if (index < 0)
            return;

        originMethods.splice(index, 1);

        this.httpFeatureForm.controls['methods'].setValue([...originMethods]);
    }

    public selected(event: MatAutocompleteSelectedEvent): void {
        const originMethods = this.httpFeatureForm.controls['methods'].value as string[];

        this.httpFeatureForm.controls['methods'].setValue([...originMethods, event.option.value]);

        this.currentMethod.set('');
        event.option.deselect();
    }
}
