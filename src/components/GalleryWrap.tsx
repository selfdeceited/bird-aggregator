import * as Blueprint from "@blueprintjs/core"
import axios from "axios"
import * as React from "react"
import * as Gallery from "react-grid-gallery"
import { Link } from "react-router-dom"

export interface IGalleryProps {
    seeFullGalleryLink: boolean,
    urlToFetch: string
 }

interface ITag {
    value: string,
    title: string
}

export interface Image {
    src: string,
    thumbnail: string,
    thumbnailWidth: number,
    thumbnailHeight: number,
    caption: string,
    tags: ITag[],
    dateTaken: string,
    locationId: number
}

interface IGalleryState {
    images: Image[],
    fullGallery: boolean
}

export class GalleryWrap extends React.Component<IGalleryProps, IGalleryState> {
    constructor(props: IGalleryProps) {
        super(props)

        this.state = {
            fullGallery: !props.seeFullGalleryLink,
            images: [],
        }
    }

    public componentDidMount() {
        this.fetchTheUrl(this.props.urlToFetch)
    }

    public componentWillReceiveProps(nextProps) {
        this.fetchTheUrl(nextProps.urlToFetch)
    }

    public render() {
        const latestShotsLink = this.state.fullGallery ? (<div></div>) : (
            <div>
                <h2 className="latest-shots">Latest photos</h2>
                <Link to="/gallery" role="button"
                    className="pt-button pt-minimal pt-icon-camera small-margin">Check Out Full Gallery
                </Link>
            </div>
        )

        return <div>
                    <div className="body">
                        <div>{latestShotsLink}</div>
                        <Gallery
                         images={this.state.images}
                         backdropClosesModal={true}
                         preloadNextImage={true}
                         enableImageSelection={false}
                         showLightboxThumbnails={true}
                         maxRows={9000}/>
                    </div>
               </div>
    }

    private fetchTheUrl(url) {
        axios.get(url).then((res) => {
            const images = res.data.map((x: Image) => {
                x.tags = [{title: x.caption, value: x.caption}]
                x.thumbnailWidth = 500
                return x
            })
            this.setState({ images })
        })
    }
}
