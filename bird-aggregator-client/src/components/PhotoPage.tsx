import * as axios from "../http.adapter"
import moment from "moment"
import * as React from "react"
import { Link } from "react-router-dom"
import { Image } from "./BirdImage"
import { MapWrap } from "./MapWrap"

interface IPhotoPageState {
    image: Image
    loaded: boolean
}

export class PhotoPage extends React.Component<any, IPhotoPageState> {
    constructor(props: any) {
        super(props)
        this.state = {
            image: null as any,
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
        return this.state.loaded ? (
            <div className="body">

            {this.state.image.birdIds.map(id => <Link
                    className="big-link" to={`/birds/${id}`}>
                    {this.state.image.caption} ({moment(this.state.image.dateTaken).format("YYYY MM DD")})
                </Link>,
            )}
                <section className="photo-flex-container">
                    <div className="flex-item photo-flex-element">
                        <img src={this.state.image.original} className="photo-page"></img>
                    </div>
                    <div className="flex-item">
                        <MapWrap asPopup={true} locationIdToShow={this.state.image.locationId}/>
                    </div>
                </section>
            </div>) : null
    }
}