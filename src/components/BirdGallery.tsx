import * as React from "react"
import { GalleryWrap } from "./GalleryWrap"

export interface BirdGalleryState { }

export class BirdGallery extends React.Component<any, BirdGalleryState>  {
    render() {
        return  (<div className={'' + this.props.match.params.id}>
                <GalleryWrap
                    seeFullGalleryLink={false}
                    urlToFetch={`/api/birds/gallery/` + this.props.match.params.id}
                /></div>)
    }
}