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
                    urlToFetch={`/api/gallery/bird/` + this.props.match.params.id}
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
        axios.get(`/api/birds/info/` + props.match.params.id).then(res => {
            const wikiData = res.data
            const extract = JSON.parse(wikiData.wikiInfo)
            const html = extract.query.pages[Object.keys(extract.query.pages)[0]].extract

            const div = document.createElement("div")
            div.innerHTML = html

            try {
                const chapters = this.fillChapters(div)
                this.setState({ wikiData: { name: res.data.name, wikiInfo: chapters.outerHTML, imageUrl: res.data.imageUrl } })
            }
            catch {
                this.setState({ wikiData: null as unknown as IWikiData } as IBirdGalleryState)
            }
        })
    }

    private fillChapters(div: HTMLDivElement): HTMLDivElement
    {
        const finalContainer = document.createElement("div")
        const paragraphs = Array.prototype.slice.call(div.getElementsByTagName("p")).filter(x => x.innerHTML.length > 2)
        // tslint:disable-next-line: prefer-for-of
        for (let i = 0 ; i < paragraphs.length; i++) {
            finalContainer.append(paragraphs[i])
            if (finalContainer.innerHTML.length > 1000)
                break
        }

        return finalContainer
    }
}
