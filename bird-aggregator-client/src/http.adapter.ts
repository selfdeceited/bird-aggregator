import axios, { AxiosResponse } from 'axios'

const backend_url = process.env.REACT_APP_BACKEND_URL
if (!backend_url) {
	throw new Error('empty backend url!')
}

const getUrl = (url: string): string => `${backend_url}${url}`

function get<T>(url: string): Promise<AxiosResponse<T>> {
	return axios.get<T>(getUrl(url))
}


export { get }
