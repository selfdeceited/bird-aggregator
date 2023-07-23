/* eslint-disable react/jsx-no-bind */
import * as React from 'react'
import { Gallery as ReactGallery, ThumbnailImageProps } from 'react-grid-gallery'
import { useCallback, useEffect, useState } from 'react'
import { BirdImage } from './BirdImage'
import { GalleryLightbox } from './GalleryLightbox'
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

	const openLightbox = useCallback((index: number, _: ImageProps) => {
		setSelectedIndex(index)
		setViewerIsOpen(true)
	}, [])

	const closeLightbox: () => void = () => {
		setSelectedIndex(undefined)
		setViewerIsOpen(false)
	}

	/* eslint-disable no-shadow, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-member-access */
	const renderImage: React.FC<ThumbnailImageProps> = (thumbnail: ThumbnailImageProps) => (
		<BirdImage
			thumbnail={thumbnail}
			onMouseDown={i => {
				setSelectedIndex(i)
				setViewerIsOpen(true)
			} }/>
	)

	return (
		<GalleryStyled>
			{seeFullGalleryLink ? <LatestPhotosLink /> : null}
			<ReactGallery
				images={images}
				onClick={openLightbox}
				thumbnailImageComponent={renderImage}
				enableImageSelection={false}
				tagStyle={showImageCaptions ? undefined : { display: 'none' }} />

			{viewerIsOpen ? (
				<GalleryLightbox
					isOpened={viewerIsOpen}
					onClose={closeLightbox}
					images={images}
					selectedIndex={selectedIndex} />
			) : null}
		</GalleryStyled>
	)
}
