import * as axios from '../http.adapter'

export type LinkResponse = string

export async function fetchAs<T>(url: string): Promise<T> {
	const response = await axios.get<T>(url)

	// eslint-disable-next-line @typescript-eslint/no-magic-numbers
	if (response.status > 399) {
		throw new Error(response.statusText)
	}

	return response.data
}
