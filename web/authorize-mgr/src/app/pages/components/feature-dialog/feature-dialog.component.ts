import { AfterViewInit, Component, QueryList, TemplateRef, ViewChild, ViewChildren, inject, viewChild } from '@angular/core';
import { FormTemplateDirective } from '../../directives/form-template.directive';
import { ListFeatureComponent } from '../list-feature/list-feature.component';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

export interface FeatureDialogInput {
    functionId: string;
}

@Component({
    selector: 'app-feature-dialog',
    standalone: true,
    imports: [
        ListFeatureComponent,
        CommonModule,
        MatDialogModule,
        MatButtonModule,
        FormTemplateDirective
    ],
    templateUrl: './feature-dialog.component.html',
    styleUrl: './feature-dialog.component.scss'
})
export class FeatureDialogComponent {
    public functionId: string;
    public viewName: string = 'list';

    constructor(
        public dialogRef: MatDialogRef<FeatureDialogComponent, FeatureDialogInput>,
    ) {
        var data = inject<FeatureDialogInput>(MAT_DIALOG_DATA);

        this.functionId = data.functionId;
    }

    click(){
        this.viewName = 'add';
    }
}
