import * as React from "react"
import * as ReactDOM from "react-dom"
import "./css/main.scss"

import * as PullToRefresh from "pulltorefreshjs"
import { browserHistory, HashRouter } from "react-router-dom"
import { App } from "./components/App"

ReactDOM.render(
    <HashRouter history={browserHistory}>
      <App/>
    </HashRouter>,
    document.getElementById("root"),
  )

const ptr = PullToRefresh.init({
    mainElement: "main",
    onRefresh: () => window.location.reload(),
})
