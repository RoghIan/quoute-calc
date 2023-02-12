import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Router } from '@angular/router'
import { QuoteRequest, UserQuote } from './quote-result.model'
import { Product, QuoteResult, User } from './quote-result.model'

@Injectable({ providedIn: 'root' })
export class QuoteService {
	private quoteResult: QuoteResult
	private userQuote: UserQuote
	private user: User

	constructor(private http: HttpClient, private router: Router) {}

	fetchUser(id: number) {
		return this.http.get<User>(`https://localhost:5001/api/users/${id}`)
	}

	calculateQuote(quoteRequest: UserQuote) {
		this.userQuote = quoteRequest
		this.http
			.post<QuoteResult>(
				'https://localhost:5001/api/users/calculate-quote',
				quoteRequest
			)
			.subscribe(responseData => {
				this.quoteResult = responseData
				this.router.navigate([`apply-loan/${responseData.user.id}`])
			})
	}

	getQuoteResult() {
		return { ...this.quoteResult }
	}

	getUser() {
		return { ...this.user }
	}

	getQuoteRequest() {
		return { ...this.userQuote }
	}
}
