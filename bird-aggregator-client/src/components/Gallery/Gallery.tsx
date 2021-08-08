import * as React from 'react'
import * as axios from '../../http.adapter'

import { BirdImage, Image } from '../BirdImage'
import Carousel, { CommonProps, Modal, ModalGateway, ViewType } from 'react-images'
import { GalleryStyled, HeaderStyled } from './GalleryStyled'
import { useCallback, useEffect, useState } from 'react'

import { BirdLink } from './BirdLink'
import { LatestPhotosLink } from './LatestPhotosLink'
import { default as ReactGallery } from 'react-photo-gallery'
import { RenderImageProps } from 'react-photo-gallery'

interface Props {
	seeFullGalleryLink: boolean
	urlToFetch: string
	showImageCaptions: boolean
}

export const Gallery: React.FC<Props> = props => {
	const { seeFullGalleryLink, urlToFetch, showImageCaptions } = props
	const [viewerIsOpen, setViewerIsOpen] = useState(false)
	const [images, setImages] = useState([ ] as Image[])
	const [selectedIndex, setSelectedIndex] = useState(undefined as number | undefined)

	useEffect(() => {
		fetchTheUrl(urlToFetch)
	}, [urlToFetch])

	const fetchTheUrl = (url: string) => {
		axios.get(url).then(res => {
			const images = res.data.photos.map((x: Image) => {
				x.tags = [{ title: x.caption, value: x.caption }]
				return x
			})
			setImages(images)
		})
	}

	const openLightbox = useCallback((event, { photo, index }) => {
		setSelectedIndex(index)
		setViewerIsOpen(true)
	}, [])

	const closeLightbox = () => {
		setSelectedIndex(undefined)
		setViewerIsOpen(false)
	}

	const renderImage = ({ index, left, top, photo,  }: RenderImageProps<{}>) => (
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
			caption={(photo as any).caption}
		/>
	)

	type ViewTypeExtended = ViewType & {
		birdIds: number[]
		dateTaken: string
		birdNames: string
	}

	const Header = (commonProps: CommonProps) => {
		const view = commonProps.currentView as ViewTypeExtended
		if (!view)
			return null

		return <HeaderStyled>
			<BirdLink
				birdIds={view.birdIds}
				birdNames={view.birdNames}
				dateTaken={view.dateTaken}/>
		</HeaderStyled>
	}
	return (
		<GalleryStyled> 
			{seeFullGalleryLink ? <LatestPhotosLink /> : null}
			<ReactGallery photos={images} onClick={openLightbox} renderImage={renderImage} />
			<ModalGateway>
				{viewerIsOpen ? (
					<Modal onClose={closeLightbox}>
						<Carousel
							components={{Header}}
							currentIndex={selectedIndex}
							views={images.map(x => ({
								caption: x.caption,
								alt: x.caption,
								source: x.original,
								id: x.id,
								dateTaken: x.dateTaken,
								birdNames: x.caption,
								birdIds: x.birdIds
							}))}

						/>
					</Modal>
				) : null}
			</ModalGateway>
		</GalleryStyled>
	)
}