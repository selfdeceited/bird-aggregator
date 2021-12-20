import * as React from 'react'

import { CommonProps, ViewType } from 'react-images'
import { FooterStyled, HeaderStyled } from './GalleryStyled'

import { BirdLink } from './BirdLink'

export type ViewTypeExtended = ViewType & {
	birdIds: number[]
	dateTaken: string
	birdNames: string
	hostingLink: string
	text: string
}

export const LightboxHeader: React.FC<CommonProps> = (commonProps: CommonProps) => {
	const view = commonProps.currentView as ViewTypeExtended
	if (!view) {
		return null
	}

	return (
		<HeaderStyled>
			<BirdLink
				birdIds={view.birdIds}
				birdNames={view.birdNames}
				dateTaken={view.dateTaken}
				hostingLink={view.hostingLink}
			/>
		</HeaderStyled>
	)
}

export const LightboxFooter: React.FC<CommonProps> = (commonProps: CommonProps) => {
	const view = commonProps.currentView as ViewTypeExtended
	if (!view) {
		return null
	}
	return <FooterStyled>
		{view.text}
	</FooterStyled>
}
