import * as React from "react"
import * as Blueprint from "@blueprintjs/core";

export interface NavbarProps { }

export default class Navbar extends React.Component<NavbarProps, undefined>  {
    render() {
        return  (
<nav className="pt-navbar .modifier">
    <div className="pt-navbar-group pt-align-left">
    <div className="pt-navbar-heading">Flickr Bird Aggregator</div>
    <input className="pt-input" placeholder="Search bird..." type="text" />
  </div>
  <div className="pt-navbar-group pt-align-right">
    <button className="pt-button pt-minimal pt-icon-git-repo">GitHub</button>
    <button className="pt-button pt-minimal pt-icon-group-objects">Flickr</button>
    <span className="pt-navbar-divider"></span>
    <button className="pt-button pt-minimal pt-icon-cog"></button>
  </div>
</nav>)
    }
}