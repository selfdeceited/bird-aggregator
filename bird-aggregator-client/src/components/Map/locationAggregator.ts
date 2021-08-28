/* eslint-disable @typescript-eslint/no-unsafe-return, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-call */
import { MapMarker, ShortBirdInfo } from './types'

export function aggregatePhotosInSameLocation(fetchedMarkers: MapMarker[], markerPrecisionComparer = 3): MapMarker[] {
	const filterDuplicateBirds = (_: ShortBirdInfo[]): ShortBirdInfo[] => _
		.reduce((birds, bird) => {
			if (birds.length === 0) {
				birds.push(bird)
				return birds
			}
			const [ existingBird ] = birds.filter(b => b.id === bird.id)
			if (!existingBird) {
				birds.push(bird)
			}
			return birds
		}, [] as ShortBirdInfo[])


	const markerEqualityComparer = (marker: MapMarker) => (candidate: MapMarker): boolean => {
		const numberComparer = (a: number, b: number): boolean => a.toFixed(markerPrecisionComparer) === b.toFixed(markerPrecisionComparer)
		return numberComparer(candidate.x, marker.x) && numberComparer(candidate.y, marker.y)
	}


	return fetchedMarkers.reduce((acc, marker) => {
		const [ candidate ] = acc.filter(markerEqualityComparer(marker))
		if (candidate) {
			candidate.birds = filterDuplicateBirds(candidate.birds.concat(marker.birds))
		} else {
			acc.push(marker)
		}

		return acc
	}, [] as MapMarker[])
}
