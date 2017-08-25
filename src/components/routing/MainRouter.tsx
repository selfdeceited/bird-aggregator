import { Switch, Route } from 'react-router-dom'
import * as React from "react"
import { LifeList } from "./../LifeList"
import { GalleryWrap } from "./../GalleryWrap"
import { MapWrap } from "./../MapWrap"
import { TripList } from "./../TripList"
import { BirdGallery } from "./../BirdGallery"

interface MainRouterProps {}
interface MainRouterState {}
export default class MainRouter extends React.Component<MainRouterProps, MainRouterState> {
    render() {

const previewGalleryRoute = (
<GalleryWrap 
    seeFullGalleryLink={true}
    urlToFetch={`/api/photos/gallery/40`}/>);

const fullGalleryRoute = (
<div>
    <GalleryWrap
        seeFullGalleryLink={false}
        urlToFetch={`/api/photos/gallery/9000`}/>
</div>
);

// fullGalleryRoute is wrapped in div as a workaround to avoid the situation in 
// https://github.com/ReactTraining/react-router/issues/4105#issuecomment-310048346

        return (
    <main>
        <Switch>
          <Route exact path='/' render={() => previewGalleryRoute }/>
          <Route path='/lifelist' component={LifeList}/>
          <Route path='/map' component={MapWrap}/>
          <Route path='/triplist' component={TripList}/>
          <Route path='/gallery' render={() => fullGalleryRoute}/>
          <Route path="/birds/:id" component={BirdGallery}/>
        </Switch>
    </main>);
    }
}