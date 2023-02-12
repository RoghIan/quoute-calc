import { NgModule } from '@angular/core'
import { MatButtonModule } from '@angular/material/button'
import { MatIconModule } from '@angular/material/icon'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatInputModule } from '@angular/material/input'
import { MatGridListModule } from '@angular/material/grid-list'
import { MatSelectModule } from '@angular/material/select'
import { MatSliderModule } from '@angular/material/slider'
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatNativeDateModule } from '@angular/material/core'
import { MatDividerModule } from '@angular/material/divider'

@NgModule({
	imports: [
		MatButtonModule,
		MatIconModule,
		MatFormFieldModule,
		MatInputModule,
		MatGridListModule,
		MatSelectModule,
		MatSliderModule,
		MatDatepickerModule,
		MatNativeDateModule,
		MatDividerModule
	],
	exports: [
		MatButtonModule,
		MatIconModule,
		MatFormFieldModule,
		MatInputModule,
		MatGridListModule,
		MatSelectModule,
		MatSliderModule,
		MatDatepickerModule,
		MatNativeDateModule,
		MatDividerModule
	]
})
export class MaterialModule {}
