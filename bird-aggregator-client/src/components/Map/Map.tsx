/* eslint-disable react/jsx-no-useless-fragment */
/* eslint-disable @typescript-eslint/no-magic-numbers */
/* eslint-disable react/jsx-no-bind */

import * as GeoJSON from 'geojson'
import * as React from 'react'
import {
	FullscreenControl,
	GeolocateControl,
	MapRef,
	Marker,
	NavigationControl,
	Map as ReactMapGl,
	ScaleControl,
} from 'react-map-gl'
import { InputUrlParameters, MapMarker } from './types'
import styled, { keyframes } from 'styled-components'
import { useEffect, useRef, useState } from 'react'
import { PinPopup } from './PinPopup'
import { aggregatePhotosInSameLocation } from './locationAggregator'
import { fetchMarkersByProps } from '../../clients/MarkerClient'
import { mapStyles } from './styles'
import { mapboxAccessToken } from '../../tokens'

const spin = keyframes`
  to { transform: rotate(360deg); }
`

const SpinnerOverlay = styled.div`
	position: absolute;
	inset: 0;
	display: flex;
	align-items: center;
	justify-content: center;
	background: rgba(255, 255, 255, 0.5);
	z-index: 10;
`

const Spinner = styled.div`
	width: 36px;
	height: 36px;
	border: 3px solid #d0d0d0;
	border-top-color: #555;
	border-radius: 50%;
	animation: ${spin} 0.7s linear infinite;
`

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
	const mapRef = useRef<MapRef>(null)
	const [mapHeight] = useState<string>(initialHeight(props))
	const [mapWidth] = useState<string>(initialWidth(props))
	const [markers, setMarkers] = useState<MapMarker[]>([])
	const [loading, setLoading] = useState(true)

	useEffect(() => {
		setMarkers([])
		setLoading(true)
		const fetchData = async (): Promise<void> => {
			let fetchedMarkers = await fetchMarkersByProps(props)
			fetchedMarkers = aggregatePhotosInSameLocation(fetchedMarkers)
			setMarkers(fetchedMarkers)
			setLoading(false)
		}
		void fetchData()
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [props.embedded ? props.birdId : null])

	useEffect(() => {
		if (markers.length === 0 || !mapRef.current) {
			return
		}

		if (props.photoId || props.birdId) {
			mapRef.current.flyTo({ center: [markers[0].x, markers[0].y], zoom: 10 })
		} else {
			const lons = markers.map(m => m.x)
			const lats = markers.map(m => m.y)
			mapRef.current.fitBounds(
				[
					[Math.min(...lons), Math.min(...lats)],
					[Math.max(...lons), Math.max(...lats)],
				],
				{ padding: 60 },
			)
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [markers])

	const markerClick = (marker: MapMarker): void => {
		mapRef.current?.flyTo({ center: [marker.x, marker.y] })
	}

	const key = (_: GeoJSON.Position): string => `${_[0]}:${_[1]}`

	const styles: Record<string, React.CSSProperties> = {
		clusterMarker: mapStyles.clusterMarker,
		marker: mapStyles.marker,
	}

	function markerSelected(_: MapMarker): boolean {
		return false
	}

	return (
		<div style={{ overflow: 'hidden', position: 'relative' }}>
			{loading && (
				<SpinnerOverlay>
					<Spinner />
				</SpinnerOverlay>
			)}
			<ReactMapGl
				ref={mapRef}
				mapboxAccessToken={mapboxAccessToken}
				mapStyle="mapbox://styles/mapbox/outdoors-v10"
				style={{
					height: mapHeight,
					width: mapWidth,
					position: 'relative',
				}}
				initialViewState={{
					longitude: 35.5,
					latitude: 55.6,
					zoom: 4,
				}}
			>
				<GeolocateControl position="top-left" />
				<FullscreenControl position="top-left" />
				<NavigationControl position="top-left" />
				<ScaleControl />
				<>
					{markers.map(m => (
						<Marker
							key={key([m.x, m.y])}
							longitude={m.x}
							latitude={m.y}
							onClick={() => markerClick(m)}
							style={styles.marker}
						>
							{markerSelected(m) && <PinPopup marker={m} />}
						</Marker>
					))}
				</>
				{}
			</ReactMapGl>
		</div>
	)
}
