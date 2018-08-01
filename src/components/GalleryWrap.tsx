import * as Blueprint from "@blueprintjs/core"
import axios from "axios"
import * as React from "react"
import Measure from "react-measure"
import Gallery from "react-photo-gallery"
import { Link } from "react-router-dom"
import { BirdImage, Image } from "./BirdImage"
import { BirdLightbox } from "./BirdLightbox"

export interface IGalleryProps {
    seeFullGalleryLink: boolean,
    urlToFetch: string
 }

interface IGalleryState {
    images: Image[],
    fullGallery: boolean,
    width: number,
    selectedIndex: number
}

export class GalleryWrap extends React.Component<IGalleryProps, IGalleryState> {
    constructor(props: IGalleryProps) {
        super(props)

        this.state = {
            fullGallery: !props.seeFullGalleryLink,
            images: [],
            selectedIndex: null,
            width: -1,
        }

        this.onClick = this.onClick.bind(this)
    }

    public componentDidMount() {
        this.fetchTheUrl(this.props.urlToFetch)
    }

    public componentWillReceiveProps(nextProps) {
        this.fetchTheUrl(nextProps.urlToFetch)
        this.setState({selectedIndex: null})
    }

    public onLightBoxClose() {
        this.setState({
          selectedIndex: null,
        })
    }

    public render() {
        const latestShotsLink = this.state.fullGallery ? <div></div> : (
            <div>
                <h2 className="latest-shots">Latest photos</h2>
                <Link to="/gallery" role="button"
                    className="pt-button pt-minimal pt-icon-camera small-margin">
                    Check Out Full Gallery
                </Link>
            </div>
        )

        const measure = (
            <Measure bounds onResize={contentRect => this.setState({ width: contentRect.bounds.width })}>
              {
                ({measureRef}) => {
                if (this.state.width < 1) {
                  return <div ref={measureRef}></div>
                }

                return <div ref={measureRef}>
                        <div className="body">
                            {latestShotsLink}
                            <Gallery
                                photos={this.state.images}
                                columns={this.getColumnSize(this.state.width)}
                                ImageComponent={BirdImage}
                                onClick={this.onClick}
                            />
                        </div>
                        {
                            (this.state.selectedIndex !== null) ? <BirdLightbox
                                photos={this.state.images}
                                index={this.state.selectedIndex}
                                onClose={() => this.onLightBoxClose()}
                            /> : null
                         }
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

    private getColumnSize(width) {
        const columnWidthMap = [
            { cols: 1, minWidth: 1 },
            { cols: 2, minWidth: 480 },
            { cols: 3, minWidth: 1024 },
            { cols: 4, minWidth: 1824 },
        ]

        let columns = 0
        columnWidthMap.map(x => columns = width >= x.minWidth ? x.cols : columns)
        return columns
    }

    private onClick(event, obj) {
        this.setState({selectedIndex: obj.index})
    }
}
