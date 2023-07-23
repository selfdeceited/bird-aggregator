/* eslint-disable @typescript-eslint/no-unsafe-assignment */

import Lightbox, { SlideImage } from 'yet-another-react-lightbox'
import { CaptionsCustom } from './customCaptionsPlugin'

import { HeaderProps } from './LightboxHeader'
import { ImageProps } from './types'


import React from 'react'

type Props = {
	onClose: () => void
	isOpened: boolean
	selectedIndex: number | undefined
	images: ImageProps[]
}

export type ImagePropsGetter = (image: SlideImage) => {
	header: HeaderProps
	description: string
}

export const GalleryLightbox: React.FC<Props> = ({ onClose, isOpened, images, selectedIndex }) => {
	const getImageProps: ImagePropsGetter = (img: SlideImage) => {
		const selectedImage = images.find(i => i.original === img.src)
		if (!selectedImage) {
			throw new Error()
		}
		return {
			header: {
				birdIds: selectedImage.birdIds,
				birdNames: selectedImage.caption,
				dateTaken: selectedImage.dateTaken,
				hostingLink: selectedImage.hostingLink,
			},
			description: selectedImage.text,
		}
	}
	return (
		<Lightbox
			index={selectedIndex}
			open={isOpened}
			close={onClose}
			plugins={[
				// Captions,
				({ augment, addModule }) => CaptionsCustom({
					augment,
					addModule,
					getImageProps,
				}),
			]}
			// todo: pass not Image[], but Record<OriginalLink, OtherProps> to avoid find() functions
			slides={images.map(x => ({
				src: x.original,
				title: x.caption,
				description: x.text,
			}))} />)
}
