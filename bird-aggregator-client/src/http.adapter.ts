import axios, { AxiosResponse } from "axios"
import * as signalR from "@aspnet/signalr"

// Added as a hack to debug client and server-side independently.
const localServerPort = 5002
const defaultDebug = false

// TODO: configure based on env! just proxy for prod.
const getUrl = (url: string) => defaultDebug ? url : `http://localhost:${localServerPort}${url}`

function get(url: string): Promise<AxiosResponse<any>> {
    return axios.get(getUrl(url))
}

function buildWithUrl(url: string): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
            .withUrl(getUrl(url))
            .configureLogging(signalR.LogLevel.Information)
            .build()
}

export {
    get,
    buildWithUrl
}


