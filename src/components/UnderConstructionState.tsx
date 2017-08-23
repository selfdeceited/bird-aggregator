import * as React from "react"
import * as Blueprint from "@blueprintjs/core";

export interface UnderConstructionStateProps { }

export default class UnderConstructionState extends React.Component<UnderConstructionStateProps, undefined>  {
    render() {
        return  (
<div className="pt-non-ideal-state">
  <div className="pt-non-ideal-state-visual pt-non-ideal-state-icon">
    <span className="pt-icon pt-icon-build"></span>
  </div>
  <h4 className="pt-non-ideal-state-title">Hi! This page is currently under construction.</h4>
  <div className="pt-non-ideal-state-description">
    I'm working on it :)
  </div>
</div>)
    }
}