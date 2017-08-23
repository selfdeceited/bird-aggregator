import { Switch, Route } from 'react-router-dom'
import * as React from "react"
import { LifeList } from "./../LifeList"
import { GalleryWrap } from "./../GalleryWrap"
import { MapWrap } from "./../MapWrap"
import { TripList } from "./../TripList"

interface MainRouterProps {}
interface MainRouterState {}
export default class MainRouter extends React.Component<MainRouterProps, MainRouterState> {
    render() {
        return (<main>
        <Switch>
          <Route exact path='/' component={GalleryWrap}/>
          <Route path='/lifelist' component={LifeList}/>
          <Route path='/map' component={MapWrap}/>
          <Route path='/triplist' component={TripList}/>
        </Switch>
        </main>);
    }
}