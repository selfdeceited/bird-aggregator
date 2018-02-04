import * as Blueprint from "@blueprintjs/core"
import axios from "axios"
import * as React from "react"
import Measure from "react-measure"
import Gallery from "react-photo-gallery"
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
    original: string,
    width: number,
    height: number,
    caption: string,
    tags: ITag[],
    dateTaken: string,
    locationId: number,
    birdId: number
}

interface IGalleryState {
    images: Image[],
    fullGallery: boolean,
    width: number
}

export class GalleryWrap extends React.Component<IGalleryProps, IGalleryState> {
    constructor(props: IGalleryProps) {
        super(props)

        this.state = {
            fullGallery: !props.seeFullGalleryLink,
            images: [],
            width: -1,
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

        const width = this.state.width
        const measure = (
            <Measure bounds onResize={(contentRect) => this.setState({ width: contentRect.bounds.width })}>
              {
              ({measureRef}) => {
                if (width < 1 ) {
                  return <div ref={measureRef}></div>
                }
                let columns = 1
                if (width >= 480) {
                  columns = 2
                }
                if (width >= 1024) {
                  columns = 3
                }
                if (width >= 1824) {
                  columns = 4
                }
                return <div ref={measureRef}>
                        <div className="body">
                            <div>{latestShotsLink}</div>
                            <Gallery
                                photos={this.state.images}
                                columns={columns}
                            />
                        </div>
                    </div>
              }
            }
            </Measure>
          )
        return measure
    }

    private fetchTheUrl(url) {
        axios.get(url).then((res) => {
            const images = res.data.map((x: Image) => {
                x.tags = [{title: x.caption, value: x.caption}]
                return x
            })
            this.setState({ images })
        })
    }
}
