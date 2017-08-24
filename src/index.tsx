import * as React from "react"
import * as ReactDOM from "react-dom"
import './css/main.scss'

import { BrowserRouter, browserHistory } from 'react-router-dom'
import { App } from "./components/App"


ReactDOM.render(
    <BrowserRouter history={browserHistory}>
      <App/>
    </BrowserRouter>,
    document.getElementById('root')
  );