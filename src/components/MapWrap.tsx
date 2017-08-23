import * as React from "react"
import UnderConstructionState from './UnderConstructionState'

interface MapWrapProps {}
interface MapWrapState {}

export class MapWrap extends React.Component<MapWrapProps, MapWrapState> {
    render() {
        return (<div className="body">
                    <UnderConstructionState/>
                </div>);
    }
}