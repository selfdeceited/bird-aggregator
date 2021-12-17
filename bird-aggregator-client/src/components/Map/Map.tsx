/* eslint-disable @typescript-eslint/no-magic-numbers */
/* eslint-disable react/jsx-no-bind */

import * as GeoJSON from 'geojson'
import * as React from 'react'

import { InputUrlParameters, MapMarker } from './types'
import ReactMapboxGl, { Cluster, Marker, Popup, ZoomControl } from 'react-mapbox-gl'
import { mapStyles, popupStyles } from './styles'
import { useEffect, useState } from 'react'

import { BirdPopup } from './BirdPopup'
import { Map as RootMap } from 'mapbox-gl'
import { aggregatePhotosInSameLocation } from './locationAggregator'
import { fetchMarkersByProps } from '../../clients/MarkerClient'
import { mapboxAccessToken } from '../../tokens'

// todo: consider https://github.com/visgl/react-map-gl
const MapBox = ReactMapboxGl({
	accessToken: mapboxAccessToken,
})

export type MapContainerProps = InputUrlParameters & { embedded: boolean }

const initialWidth = (props: MapContainerProps): string => {
	if (props.embedded) {
		return props.birdId ? '100%' : '220px'
	}
	return '100vw'
}

const initialHeight = (props: MapContainerProps): string => {
	if (props.embedded) {
		return props.birdId ? '400px' : '220px'
	}
	return 'calc(100vh - 50px)'
}

export const MapContainer: React.FC<MapContainerProps> = props => {
	const [center, setCenter] = useState<[number, number]>([35.5, 55.6])
	const [mapHeight] = useState<string>(initialHeight(props))
	const [mapWidth] = useState<string>(initialWidth(props))
	const [markers, setMarkers] = useState<MapMarker[]>([])
	const [zoomLevel, setZoomLevel] = useState<[number]>([6])
	const [selectedMarker, setSelectedMarker] = useState<MapMarker | undefined>()


	const fetchData = async (): Promise<void> => {
		let fetchedMarkers = await fetchMarkersByProps(props)

		fetchedMarkers = aggregatePhotosInSameLocation(fetchedMarkers)
		setMarkers(fetchedMarkers)

		if ((props.photoId || props.birdId) && fetchedMarkers.length > 0) {
			setCenter([fetchedMarkers[0].x, fetchedMarkers[0].y])
		}
	}

	useEffect(() => {
		setMarkers([])
		setSelectedMarker(void 0)
		void fetchData()
	// eslint-disable-next-line react-hooks/exhaustive-deps
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
