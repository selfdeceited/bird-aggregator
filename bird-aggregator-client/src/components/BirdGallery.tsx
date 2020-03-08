import * as axios from "../http.adapter"
import * as React from "react"
import { GalleryWrap } from "./GalleryWrap"
import { MapWrap } from "./MapWrap"

export interface IBirdGalleryState {
    wikiData: IWikiData
}

interface IWikiData {
    name: string
    wikiInfo: string
    imageUrl: string
}

export class BirdGallery extends React.Component<any, IBirdGalleryState>  {
    constructor(props: any) {
        super(props)

        this.state = {
            wikiData: { name: "", wikiInfo: "", imageUrl: "" },
        }
    }

    public componentDidMount() {
        this.fetchWikiInfo(this.props)
    }

    public componentWillReceiveProps(nextProps: any) {
        this.fetchWikiInfo(nextProps)
    }

    public render() {
        return  (
            <div>
                <div className="half-screen">
                <GalleryWrap
                    seeFullGalleryLink={false}
                    urlToFetch={`/api/birds/gallery/` + this.props.match.params.id}
                    />
                </div>
                    <div className="flex-container body fourty">
                        {
                            !this.state.wikiData ? null : (
                            <div className="wiki-info hide">
                                <img src={this.state.wikiData.imageUrl} width="80%"/>
                            </div>)
                        }
                        {
                            !this.state.wikiData ? null : (
                            <div className="wiki-info">
                            <h2>{this.state.wikiData.name}</h2>
                            <div dangerouslySetInnerHTML={{ __html: this.state.wikiData.wikiInfo }}></div>
                            <a className="new-window"
                               href={"https://en.wikipedia.org/wiki/" + this.state.wikiData.name}
                               target="_blank">more from Wikipedia...</a>
                            </div>)
                        }
                        <div className="wiki-info">
                            <h4>Occurences on map</h4>
                            <MapWrap asPopup={true}
                                birdId={this.props.match.params.id}
                            />
                        </div>

                        <div className="wiki-info hide">
                            <h4>Voice sample</h4>
                            <p>(todo)</p>
                            <code>http://www.xeno-canto.org/article/153</code>
                        </div>
                    </div>
            </div>
        )
    }

    private fetchWikiInfo(props: any) {
        axios.get(`api/birds/wiki/` + props.match.params.id).then(res => {
            const wikiData = res.data
            const extract = JSON.parse(wikiData.wikiInfo)
            const html = extract.query.pages[Object.keys(extract.query.pages)[0]].extract

            const div = document.createElement("div")
            div.innerHTML = html
            const firstDiv = div.getElementsByTagName("p")[0]
            if (firstDiv) {
                const chapter = firstDiv.outerHTML
                this.setState({ wikiData: { name: res.data.name, wikiInfo: chapter, imageUrl: res.data.imageUrl } })
            } else {
                this.setState({ wikiData: null as unknown as IWikiData } as IBirdGalleryState)
            }
        })
    }
}
