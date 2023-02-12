import { HttpClient } from '@angular/common/http'
import { Component, OnInit } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { QuoteResult, User } from '../calculate-quote/quote-result.model'
import { QuoteService } from '../calculate-quote/quote.service'

@Component({
	selector: 'app-apply-for-loan',
	templateUrl: './apply-for-loan.component.html',
	styleUrls: ['./apply-for-loan.component.css']
})
export class ApplyForLoanComponent implements OnInit {
	quoteResult: QuoteResult
	user: User
	name: string
	mobile: string
	email: string
	dob: string | null
	loanAmount: number = 0
	monthlyPayment: number = 0
	durationInMonths: number = 0
	totalCost: number = 0
	totalInterest: number = 0
	is2MonthsInterestFree: boolean
	interestPerMonth: number = 0
	error = null

	constructor(
		private quoteService: QuoteService,
		private router: Router,
		private route: ActivatedRoute,
		private http: HttpClient
	) {}

	ngOnInit(): void {
		if (Object.keys(this.quoteService.getQuoteResult()).length === 0) {
			this.quoteService
				.fetchUser(this.route.snapshot.params['userId'])
				.subscribe(user => {
					this.http
						.post<QuoteResult>(
							'https://localhost:5001/api/users/calculate-quote',
							{
								userId: user.id,
								loanAmount: user.loanAmount,
								numberOfRepayments: user.numberOfRepayments,
								productId: user.productId
							}
						)
						.subscribe(responseData => {
							this.quoteResult = responseData
							this.user = this.quoteResult.user

							this.setQuoteSummary()
						})
				})
		} else {
			this.quoteResult = this.quoteService.getQuoteResult()
			this.user = this.quoteResult.user
			this.setQuoteSummary()
		}
	}

	setQuoteSummary() {
		this.name = `${this.user.firstName} ${this.user.lastName}`
		this.mobile = this.user.mobileNumber
		this.email = this.user.email
		this.dob = this.user.dateOfBirth
		this.loanAmount = this.user.loanAmount
		this.monthlyPayment = this.quoteResult.monthlyRepaymentWithInterest
		this.durationInMonths = this.quoteResult.numberOfRepayments
		this.totalCost = this.quoteResult.totalCost
		this.totalInterest = this.quoteResult.totalInterest
		this.is2MonthsInterestFree =
			this.quoteResult.product.is2MonthsInterestFree
		this.interestPerMonth = this.quoteResult.interestValuePerMonth
	}

	onEdit() {
		this.router.navigate([`create-request/${this.user.id}`])
	}

	applyNow() {
		this.http
			.post<QuoteResult>('https://localhost:5001/api/users/apply-now', {
				userId: this.user.id,
				loanAmount: this.user.loanAmount,
				numberOfRepayments: this.user.numberOfRepayments,
				productId: this.user.productId
			})
			.subscribe(
				response => {
					this.router.navigate([`success`])
				},
				error => {
					console.log(error)

					this.error = error.error
				}
			)
	}
}
