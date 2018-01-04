import * as React from "react"
import Navbar from "./Navbar"
import MainRouter from "./routing/MainRouter"

interface IAppProps {}
interface IAppState {}
export class App extends React.Component<IAppProps, IAppState> {
    public render() {
        return (<div>
                    <Navbar/>
                    <MainRouter />
                </div>)
    }
}
