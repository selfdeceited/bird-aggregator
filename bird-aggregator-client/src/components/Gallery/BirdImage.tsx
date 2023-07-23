/* eslint-disable react/jsx-no-bind */

import * as React from 'react'
import { ThumbnailImageProps } from 'react-grid-gallery'

interface IBirdProps {
	onMouseDown: (index: number) => void
	thumbnail: ThumbnailImageProps
}


const imgStyle: React.CSSProperties = {
	borderRadius: '2px',
	display: 'block',
	width: '100%',
	height: '100%',
	transition: 'transform .135s cubic-bezier(0.0,0.0,0.2,1),opacity linear .15s',
}

export const BirdImage: React.FC<IBirdProps> = ({ thumbnail, onMouseDown }) => (
	<img
		style={imgStyle}
		src={thumbnail.imageProps.src}
		onMouseDown={_ => onMouseDown(thumbnail.index)}
		alt={thumbnail.imageProps.title ?? 'no title found'}
	/>
)
