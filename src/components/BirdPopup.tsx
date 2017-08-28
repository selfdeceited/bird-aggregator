import * as React from "react"
import { Link } from 'react-router-dom'
import {BirdDto} from './MapWrap'

interface BirdPopupProps {
    birds: BirdDto[]
}
interface BirdPopupState {
    birds: BirdDto[]
}

export class BirdPopup extends React.Component<BirdPopupProps, BirdPopupState> {

    constructor(props) {
        super(props);
        this.state = {
            birds: []
        };
    }

    componentWillReceiveProps(nextProps) {
        this.setState({ birds: nextProps.birds });
    }

    //todo: fix when links does not render
    render() {
        return (<div>
                    {
                        this.state.birds.map(x => {
                        <Link key={x.id} to={"/birds/" + x.id} role="button" className="pt-button pt-minimal pt-icon-arrow-right">{x.name}</Link>
                        })
                    }
                </div>);
    }
}