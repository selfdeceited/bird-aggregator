import * as signalR from "@aspnet/signalr";

import axios, { AxiosResponse } from "axios";

// Added as a hack to debug client and server-side independently.
const localServerPort = 10001;
const supportsHttps = false;

// TODO: configure based on env variables
const getUrl = (url: string) =>
  `http${supportsHttps ? "s" : ""}://localhost:${localServerPort}${url}`;

function get(url: string): Promise<AxiosResponse<any>> {
  return axios.get(getUrl(url));
}

function buildWithUrl(url: string): signalR.HubConnection {
  return new signalR.HubConnectionBuilder()
    .withUrl(getUrl(url))
    .configureLogging(signalR.LogLevel.Information)
    .build();
}

export { get, buildWithUrl };
