import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { ApplyForLoanComponent } from './apply-for-loan/apply-for-loan.component'
import { CalculateQuoteComponent } from './calculate-quote/calculate-quote.component'
import { SuccessComponent } from './success/success.component'

const routes: Routes = [
	{ path: 'create-request/:userId', component: CalculateQuoteComponent },
	{ path: 'apply-loan/:userId', component: ApplyForLoanComponent },
	{ path: 'success', component: SuccessComponent }
]

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule {}
