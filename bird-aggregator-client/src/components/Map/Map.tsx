/* eslint-disable @typescript-eslint/no-magic-numbers */
/* eslint-disable react/jsx-no-bind */

import * as GeoJSON from 'geojson'
import * as React from 'react'
import * as axios from '../../http.adapter'

import { InputUrlParameters, MapMarker } from './types'
import ReactMapboxGl, { Cluster, Marker, Popup, ZoomControl } from 'react-mapbox-gl'
import { mapStyles, popupStyles } from './styles'
import { useEffect, useState } from 'react'

import { BirdPopup } from './BirdPopup'
import { Map as RootMap } from 'mapbox-gl'
import { aggregatePhotosInSameLocation } from './locationAggregator'
import { mapboxAccessToken } from '../../tokens'

// todo: consider https://github.com/visgl/react-map-gl
const MapBox = ReactMapboxGl({
	accessToken: mapboxAccessToken,
})

type MapContainerProps = InputUrlParameters & { embedded: boolean }

export const MapContainer: React.FC<MapContainerProps> = props => {
	const initialWidth = (): string => {
		if (props.embedded) {
			return props.birdId ? '100%' : '220px'
		}
		return '100vw'
	}
	const initialHeight = (): string => {
		if (props.embedded) {
			return props.birdId ? '400px' : '220px'
		}
		return 'calc(100vh - 50px)'
	}

	const [center, setCenter] = useState<[number, number]>([35.5, 55.6])
	const [mapHeight] = useState<string>(initialHeight())
	const [mapWidth] = useState<string>(initialWidth())
	const [markers, setMarkers] = useState<MapMarker[]>([])
	const [zoomLevel, setZoomLevel] = useState<[number]>([6])
	const [selectedMarker, setSelectedMarker] = useState<MapMarker | undefined>()

	const urlHandler = (propName: keyof InputUrlParameters): string => {
		const check = (url: string): string => (props[propName] ? `${url}/${props[propName] ?? ''}` : '')

		const dict: Record<keyof InputUrlParameters, string> = {
			birdId: check('/api/map/bird'),
			photoId: check('/api/map/markers'),
		}

		return dict[propName]
	}

	/* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/
	const fetchData = async (): Promise<void> => {
		const urlsToFetch = ['birdId', 'photoId']
			.map(_ => urlHandler(_ as keyof InputUrlParameters))
			.filter(x => Boolean(x))

		const urlToFetch = urlsToFetch.length > 0 ? urlsToFetch[0] : '/api/map/markers'

		const response = await axios.get(urlToFetch)
		let { markers: fetchedMarkers } = response.data

		fetchedMarkers = aggregatePhotosInSameLocation(fetchedMarkers)
		setMarkers(fetchedMarkers)

		if ((props.photoId || props.birdId) && fetchedMarkers.length > 0) {
			setCenter([fetchedMarkers[0].x, fetchedMarkers[0].y])
		}
	}
	/* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/

	useEffect(() => {
		setMarkers([])
		setSelectedMarker(void 0)
		// eslint-disable-next-line @typescript-eslint/no-floating-promises
		fetchData()
	}, [props.embedded ? props.birdId : null])

	const markerClick = (marker: MapMarker): void => {
		setCenter([marker.x, marker.y])
		setSelectedMarker(marker)
	}

	const removePopup = (): void => {
		setSelectedMarker(void 0)
	}

	const clusterClick = (coordinates: GeoJSON.Position): void => {
		setCenter([coordinates[0], coordinates[1]])
		setZoomLevel([zoomLevel[0] + 1])
	}

	const key = (_: GeoJSON.Position): string => `${_[0]}:${_[1]}`

	const getStyles: () => Record<string, React.CSSProperties> = () => ({
		clusterMarker: mapStyles.clusterMarker,
		marker: mapStyles.marker,
	})

	const clusterMarker: (coordinates: GeoJSON.Position, pointCount: number) => JSX.Element = (
		coordinates,
		pointCount,
	) => (
		<Marker
			key={key(coordinates)}
			coordinates={coordinates}
			style={getStyles().clusterMarker}
			onClick={_ => clusterClick(coordinates)}
		>
			<div>{pointCount}</div>
		</Marker>
	)

	const onZoom = (map: RootMap, _: React.SyntheticEvent<any>): void => {
		setZoomLevel([map.getZoom()])
	}

	const markersMap = (): JSX.Element[] =>
		markers.map(m => (
			<Marker
				key={key([m.x, m.y])}
				coordinates={[m.x, m.y]}
				onClick={_ => markerClick(m)}
				style={getStyles().marker}
			/>
		))

	return (
		<div className={props.embedded ? '' : 'body'}>
			<MapBox
				// eslint-disable-next-line react/style-prop-object
				style="mapbox://styles/mapbox/outdoors-v10"
				containerStyle={{
					height: mapHeight,
					width: mapWidth,
					position: 'relative',
				}}
				center={center}
				zoom={zoomLevel}
				onZoomEnd={onZoom}
			>
				<ZoomControl position={props.embedded ? 'top-right' : 'bottom-right'} />

				{props.embedded ? (
					<>{markersMap()}</>
				) : (
					<Cluster ClusterMarkerFactory={clusterMarker}>{markersMap()}</Cluster>
				)}

				{selectedMarker && !props.embedded ? (
					<Popup
						key={selectedMarker.id}
						offset={[0, -50]}
						coordinates={[selectedMarker.x, selectedMarker.y]}
						style={popupStyles}
					>
						<div className="map-popup">
							<button
								style={{ float: 'right' }}
								className="bp3-button bp3-minimal small-reference bp3-icon-cross"
								onMouseDown={removePopup}
							></button>
							<BirdPopup birds={selectedMarker.birds} photoUrl={selectedMarker.firstPhotoUrl} />
						</div>
					</Popup>
				) : (
					void 0
				)}
			</MapBox>
		</div>
	)
}
