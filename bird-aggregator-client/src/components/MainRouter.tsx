/* eslint-disable react/jsx-no-bind */
import * as React from 'react'

import { Route, Switch } from 'react-router-dom'

import { BirdGallery } from './BirdPage/BirdGallery'
import { Gallery } from './Gallery/Gallery'
import { LifeList } from './LifeList'
import { MapContainer } from './Map/Map'
import { PhotoPage } from './PhotoPage'
import { TripList } from './TripList'

export const MainRouter: React.FC = () => {
	const previewGalleryRoute = (
		<Gallery
			seeFullGalleryLink={true}
			urlToFetch={'/api/gallery/100'}
			showImageCaptions/>)

	const fullGalleryRoute = (
		<div>
			<Gallery
				seeFullGalleryLink={false}
				urlToFetch={'/api/gallery/9000'}
				showImageCaptions />
		</div>
	)

	// fullGalleryRoute is wrapped in div as a workaround to avoid the situation in
	// https://github.com/ReactTraining/react-router/issues/4105#issuecomment-310048346

	const defaultMapContainer = (<MapContainer embedded={false} />)

	return (
		<main>
			<Switch>
				<Route exact path="/" render={() => previewGalleryRoute }/>
				<Route path="/lifelist" component={LifeList}/>
				<Route path="/map" render={() => defaultMapContainer}/>
				<Route path="/triplist" component={TripList}/>
				<Route path="/gallery" render={() => fullGalleryRoute}/>
				<Route path="/birds/:id" component={BirdGallery} />
				<Route path="/photos/:id" component={PhotoPage} />
			</Switch>
		</main>)
}