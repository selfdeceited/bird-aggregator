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

const PopupCard = styled.div`
	position: absolute;
	bottom: 30px;
	left: 20px;
	background: #fff;
	border-radius: 6px;
	box-shadow: 0 2px 14px rgba(0, 0, 0, 0.22);
	z-index: 5;
	min-width: 190px;
	max-width: 250px;
`

const PopupHeader = styled.div`
	display: flex;
	justify-content: flex-end;
	padding: 4px 6px 0;
`

const CloseButton = styled.button`
	background: none;
	border: none;
	cursor: pointer;
	font-size: 16px;
	color: #5c7080;
	line-height: 1;
	padding: 2px 4px;
	border-radius: 3px;
	&:hover { background: #f0f4f8; }
`

const PopupBody = styled.div`
	padding: 0 10px 10px;
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
	const [selectedMarker, setSelectedMarker] = useState<MapMarker | null>(null)

	useEffect(() => {
		setMarkers([])
		setSelectedMarker(null)
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
		if (!props.embedded) {
			setSelectedMarker(marker)
		}
	}

	const key = (_: GeoJSON.Position): string => `${_[0]}:${_[1]}`

	const styles: Record<string, React.CSSProperties> = {
		clusterMarker: mapStyles.clusterMarker,
		marker: mapStyles.marker,
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
						/>
					))}
				</>
				{}
			</ReactMapGl>
			{selectedMarker && (
				<PopupCard>
					<PopupHeader>
						<CloseButton onClick={() => setSelectedMarker(null)}>×</CloseButton>
					</PopupHeader>
					<PopupBody>
						<PinPopup marker={selectedMarker} />
					</PopupBody>
				</PopupCard>
			)}
		</div>
	)
}
