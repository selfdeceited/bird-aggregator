import * as React from 'react'

interface IBirdProps {
	index: number
	onMouseDown: any
	photo: Image
	margin: string
	left: string
	top: string
	showCaption: boolean
}

export interface Image {
	id: string
	src: string
	original: string
	width: number
	height: number
	caption: string
	tags: ITag[]
	dateTaken: string
	birdIds: number[]
	text: string
}

interface ITag {
	value: string
	title: string
}

export const BirdImage: React.FC<IBirdProps> = props => {
	const cont = {
		backgroundColor: '#eee',
		cursor: 'pointer',
		float: 'left',
		overflow: 'hidden',
		position: 'relative',
	} as any

	const imgStyle = {
		borderRadius: '2px',
		display: 'block',
		transition: 'transform .135s cubic-bezier(0.0,0.0,0.2,1),opacity linear .15s',
	} as any

	return (
		<div style={{ height: props.photo.height,
			margin: props.margin,
			width: props.photo.width,
			...cont }}>

			<img style={{ ...imgStyle }} {...props.photo}
				onMouseDown={e => props.onMouseDown(e, { index: props.index, photo: props.photo })} />
			{ props.showCaption ? <span className="image-bird-name">{props.photo.caption}</span> : null }
			<style>
				{'.not-selected:hover{outline:2px solid #06befa}'}
			</style>
		</div>
	)
}
