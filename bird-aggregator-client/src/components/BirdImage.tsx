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
	tags: {
		value: string
		title: string
	}[]
	dateTaken: string
	birdIds: number[]
	text: string
}

export const BirdImage: React.FC<IBirdProps> = props => {
	const containerStyle: React.CSSProperties = {
		backgroundColor: '#eee',
		cursor: 'pointer',
		float: 'left',
		overflow: 'hidden',
		position: 'relative',
	}

	const imgStyle: React.CSSProperties = {
		borderRadius: '2px',
		display: 'block',
		transition: 'transform .135s cubic-bezier(0.0,0.0,0.2,1),opacity linear .15s',
	}

	return (
		<div style={{ height: props.photo.height,
			margin: props.margin,
			width: props.photo.width,
			...containerStyle }}>

			<img
				style={{ ...imgStyle }} {...props.photo}
				onMouseDown={e => props.onMouseDown(e, { index: props.index, photo: props.photo })}
				alt={props.photo.caption}
			/>
			{ props.showCaption ? <span className="image-bird-name">{props.photo.caption}</span> : null }
			<style>
				{'.not-selected:hover{outline:2px solid #06befa}'}
			</style>
		</div>
	)
}
