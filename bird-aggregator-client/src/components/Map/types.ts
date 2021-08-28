export type InputUrlParameters = {
	photoId?: number
	birdId?: number
}

export interface MapMarker {
	id: number
	x: number
	y: number
	birds: ShortBirdInfo[]
	firstPhotoUrl: string
}

export interface ShortBirdInfo {
	id: number
	name: string
}
