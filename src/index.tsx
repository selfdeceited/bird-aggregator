import * as React from "react"
import * as ReactDOM from "react-dom"
import './css/main.scss'

import { HashRouter, browserHistory } from 'react-router-dom'
import { App } from "./components/App"


ReactDOM.render(
    <HashRouter history={browserHistory}>
      <App/>
    </HashRouter>,
    document.getElementById('root')
  );