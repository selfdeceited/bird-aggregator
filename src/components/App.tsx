import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import {Navbar} from './Navbar'

export interface AppProps { compiler: string; framework: string; }

// 'HelloProps' describes the shape of props.
// State is never set so we use the 'undefined' type.
export class App extends React.Component<AppProps, undefined> {
    render() {
        return  <div>
                    <Navbar></Navbar>
                    <br/>
                    <h4>Hello from {this.props.compiler} and {this.props.framework}!</h4>
                    <Blueprint.Spinner className="pt-intent-primary pt-small" />
                </div>;
    }
}