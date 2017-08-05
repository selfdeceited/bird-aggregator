import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import Navbar from './Navbar'
import * as Gallery from "react-grid-gallery"
import axios from 'axios';

export interface AppProps { compiler: string; framework: string; }

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

interface AppState {
    images: Image[]
}

export class App extends React.Component<AppProps, AppState> {
    constructor(props) {
        super(props);

        this.state = {
            images: []
        };
    }

    componentDidMount() {
        axios.get(`/api/birds/gallery`).then(res => {
            const images = res.data.map(x => {
                x.tags = [{title: x.caption, value: x.caption}];
                return x;
            });

            this.setState({ images });
        });
    }

    render() {
        return <div>
                    <Navbar/>
                    <div className="body">
                        <Gallery 
                         images={this.state.images}
                         backdropClosesModal={true}
                         preloadNextImage={true}
                         enableImageSelection={false}/>
                    </div>
               </div>;
    }
}