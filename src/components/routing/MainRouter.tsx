import * as React from "react"
import { Route, Switch } from "react-router-dom"
import { PhotoPage } from "../PhotoPage";
import { SeedProgress } from "../SeedProgress"
import { BirdGallery } from "./../BirdGallery"
import { GalleryWrap } from "./../GalleryWrap"
import { LifeList } from "./../LifeList"
import { MapWrap } from "./../MapWrap"
import { TripList } from "./../TripList"

interface IMainRouterProps {}
interface IMainRouterState {}

export default class MainRouter extends React.Component<IMainRouterProps, IMainRouterState> {
    public render() {

const previewGalleryRoute = (
<GalleryWrap
    seeFullGalleryLink={true}
    urlToFetch={`/api/photos/gallery/100`}/>)

const fullGalleryRoute = (
<div>
    <GalleryWrap
        seeFullGalleryLink={false}
        urlToFetch={`/api/photos/gallery/9000`}/>
</div>
)

// fullGalleryRoute is wrapped in div as a workaround to avoid the situation in
// https://github.com/ReactTraining/react-router/issues/4105#issuecomment-310048346

const defaultMapWrap = (<MapWrap asPopup={false} locationIdToShow={undefined}/>)

return (
    <main>
        <SeedProgress/>
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
}
