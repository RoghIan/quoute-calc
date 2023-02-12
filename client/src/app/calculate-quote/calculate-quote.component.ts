import { DatePipe } from '@angular/common'
import { Component, OnInit, ViewChild } from '@angular/core'
import { NgForm } from '@angular/forms'
import { ActivatedRoute, Router } from '@angular/router'
import { User } from './quote-result.model'
import { QuoteService } from './quote.service'

@Component({
	selector: 'app-calculate-quote',
	templateUrl: './calculate-quote.component.html',
	styleUrls: ['./calculate-quote.component.css'],
	providers: [DatePipe]
})
export class CalculateQuoteComponent implements OnInit {
	@ViewChild('f', { static: true }) quoteForm!: NgForm
	loanValue = 0
	repaymentDuration = 0
	user: User

	constructor(
		private route: ActivatedRoute,
		private quoteService: QuoteService,
		private datePipe: DatePipe
	) {}

	ngOnInit(): void {
		this.quoteService
			.fetchUser(this.route.snapshot.params['userId'])
			.subscribe(user => {
				this.user = user
				this.loanValue = this.user.loanAmount
				this.repaymentDuration = this.user.numberOfRepayments
				this.quoteForm.setValue({
					product: this.user.productId,
					title: this.user.title.toLowerCase(),
					firstName: this.user.firstName,
					lastName: this.user.lastName,
					email: this.user.email,
					phone: this.user.mobileNumber,
					dateOfBirth: this.user.dateOfBirth
				})
			})
	}

	formatLabel(value: number) {
		if (value >= 1000) {
			return Math.round(value / 1000) + 'k'
		}

		return value
	}

	onSubmit(form: NgForm) {
		console.log(form)

		this.quoteService.calculateQuote({
			quoteRequest: {
				userId: this.user.id,
				loanAmount: this.loanValue,
				numberOfRepayments: this.repaymentDuration,
				productId: form.value.product
			},
			user: {
				id: this.user.id,
				title: this.quoteForm.controls['title'].value,
				firstName: this.quoteForm.controls['firstName'].value,
				lastName: this.quoteForm.controls['lastName'].value,
				email: this.quoteForm.controls['email'].value,
				mobileNumber: this.quoteForm.controls['phone'].value,
				dateOfBirth: this.datePipe.transform(
					this.quoteForm.controls['dateOfBirth'].value,
					'yyyy-MM-dd'
				),
				loanAmount: this.loanValue,
				numberOfRepayments: this.repaymentDuration,
				productId: this.quoteForm.controls['product'].value
			}
		})
	}
}
