import { MapContainerProps } from '../components/Map/Map'
import { MapMarker } from '../components/Map/types'
import { fetchAs } from '.'

type MapMarkersResponse = {
	markers: MapMarker[]
}

async function fetchMarkersForBird(birdId: string): Promise<MapMarker[]> {
	const url = `/api/map/bird/${birdId}`
	const { markers } = await fetchAs<MapMarkersResponse>(url)
	return markers
}

async function fetchMarkersForPhoto(photoId: string): Promise<MapMarker[]> {
	const url = `/api/map/photo/${photoId}`
	const { markers } = await fetchAs<MapMarkersResponse>(url)
	return markers
}

async function fetchAllMarkers(): Promise<MapMarker[]> {
	const url = '/api/map/markers'
	const { markers } = await fetchAs<MapMarkersResponse>(url)
	return markers
}

export async function fetchMarkersByProps(props: MapContainerProps): Promise<MapMarker[]> {
	if (props.birdId) {
		return fetchMarkersForBird(props.birdId)
	}
	if (props.photoId) {
		return fetchMarkersForPhoto(props.photoId)
	}
	return fetchAllMarkers()
}
