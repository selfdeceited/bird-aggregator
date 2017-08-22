import * as React from "react"
import * as ReactDOM from "react-dom"
// import './css/main.scss' // - removed as included as another file by now

import { BrowserRouter, Route, IndexRoute, browserHistory } from 'react-router-dom'
import { App } from "./components/App"


ReactDOM.render(
    <BrowserRouter history={browserHistory}>
      <App/>
    </BrowserRouter>,
    document.getElementById('root')
  );