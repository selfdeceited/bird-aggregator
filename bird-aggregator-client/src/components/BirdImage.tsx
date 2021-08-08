import * as React from 'react'

import { PhotoProps } from 'react-photo-gallery'

interface IBirdProps {
	index: number
	onMouseDown: (index: number) => void
	photo: PhotoProps
	margin: string
	left: number | undefined
	top: number | undefined
	showCaption: boolean
	caption: string
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
		<div style={{
			height: props.photo.height,
			margin: props.margin,
			width: props.photo.width,
			...containerStyle }}>

			<img
				style={{ ...imgStyle }} {...props.photo as React.ImgHTMLAttributes<{}>}
				onMouseDown={e => props.onMouseDown(props.index)}
				alt={props.caption}
			/>
			{ props.showCaption ? <span className="image-bird-name">{props.caption}</span> : null }
			<style>
				{'.not-selected:hover{outline:2px solid #06befa}'}
			</style>
		</div>
	)
}
