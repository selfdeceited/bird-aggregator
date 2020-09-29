import * as signalR from '@aspnet/signalr'

import axios, { AxiosResponse } from 'axios'

const backend_url = process.env.REACT_APP_BACKEND_URL

const getUrl = (url: string) => `${backend_url}${url}`

function get(url: string): Promise<AxiosResponse> {
	return axios.get(getUrl(url))
}

function buildWithUrl(url: string): signalR.HubConnection {
	return new signalR.HubConnectionBuilder()
		.withUrl(getUrl(url))
		.configureLogging(signalR.LogLevel.Information)
		.build()
}

export { get, buildWithUrl }
