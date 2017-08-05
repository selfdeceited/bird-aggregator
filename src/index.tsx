import * as React from "react"
import * as ReactDOM from "react-dom"
// import './css/main.scss' // - removed as included as another file by now

import { App } from "./components/App"

ReactDOM.render(
    <App compiler="TypeScript" framework="React" />,
    document.getElementById("example")
);