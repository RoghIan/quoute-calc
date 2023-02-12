import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { HttpClientModule } from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FormsModule } from '@angular/forms'

import { MaterialModule } from './material.module'
import { CalculateQuoteComponent } from './calculate-quote/calculate-quote.component'
import { ApplyForLoanComponent } from './apply-for-loan/apply-for-loan.component'
import { SuccessComponent } from './success/success.component'

@NgModule({
	declarations: [
		AppComponent,
		CalculateQuoteComponent,
		ApplyForLoanComponent,
		SuccessComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		HttpClientModule,
		BrowserAnimationsModule,
		MaterialModule,
		FormsModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule {}
