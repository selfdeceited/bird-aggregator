import { Image as ReactGridImage } from 'react-grid-gallery'

export interface ImageProps extends ReactGridImage {
	id: string
	src: string
	original: string
	width: number
	height: number
	caption: string
	tags: {
		value: string
		title: string
	}[]
	dateTaken: string
	birdIds: number[]
	text: string
	hostingLink: string
}
