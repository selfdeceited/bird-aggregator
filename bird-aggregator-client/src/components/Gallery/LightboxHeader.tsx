import * as React from 'react'

import { FooterStyled, HeaderStyled } from './GalleryStyled'
import { BirdLink } from './BirdLink'


export type HeaderProps = {
	birdIds: number[]
	birdNames: string
	dateTaken: string
	hostingLink: string
}

export const LightboxHeader: React.FC<HeaderProps> = ({ birdIds, birdNames, dateTaken,hostingLink }) => (
	<HeaderStyled>
		<BirdLink
			birdIds={birdIds}
			birdNames={birdNames}
			dateTaken={dateTaken}
			hostingLink={hostingLink}
		/>
	</HeaderStyled>
)

export const LightboxFooter: React.FC<{text:string}> = ({ text }) => <FooterStyled>
	{text}
</FooterStyled>
