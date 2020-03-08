import * as React from "react"
import { Link } from "react-router-dom"
import { IBirdDto } from "./MapWrap"

interface IBirdPopupProps {
    birds: IBirdDto[]
    photoUrl: string
}
interface IBirdPopupState {

}

export class BirdPopup extends React.Component<IBirdPopupProps, IBirdPopupState> {
    constructor(props: IBirdPopupProps) {
        super(props)
        this.state = {}
    }

    public render() {

        const imagePreview = this.props.birds.length > 2 ? null :
        (<div>
            <img src={this.props.photoUrl} className="marker-thumbnail"/>
        </div>)

        return (<div>
                    <p>Birds found:</p>
                    {
                        this.props.birds.filter(x => x.id > 0).map(x => (<Link
                                key={x.id}
                                to={"/birds/" + x.id}
                                role="button"
                                className="pt-button pt-minimal pt-icon-arrow-right display-block small-reference">
                                    {x.name}
                            </Link>
                        ))
                    }
                    {imagePreview}
                </div>)
    }
}
