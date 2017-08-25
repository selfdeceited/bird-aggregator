import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import * as Gallery from "react-grid-gallery"
import axios from 'axios';
import { Link } from 'react-router-dom'

export interface GalleryProps {
    seeFullGalleryLink: boolean,
    urlToFetch: string
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
    fullGallery: boolean
}

export class GalleryWrap extends React.Component<GalleryProps, GalleryState> {
    constructor(props: GalleryProps) {
        super(props);

        this.state = {
            images: [],
            fullGallery: !props.seeFullGalleryLink
        };
    }

    componentDidMount() {
        axios.get(this.props.urlToFetch).then(res => {
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
                <h2 className="latest-shots">Latest photos</h2>
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
                         maxRows={9000}/>
                    </div>
               </div>;
    }
}