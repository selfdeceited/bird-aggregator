import * as React from "react"

export interface IUnderConstructionStateProps { }

export default class UnderConstructionState extends React.Component<IUnderConstructionStateProps, any>  {
    public render() {
        return  (
            <div className="bp3-non-ideal-state">
                <div className="bp3-non-ideal-state-visual bp3-non-ideal-state-icon">
                    <span className="bp3-icon bp3-icon-build"></span>
                </div>
                <h4 className="bp3-non-ideal-state-title">Hi! This page is currently under construction.</h4>
                <div className="bp3-non-ideal-state-description">
                    I'm working on it :)
                </div>
            </div>)
    }
}
