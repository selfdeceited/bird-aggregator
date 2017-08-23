import * as React from "react"
import UnderConstructionState from './UnderConstructionState'

interface TripListProps {}
interface TripListState {}

export class TripList extends React.Component<TripListProps, TripListState> {
    render() {
        return (<div className="body">
                    <UnderConstructionState/>
                </div>);
    }
}