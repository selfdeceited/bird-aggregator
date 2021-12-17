import { MapMarker, ShortBirdInfo } from './types'

import { aggregatePhotosInSameLocation } from './locationAggregator'

const duck: ShortBirdInfo = { id: '1', name: 'duck' }
const owl: ShortBirdInfo = { id: '2', name: 'owl' }
const eagle: ShortBirdInfo = { id: '3', name: 'eagle' }

describe('should correctly aggregate locations', () => {
	it("when there's none", () => {
		const rawMarkers: MapMarker[] = []
		const markers = aggregatePhotosInSameLocation(rawMarkers)
		expect(markers.length).toBe(0)
	})

	it("when there's literally same location", () => {
		const rawMarkers: MapMarker[] = [
			{
				x: 5,
				y: 5,
				birds: [duck],
				firstPhotoUrl: 'string',
			},
			{
				x: 5,
				y: 5,
				birds: [owl],
				firstPhotoUrl: 'string',
			},
		]
		const markers = aggregatePhotosInSameLocation(rawMarkers)
		expect(markers.length).toBe(1)
		expect(markers).toContainEqual(
			expect.objectContaining<Partial<MapMarker>>({
				x: 5,
				y: 5,
				birds: [duck, owl],
			}),
		)
	})

	it.each`
		precision | expectedMarkers
		${0}      | ${[{ x: 5, y: 5, birds: [duck, owl, eagle] }]}
		${1}      | ${[{ x: 5, y: 5, birds: [duck, owl] }, { x: 5.1, y: 4.6, birds: [eagle, owl] }]}
	`(
		"when there's not the same location, but with precision $precision it either merges markers or not",
		({ precision, expectedMarkers }) => {
			const rawMarkers: MapMarker[] = [
				{
					x: 5,
					y: 5,
					birds: [duck, owl],
					firstPhotoUrl: 'string',
				},
				{
					x: 5.1,
					y: 4.6,
					birds: [eagle, owl],
					firstPhotoUrl: 'string',
				},
			]
			const markers = aggregatePhotosInSameLocation(rawMarkers, precision)
			// eslint-disable-next-line @typescript-eslint/unbound-method
			expect(markers).toEqual(expectedMarkers.map(expect.objectContaining))
		},
	)
})
