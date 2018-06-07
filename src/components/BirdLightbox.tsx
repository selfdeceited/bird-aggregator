import * as React from "react"
import * as Lightbox from "react-image-lightbox"
import { Link } from "react-router-dom"
import { Image } from "./BirdImage"

interface IBirdLightboxProps {
    photos: Image[],
    index: number
}

interface IBirdLightboxState {
    index: number
}

export class BirdLightbox extends React.Component<IBirdLightboxProps, IBirdLightboxState> {
    private images: string[]

    constructor(props) {
        super(props)
        this.state = {
            index: props.index,
        }
        this.images = this.props.photos.map(x => x.original)
    }

    public componentWillReceiveProps(nextProps) {
        this.setState({index: nextProps.index})
    }

    public render() {

        const getTitle = () => <div>
            {this.props.photos[this.state.index].birdIds.map(id => <Link
                      className="big-link lightbox-caption"
                      to={`/birds/${id}`}>
                      {this.props.photos[this.state.index].caption}
                 </Link>,
            )}
        </div>

        const getDescription = () => <div>
            <span>{this.props.photos[this.state.index].text}</span>
            <Link
                key={this.props.photos[this.state.index].id}
                to={"/photos/" + this.props.photos[this.state.index].id}
                role="button"
                className="pt-button pt-minimal pt-icon-arrow-right lightbox-photo-link">
                Visit photo page
            </Link>
        </div>

        const getLightbox = () => (<div>
            <Lightbox
                mainSrc={this.images[this.state.index]}
                nextSrc={this.images[(this.state.index + 1) % this.images.length]}
                prevSrc={this.images[(this.state.index + this.images.length - 1) % this.images.length]}
                onCloseRequest={() => this.setState({ index: null })}
                onMovePrevRequest={() =>
                    this.setState({
                        index: (this.state.index + this.images.length - 1) % this.images.length,
                    })
                }
                onMoveNextRequest={() =>
                    this.setState({
                        index: (this.state.index + 1) % this.images.length,
                    })
                }
                imageTitle={getTitle()}
                imageCaption={getDescription()}
            />
        </div>)

        return this.state.index === null ? null : getLightbox()
    }
}
