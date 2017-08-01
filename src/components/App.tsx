import * as React from "react"
import * as Blueprint from "@blueprintjs/core";
import Navbar from './Navbar'
import UnderConstructionState from './UnderConstructionState'
import * as Gallery from "react-grid-gallery"

export interface AppProps { compiler: string; framework: string; }
const footerStyle ={
    flex: '0 0 auto'
}

const IMAGES =
[{
        src: "https://c2.staticflickr.com/9/8817/28973449265_07e3aa5d2e_b.jpg",
        thumbnail: "https://c2.staticflickr.com/9/8817/28973449265_07e3aa5d2e_n.jpg",
        thumbnailWidth: 320,
        thumbnailHeight: 174,
        caption: "After Rain (Jeshu John - designerspics.com)"
},
{
        src: "https://c2.staticflickr.com/9/8356/28897120681_3b2c0f43e0_b.jpg",
        thumbnail: "https://c2.staticflickr.com/9/8356/28897120681_3b2c0f43e0_n.jpg",
        thumbnailWidth: 320,
        thumbnailHeight: 212,
        tags: [{value: "Ocean", title: "Ocean"}, {value: "People", title: "People"}],
        caption: "Boats (Jeshu John - designerspics.com)"
},

{
        src: "https://c4.staticflickr.com/9/8887/28897124891_98c4fdd82b_b.jpg",
        thumbnail: "https://c4.staticflickr.com/9/8887/28897124891_98c4fdd82b_n.jpg",
        thumbnailWidth: 320,
        thumbnailHeight: 212
}]

export class App extends React.Component<AppProps, undefined> {
    getImages() {
        return IMAGES
    }

    render() {
        return  <div>
                    <Navbar></Navbar>
                    <div className="body">
                        <UnderConstructionState></UnderConstructionState>
                        <Gallery images={this.getImages()} backdropClosesModal={true}/>
                    </div>
                    <footer>Done with love and pain by using {this.props.compiler} and {this.props.framework}, 2017</footer>
                </div>;
    }
}