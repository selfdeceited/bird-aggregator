import * as React from "react"
import Navbar from './Navbar'
import MainRouter from './routing/MainRouter'

interface AppProps {}
interface AppState {}
export class App extends React.Component<AppProps, AppState> {
    render() {
        return (<div>
                    <Navbar/>
                    <MainRouter />
                </div>);
    }
}