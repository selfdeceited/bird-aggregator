import axios from "axios"
import * as React from "react"
import { Image } from "./GalleryWrap"
import { MapWrap } from "./MapWrap"

interface IPhotoPageState {
    image: Image
    loaded: boolean
}

export class PhotoPage extends React.Component<any, IPhotoPageState> {
    constructor(props) {
        super(props)
        this.state = {
            image: null,
            loaded: false,
        }
    }

    public componentDidMount() {
        axios.get(`api/photos/${this.props.match.params.id}`).then((res) => {
            const image = res.data
            image.tags = [{title: image.caption, value: image.caption}]
            this.setState({ image, loaded: true })
        })
    }

    public render() {
        return this.state.loaded ? (<div className="body">
                    <h2 className="show-mobile">{this.state.image.caption}</h2>
                    <h4>{this.state.image.dateTaken}</h4>
                    <img src={this.state.image.src}></img>
                    <MapWrap asPopup={true} locationIdToShow={this.state.image.locationId}/>
                </div>) : null
    }
}
