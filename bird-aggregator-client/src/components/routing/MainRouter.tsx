import * as React from "react"
import { Route, Switch } from "react-router-dom"
import { PhotoPage } from "../PhotoPage"
import { BirdGallery } from "./../BirdGallery"
import { GalleryWrap } from "./../GalleryWrap"
import { LifeList } from "./../LifeList"
import { MapWrap } from "./../MapWrap"
import { TripList } from "./../TripList"

export const MainRouter: React.FC = () => {
    const previewGalleryRoute = (
    <GalleryWrap
        seeFullGalleryLink={true}
        urlToFetch={`/api/gallery/100`}/>)

    const fullGalleryRoute = (
    <div>
        <GalleryWrap
            seeFullGalleryLink={false}
            urlToFetch={`/api/gallery/9000`}/>
    </div>
    )

    // fullGalleryRoute is wrapped in div as a workaround to avoid the situation in
    // https://github.com/ReactTraining/react-router/issues/4105#issuecomment-310048346

    const defaultMapWrap = (<MapWrap asPopup={false} locationIdToShow={undefined}/>)

    return (
        <main>
            <Switch>
            <Route exact path="/" render={() => previewGalleryRoute }/>
            <Route path="/lifelist" component={LifeList}/>
            <Route path="/map" render={() => defaultMapWrap}/>
            <Route path="/triplist" component={TripList}/>
            <Route path="/gallery" render={() => fullGalleryRoute}/>
            <Route path="/birds/:id" component={BirdGallery} />
            <Route path="/photos/:id" component={PhotoPage} />
            </Switch>
        </main>)
}
