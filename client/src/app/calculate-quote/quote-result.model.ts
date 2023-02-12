export interface QuoteRequest {
	userId: number
	loanAmount: number
	numberOfRepayments: number
	productId: number
}

export interface UserQuote {
	user: User
	quoteRequest: QuoteRequest
}

export interface User {
	id: number
	title: string
	firstName: string
	lastName: string
	dateOfBirth: string | null
	email: string
	mobileNumber: string
	loanAmount: number
	numberOfRepayments: number
	productId: number
}

export interface Product {
	productName: string
	annualInterestRate: number
	is2MonthsInterestFree: boolean
}

export interface QuoteResult {
	numberOfRepayments: number
	monthlyRepaymentNoInterest: number
	monthlyRepaymentWithInterest: number
	interestValuePerMonth: number
	totalCost: number
	totalInterest: number
	user: User
	product: Product
}
