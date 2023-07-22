/* eslint-disable react/jsx-no-bind */
import * as React from 'react'

import { Route, Routes } from 'react-router-dom'

import { BirdGallery } from './BirdPage/BirdGallery'
import { Gallery } from './Gallery/Gallery'
import { LifeList } from './LifeList'
import { MapContainer } from './Map/Map'
import { PhotoPage } from './PhotoPage'
import { TripList } from './TripList'

export const MainRouter: React.FC = () =>

	// fullGalleryRoute is wrapped in div as a workaround to avoid the situation in
	// https://github.com/ReactTraining/react-router/issues/4105#issuecomment-310048346
	(
		<main>
			<Routes>
				<Route path="/*" element={<Gallery
					seeFullGalleryLink={true}
					urlToFetch={'/api/gallery/100'}
					showImageCaptions/>}/>
				<Route path="/lifelist" element={<LifeList/>}/>
				<Route path="/map" element={<MapContainer embedded={false} />}/>
				<Route path="/triplist" element={<TripList/>}/>
				<Route path="/gallery" element={<Gallery
					seeFullGalleryLink={false}
					urlToFetch={'/api/gallery/9000'}
					showImageCaptions />}/>
				<Route path="/birds/:id" element={<BirdGallery/>} />
				<Route path="/photos/:id" element={<PhotoPage/>} />
			</Routes>
		</main>)

