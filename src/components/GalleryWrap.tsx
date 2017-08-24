import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import * as Gallery from "react-grid-gallery"
import axios from 'axios';
import { Link } from 'react-router-dom'

export interface GalleryProps {
    count: number,
    seeFullGalleryLink: boolean
 }

interface Tag {
    value: string,
    title: string
}
interface Image {
    src: string,
    thumbnail: string,
    thumbnailWidth: number,
    thumbnailHeight: number,
    caption: string,
    tags: Tag[]
}

interface GalleryState {
    images: Image[],
    count: number,
    fullGallery: boolean
}

export class GalleryWrap extends React.Component<GalleryProps, GalleryState> {
    constructor(props: GalleryProps) {
        super(props);

        this.state = {
            images: [],
            count: props.count,
            fullGallery: !props.seeFullGalleryLink
        };
    }

    componentDidMount() {
        axios.get(`/api/birds/gallery/${this.state.count}`).then(res => {
            const images = res.data.map((x: Image) => {
                x.tags = [{title: x.caption, value: x.caption}];
                x.thumbnailWidth = 500;
                return x;
            });

            this.setState({ images });
        });
    }

    render() {

        const latestShotsLink = this.state.fullGallery ? (<div></div>) : (
            <div>
                <h2 className="latest-shots">Latest {this.state.count} photos</h2>
                <Link to="/gallery" role="button" className="pt-button pt-minimal pt-icon-camera small-margin">Check Out Full Gallery</Link>
            </div>
        );

        return <div>
                    <div className="body">
                        <div>{latestShotsLink}</div>
                        <Gallery 
                         images={this.state.images}
                         backdropClosesModal={true}
                         preloadNextImage={true}
                         enableImageSelection={false}
                         showLightboxThumbnails={true}
                         maxRows={this.state.count}/>
                    </div>
               </div>;
    }
}