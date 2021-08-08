/* eslint-disable react/jsx-no-bind */
import * as React from 'react'
import * as axios from '../../http.adapter'

import Carousel, { Modal, ModalGateway } from 'react-images'
import { LightboxEmptyFooter, LightboxHeader } from './LightboxHeader'
import { default as ReactGallery, RenderImageProps } from 'react-photo-gallery'
import { useCallback, useEffect, useState } from 'react'

import { BirdImage } from './BirdImage'
import { GalleryStyled } from './GalleryStyled'
import { ImageProps } from './types'
import { LatestPhotosLink } from './LatestPhotosLink'

interface Props {
	seeFullGalleryLink: boolean
	urlToFetch: string
	showImageCaptions: boolean
}

export const Gallery: React.FC<Props> = props => {
	const { seeFullGalleryLink, urlToFetch, showImageCaptions } = props
	const [viewerIsOpen, setViewerIsOpen] = useState(false)
	const [images, setImages] = useState([ ] as ImageProps[])
	const [selectedIndex, setSelectedIndex] = useState(void 0 as number | undefined)

	useEffect(() => {
		// eslint-disable-next-line @typescript-eslint/no-floating-promises
		fetchTheUrl(urlToFetch)
	}, [urlToFetch])

	/* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/
	const fetchTheUrl = async (url: string): Promise<void> => {
		const { data: { photos } } = await axios.get(url)
		const typedPhotos = photos as ImageProps[]
		const fetchedImages = typedPhotos.map(photo => {
			photo.tags = [{ title: photo.caption, value: photo.caption }]
			return photo
		})
		setImages(fetchedImages)
	}
	/* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/

	const openLightbox = useCallback((_, { index }) => {
		setSelectedIndex(index)
		setViewerIsOpen(true)
	}, [])

	const closeLightbox: () => void = () => {
		setSelectedIndex(void 0)
		setViewerIsOpen(false)
	}

	/* eslint-disable no-shadow, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-member-access */
	const renderImage: React.FC<RenderImageProps> = ({ index, left, top, photo }: RenderImageProps) => (
		<BirdImage
			key={photo.key}
			margin={'2px'}
			index={index}
			photo={photo}
			left={left}
			top={top}
			onMouseDown={i => {
				setSelectedIndex(i)
				setViewerIsOpen(true)
			}}
			showCaption={showImageCaptions}
			caption={(photo as unknown as any).caption}
		/>
	)
	/* eslint-enable no-shadow, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-member-access */

	/* eslint-disable @typescript-eslint/naming-convention */
	return (
		<GalleryStyled>
			{seeFullGalleryLink ? <LatestPhotosLink /> : null}
			<ReactGallery photos={images} onClick={openLightbox} renderImage={renderImage} />
			<ModalGateway>
				{viewerIsOpen ? (
					<Modal onClose={closeLightbox}>
						<Carousel
							components={{ Header: LightboxHeader, Footer: LightboxEmptyFooter }}
							currentIndex={selectedIndex}
							views={images.map(x => ({
								caption: x.caption,
								alt: x.caption,
								source: x.original,
								id: x.id,
								dateTaken: x.dateTaken,
								birdNames: x.caption,
								birdIds: x.birdIds,
							}))}

						/>
					</Modal>
				) : null}
			</ModalGateway>
		</GalleryStyled>
	)
	/* eslint-enable @typescript-eslint/naming-convention */
}
