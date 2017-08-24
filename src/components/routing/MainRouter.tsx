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
          <Route exact path='/' render={() => <GalleryWrap count={40} seeFullGalleryLink={true}/>}/>
          <Route path='/lifelist' component={LifeList}/>
          <Route path='/map' component={MapWrap}/>
          <Route path='/triplist' component={TripList}/>
          
          <Route path='/gallery' render={() => <div><GalleryWrap count={9000} seeFullGalleryLink={false}/></div>}/>
          {
              // GalleryWrap is wrapped in div as a workaround to avoid the situation in 
              // https://github.com/ReactTraining/react-router/issues/4105#issuecomment-310048346
          }
        </Switch>
        </main>);
    }
}