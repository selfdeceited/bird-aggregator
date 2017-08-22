import { Switch, Route } from 'react-router-dom'
import * as React from "react"
import { LifeList } from "./LifeList"
import { GalleryWrap } from "./GalleryWrap"

interface MainRouterProps {}
interface MainRouterState {}
export default class MainRouter extends React.Component<MainRouterProps, MainRouterState> {
    render() {
        return (<main>
        <Switch>
          <Route exact path='/' component={GalleryWrap}/>
          <Route path='/lifelist' component={LifeList}/>
        </Switch>
        </main>);
    }
}