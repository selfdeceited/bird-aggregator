import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import * as Gallery from "react-grid-gallery"
import axios from 'axios';

export interface GalleryProps { }

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
    count: number
}

export class GalleryWrap extends React.Component<GalleryProps, GalleryState> {
    constructor(props) {
        super(props);

        this.state = {
            images: [],
            count: 20
        };
    }

    componentDidMount() {
        axios.get(`/api/birds/gallery/${this.state.count}`).then(res => {
            const images = res.data.map(x => {
                x.tags = [{title: x.caption, value: x.caption}];
                x.thumbnailWidth = 400;
                x.thumbnailHeight = 400;
                return x;
            });

            this.setState({ images });
        });
    }

    render() {
        return <div>
                    <div className="body">
                        <h2 className="latest-shots">Latest {this.state.count} photos</h2>
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