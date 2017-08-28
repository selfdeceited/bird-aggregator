import * as React from "react"
import { Link } from 'react-router-dom'
import {BirdDto} from './MapWrap'

interface BirdPopupProps {
    birds: BirdDto[]
}
interface BirdPopupState {

}

export class BirdPopup extends React.Component<BirdPopupProps, BirdPopupState> {

    constructor(props) {
        super(props);
        this.state = {};
    }

    render() {
        return (<div>
                    <p>Birds found:</p>
                    <div>
                    {
                        this.props.birds.map(x => (<Link 
                                key={x.id}
                                to={"/birds/" + x.id}
                                role="button"
                                className="pt-button pt-minimal pt-icon-arrow-right">
                                    {x.name}
                            </Link>
                        ))
                    }
                    </div>
                </div>);
    }
}