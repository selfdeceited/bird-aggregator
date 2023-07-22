/* eslint-disable react/jsx-no-useless-fragment */
/* eslint-disable @typescript-eslint/no-magic-numbers */
/* eslint-disable react/jsx-no-bind */

import * as GeoJSON from 'geojson'
import * as React from 'react'
import { InputUrlParameters, MapMarker } from './types'

import { Marker, Map as ReactMapGl, ScaleControl } from 'react-map-gl'
import { useEffect, useState } from 'react'
import { Pin } from './Pin'
import { aggregatePhotosInSameLocation } from './locationAggregator'

import { fetchMarkersByProps } from '../../clients/MarkerClient'
import { mapStyles } from './styles'
import { mapboxAccessToken } from '../../tokens'

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
		void fetchData()
	// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [props.embedded ? props.birdId : null])

	const markerClick = (marker: MapMarker): void => {
		setCenter([marker.x, marker.y])
	}

	/* const clusterClick = (coordinates: GeoJSON.Position): void => {
		setCenter([coordinates[0], coordinates[1]])
		setZoomLevel([zoomLevel[0] + 1])
	}*/

	const key = (_: GeoJSON.Position): string => `${_[0]}:${_[1]}`

	const styles: Record<string, React.CSSProperties> = {
		clusterMarker: mapStyles.clusterMarker,
		marker: mapStyles.marker,
	}

	/* const clusterMarker: (coordinates: GeoJSON.Position, pointCount: number) => JSX.Element = (
		coordinates,
		pointCount,
	) => (
		<Marker
			key={key(coordinates)}
			latitude={coordinates[0]}
			longitude={coordinates[1]}
			style={getStyles().clusterMarker}
			onClick={() => clusterClick(coordinates)}
		>
			<div>{pointCount}</div>
		</Marker>
	)*/

	return (
		<div className={props.embedded ? '' : 'body'}>
			<ReactMapGl
				accessToken={mapboxAccessToken}
				mapStyle="mapbox://styles/mapbox/outdoors-v10"
				style={{
					height: mapHeight,
					width: mapWidth,
					position: 'relative',
				}}
				initialViewState={{
					longitude: center[0],
					latitude: center[1],
					zoom: 6,
				}}
			>
				<ScaleControl />
				<>{
					props.embedded ? markers.map(m => (<Marker
						key={key([m.x, m.y])}
						longitude={m.x}
						latitude={m.y}
						onClick={() => markerClick(m)}
						style={styles.marker}
					>
						<Pin marker={m} />
					</Marker>)) : null
				}
				</>
				{
				// <Cluster ClusterMarkerFactory={clusterMarker}>{markersMap()}</Cluster>
				}
			</ReactMapGl>
		</div>
	)
}
