import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import axios from 'axios';

export interface NavbarProps { }
export interface NavbarState {
    flickr: string,
    github: string
 }
export default class Navbar extends React.Component<NavbarProps, NavbarState>  {

    constructor(props) {
        super(props);

        this.state = {
            flickr: "",
            github: ""
        };
    }

    componentDidMount() {
        axios.get(`/flickr`).then(res => {
            const flickr = res.data;
            this.setState({ flickr });
        });

        axios.get(`/github`).then(res => {
            const github = res.data;
            this.setState({ github });
        });
    }

    render() {
        return  (
<nav className="pt-navbar .modifier">
    <div className="pt-navbar-group pt-align-left">
    <div className="pt-navbar-heading">Flickr Bird Aggregator</div>
    <input className="pt-input" placeholder="Search bird..." type="text" />
  </div>
  <div className="pt-navbar-group pt-align-right">
    <a role="button" className="pt-button pt-minimal pt-icon-git-repo" href={this.state.github}>GitHub</a>
    <a role="button" className="pt-button pt-minimal pt-icon-group-objects" href={this.state.flickr}>Flickr</a>
    <span className="pt-navbar-divider"></span>
    <button className="pt-button pt-minimal pt-icon-cog"></button>
  </div>
</nav>)
    }
}