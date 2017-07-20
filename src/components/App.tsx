import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import Navbar from './Navbar'
import UnderConstructionState from './UnderConstructionState'

export interface AppProps { compiler: string; framework: string; }
const footerStyle ={
    flex: '0 0 auto'
}
export class App extends React.Component<AppProps, undefined> {
    render() {
        return  <div>
                    <Navbar></Navbar>
                    
                    <div className="body">
                        <UnderConstructionState></UnderConstructionState>
                    </div>
                    
                    <footer>Done with love and pain by using {this.props.compiler} and {this.props.framework}, 2017</footer>
                </div>;
    }
}