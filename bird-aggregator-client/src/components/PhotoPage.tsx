import * as axios from '../http.adapter'

import React, { useEffect, useState } from 'react'

import { ImageProps } from './Gallery/types'
import { Link } from 'react-router-dom'
import { MapContainer } from './Map/Map'
import moment from 'moment'

export const PhotoPage: React.FC<any> = props => {
	const [image, setImage] = useState<ImageProps | undefined>(void 0)
	/* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment */
	const photoId = props.match.params.id as number


	const fetchPhoto = async (id: number): Promise<void> => {
		const res = await axios.get(`/api/gallery/photo/${id}`)
		const receivedImage = res.data.photo
		receivedImage.tags = [{ title: receivedImage.caption, value: receivedImage.caption }]
		setImage(receivedImage)
	}

	useEffect(() => {
		// eslint-disable-next-line @typescript-eslint/no-floating-promises
		fetchPhoto(photoId)
	}, [photoId])
	/* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment */

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
				<div className="flex-item photo-flex-element">
					<img src={image.original} className="photo-page" alt={image.caption}></img>
				</div>
				<div className="flex-item">
					<MapContainer embedded photoId={photoId}/>
				</div>
			</section>
		</div>)
}
