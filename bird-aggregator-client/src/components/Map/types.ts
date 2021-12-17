export type InputUrlParameters = {
	photoId?: string
	birdId?: string
}

export interface MapMarker {
	x: number
	y: number
	birds: ShortBirdInfo[]
	firstPhotoUrl: string
}

export interface ShortBirdInfo {
	id: string
	name: string
}
