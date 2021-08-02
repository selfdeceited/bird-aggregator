import * as GeoJSON from 'geojson'
import * as React from 'react'
import * as axios from '../../http.adapter'

import ReactMapboxGl, { Cluster, Marker, Popup, ZoomControl } from 'react-mapbox-gl'
import { absoluteMapStyles, mapStyles, popupStyles } from '../../styles'
import { useEffect, useState } from 'react'

import { BirdPopup } from './BirdPopup'
import { Map as RootMap } from 'mapbox-gl'

const Map = ReactMapboxGl({
	accessToken: 'pk.eyJ1IjoidG9ueXJ5emhpa292IiwiYSI6ImNpbHhvYTY0MDA4MTF0bWtyaW9xbjAyaWsifQ.ih-8rDMRiBmDPqdeyyrHNg',
})

type InputUrlParameters = {
	photoId?: number
	birdId?: number
}

type IMapWrapProps = InputUrlParameters & { embedded: boolean }

interface IMapMarkerDto {
	id: number
	x: number
	y: number
	birds: IBirdDto[]
	firstPhotoUrl: string
}

export interface IBirdDto {
	id: number
	name: string
}

export const MapWrap: React.FC<IMapWrapProps> = props => {
	const set = (_: string) => (props.embedded ? (props.birdId ? '400px' : '220px') : `100v${_}`)

	const [center, setCenter] = useState<[number, number]>([35.5, 55.6])
	const [mapHeight] = useState<string>(set('h'))
	const [mapWidth] = useState<string>(set('w'))
	const [markers, setMarkers] = useState<IMapMarkerDto[]>([])
	const [zoomLevel, setZoomLevel] = useState<[number]>([6])
	const [selectedMarker, setSelectedMarker] = useState<IMapMarkerDto | undefined>()

	const urlHandler = (propName: keyof InputUrlParameters) => {
		const check = (url: string) => props[propName] ? `${url}/${props[propName]}` : ''

		const dict: Record<keyof InputUrlParameters, string> = {
			birdId: check('/api/map/bird'),
			photoId: check('/api/map/markers'),
		}

		return dict[propName]
	}

	const fetchData = async () => {
		const urlsToFetch = ['birdId', 'photoId']
			.map(_ => urlHandler(_ as keyof InputUrlParameters))
			.filter(x => !!x)

		const urlToFetch = urlsToFetch.length > 0 ? urlsToFetch[0] : '/api/map/markers'

		const response = await axios.get(urlToFetch)
		let { markers: fetchedMarkers } = response.data

		fetchedMarkers = aggregatePhotosInSameLocation(fetchedMarkers)
		setMarkers(fetchedMarkers)

		if (props.photoId || props.birdId) {
			setCenter([fetchedMarkers[0].x, fetchedMarkers[0].y])
		}
	}
	
	useEffect(() => {
		setMarkers([])
		setSelectedMarker(undefined)
		fetchData()
	}, [])

	const markerClick = (marker: IMapMarkerDto) => {
		setCenter([marker.x, marker.y])
		setSelectedMarker(marker)
	}

	const removePopup = () => {
		setSelectedMarker(undefined)
	}

	const clusterClick = (coordinates: GeoJSON.Position) => {
		setCenter([coordinates[0], coordinates[1]])
		setZoomLevel([zoomLevel[0] + 1])
	}


	const key = (_: GeoJSON.Position) => `${_[0]}:${_[1]}`

	const getStyles: () => Record<string, React.CSSProperties> = () => ({
		clusterMarker: props.embedded ? mapStyles.clusterMarker : {
			...mapStyles.clusterMarker,
			...absoluteMapStyles.clusterMarker
		},
		marker: props.embedded ? mapStyles.marker : {
			...mapStyles.marker,
			...absoluteMapStyles.marker
		},
	})

	const clusterMarker = (
		coordinates: GeoJSON.Position,
		pointCount: number,
		getLeaves: (limit?: number, offset?: number) => React.ReactElement[],
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

	const onZoom = (map: RootMap, event: React.SyntheticEvent<any>) => {
		setZoomLevel([map.getZoom()])
	}

	return (
		<div className={props.embedded ? '' : 'body'}>
			<Map
				style="mapbox://styles/mapbox/outdoors-v10"
				containerStyle={{
					height: mapHeight,
					width: mapWidth,
				}}
				center={center}
				zoom={zoomLevel}
				onZoomEnd={onZoom}
			>
				<ZoomControl position='bottom-right'/>
				<Cluster ClusterMarkerFactory={clusterMarker}>
					{ markers.map(m => (
							<Marker
								key={key([m.x, m.y])}
								coordinates={[m.x, m.y]}
								onClick={_ => markerClick(m)}
								style={getStyles().marker}
							/>
						))}
				</Cluster>
				{ (selectedMarker && !props.embedded) ? (
					<Popup
						key={selectedMarker.id}
						offset={[0, -50]}
						coordinates={[selectedMarker.x, selectedMarker.y]}
						style={popupStyles}>
						<div className="map-popup">
							<a
								className="bp3-button bp3-minimal small-reference bp3-icon-cross"
								onMouseDown={() => removePopup()}
							></a>
							<BirdPopup birds={selectedMarker.birds} photoUrl={selectedMarker.firstPhotoUrl}/>
						</div>
					</Popup>
				): undefined }
			</Map>
		</div>
	)
}

function aggregatePhotosInSameLocation(fetchedMarkers: IMapMarkerDto[]): IMapMarkerDto[] {
	const filterDuplicateBirds = (_: IBirdDto[]) => _
		.reduce((birds, bird) => {
			if (birds.length === 0) {
				birds.push(bird)
				return birds
			}
			const existingBird = birds.filter(b => b.id === bird.id)[0]
			if (!existingBird)
				birds.push(bird)
			return birds
		}, [] as IBirdDto[])

	
	const markerEqualityComparer = (marker: IMapMarkerDto) => (candidate: IMapMarkerDto) => {
		const decimals = 3
		const numberComparer = (a: number, b: number) => a.toFixed(decimals) === b.toFixed(decimals)
		return numberComparer(candidate.x, marker.x) && numberComparer(candidate.y, marker.y)
	}


	return fetchedMarkers.reduce((acc, marker) => {
		const candidate: IMapMarkerDto = acc.filter(markerEqualityComparer(marker))[0]
		if (candidate) {
			candidate.birds = filterDuplicateBirds(candidate.birds.concat(marker.birds))
		} else {
			acc.push(marker)
		}

		return acc
	}, [] as IMapMarkerDto[])
}

