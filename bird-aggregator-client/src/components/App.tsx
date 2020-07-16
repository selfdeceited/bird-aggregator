import * as React from "react"
import Navbar from "./Navbar"
import { MainRouter } from "./routing/MainRouter"

export const App: React.FC = () => (<div>
    <Navbar/>
    <MainRouter />
</div>)
