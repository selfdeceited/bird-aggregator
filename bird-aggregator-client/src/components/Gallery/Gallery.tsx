/* eslint-disable react/jsx-no-bind */
import * as React from 'react'

import Carousel, { Modal } from 'react-images'
import { LightboxFooter, LightboxHeader } from './LightboxHeader'
import { default as ReactGallery, RenderImageProps } from 'react-photo-gallery'
import { useCallback, useEffect, useState } from 'react'

import { BirdImage } from './BirdImage'
import { GalleryStyled } from './GalleryStyled'
import { ImageProps } from './types'
import { LatestPhotosLink } from './LatestPhotosLink'
import { fetchGalleryPhotos } from '../../clients/GalleryClient'


interface Props {
	seeFullGalleryLink: boolean
	urlToFetch: string
	showImageCaptions: boolean
}


export const Gallery: React.FC<Props> = ({ seeFullGalleryLink, urlToFetch, showImageCaptions }) => {
	const [viewerIsOpen, setViewerIsOpen] = useState(false)
	const [images, setImages] = useState<ImageProps[]>([])
	const [selectedIndex, setSelectedIndex] = useState<number | undefined>(undefined)

	useEffect(() => {
		// eslint-disable-next-line @typescript-eslint/no-floating-promises
		fetchTheUrl(urlToFetch)
	}, [urlToFetch])

	/* eslint-disable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/
	const fetchTheUrl = async (url: string): Promise<void> => {
		const photos = await fetchGalleryPhotos(url)

		const fetchedImages = photos.map(photo => {
			photo.tags = [{ title: photo.caption, value: photo.caption }]
			if (photos.length < 3) {
				photo.src = photo.original
			}
			return photo
		})
		setImages(fetchedImages)
	}
	/* eslint-enable @typescript-eslint/no-unsafe-member-access, @typescript-eslint/no-unsafe-assignment*/

	const openLightbox = useCallback((_: React.MouseEvent, props: { index:number }) => {
		setSelectedIndex(props.index)
		setViewerIsOpen(true)
	}, [])

	const closeLightbox: () => void = () => {
		setSelectedIndex(undefined)
		setViewerIsOpen(false)
	}

	/* eslint-disable no-shadow, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-member-access */
	const renderImage: React.FC<RenderImageProps> = ({ index, left, top, photo }: RenderImageProps) => (
		<BirdImage
			key={index}
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

	return (
		<GalleryStyled>
			{seeFullGalleryLink ? <LatestPhotosLink /> : null}
			<ReactGallery photos={images} onClick={openLightbox} renderImage={renderImage} />

			{viewerIsOpen ? (
				<Modal onClose={closeLightbox}>
					<Carousel
						// eslint-disable-next-line @typescript-eslint/naming-convention
						components={{ Header: LightboxHeader, Footer: LightboxFooter }}
						currentIndex={selectedIndex}
						views={images.map(x => ({
							caption: x.caption,
							alt: x.caption,
							source: x.original,
							id: x.id,
							key: x.id,
							dateTaken: x.dateTaken,
							birdNames: x.caption,
							birdIds: x.birdIds,
							hostingLink: x.hostingLink,
							text: x.text,
						}))}
					/>
				</Modal>
			) : null}
		</GalleryStyled>
	)
}
