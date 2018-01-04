import * as React from "react"
import UnderConstructionState from "./UnderConstructionState"

interface ITripListProps {}
interface ITripListState {}

export class TripList extends React.Component<ITripListProps, ITripListState> {
    public render() {
        return (<div className="body">
                <h2 className="show-mobile">Trips</h2>
                    <UnderConstructionState/>
                </div>)
    }
}
