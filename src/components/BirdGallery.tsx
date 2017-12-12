import * as React from "react"
import { GalleryWrap } from "./GalleryWrap"
import axios from 'axios'

export interface BirdGalleryState { 
    wikiData: WikiData
}

interface WikiData { 
    name: string;
    wikiInfo: string;
    imageUrl: string;
}

export class BirdGallery extends React.Component<any, BirdGalleryState>  {
    constructor(props) {
        super(props);

        this.state = {
            wikiData: { name: "", wikiInfo: "", imageUrl: "" }
        };
    }

    componentDidMount() {
        this.fetchWikiInfo(this.props);
    }
    
    componentWillReceiveProps(nextProps){
        this.fetchWikiInfo(nextProps);
    }

    fetchWikiInfo(props){
        axios.get(`api/birds/wiki/`+ props.match.params.id).then(res => {
            const wikiData = res.data;
            var extract = JSON.parse(wikiData.wikiInfo);
            var html = extract.query.pages[Object.keys(extract.query.pages)[0]].extract;
            
            var div = document.createElement('div');
            div.innerHTML = html;
            var firstPart = div.getElementsByTagName('p')[0].outerHTML;
            
            this.setState({ wikiData: { name: res.data.name, wikiInfo: firstPart, imageUrl: res.data.imageUrl } });
        });
    }

    render() {
        return  (
            <div>
                <div className="half-screen">
                <GalleryWrap
                    seeFullGalleryLink={false}
                    urlToFetch={`/api/birds/gallery/` + this.props.match.params.id}
                    />
                </div>
            <div className="flex-container body">

                <div className="wiki-info hide">
                    <img src={this.state.wikiData.imageUrl} width="80%"/>
                </div>
                <div className="wiki-info">
                    <h2>{this.state.wikiData.name}</h2>
                    <div dangerouslySetInnerHTML={{ __html: this.state.wikiData.wikiInfo }}></div>
                    <a className="new-window" href={"https://en.wikipedia.org/wiki/"+this.state.wikiData.name} target="_blank">more from Wikipedia...</a>
                </div>
                <div className="wiki-info hide">
                    <h4>Voice sample</h4>
                    <p>(todo)</p>
                    <code>http://www.xeno-canto.org/article/153</code>
                </div>
                <div className="wiki-info hide">
                    <h4>Occurences on map</h4>
                    <p>(todo)</p>
                </div>
            </div>
            </div>
        )
    }
}