import React, { useEffect, useState } from 'react'

import { ImageProps } from './Gallery/types'
import { Link } from 'react-router-dom'
import { MapContainer } from './Map/Map'
import { fetchPhoto } from '../clients/PhotoClient'
import moment from 'moment'
import { useParamsTyped } from '../hacks/useParamsHack'


export const PhotoPage: React.FC = () => {
	const { id: photoId } = useParamsTyped<'id', string>()
	const [image, setImage] = useState<ImageProps | undefined>(undefined)

	const fillPhoto = async (id: string): Promise<void> => {
		const receivedImage = await fetchPhoto(id)
		receivedImage.tags = [{ title: receivedImage.caption, value: receivedImage.caption }]
		setImage(receivedImage)
	}

	useEffect(() => {
		void fillPhoto(photoId)
	}, [photoId])

	if (!image) {
		return null
	}

	return (
		<div className="body">
			{image.birdIds.map(id => <Link
				key={id}
				className="big-link" to={`/birds/${id}`}>
				{image.caption} ({moment(image.dateTaken).format('YYYY MM DD')})
			</Link>,
			)}
			<section className="photo-flex-container">
				<div className="photo-flex-item photo-flex-element">
					<img src={image.original} className="photo-page" alt={image.caption}></img>
				</div>
				<div className="photo-flex-item">
					<MapContainer embedded photoId={photoId}/>
				</div>
			</section>
		</div>)
}
