import * as React from 'react'

import { CommonProps, ViewType } from 'react-images'

import { BirdLink } from './BirdLink'
import { HeaderStyled } from './GalleryStyled'

export type ViewTypeExtended = ViewType & {
	birdIds: number[]
	dateTaken: string
	birdNames: string
}

export const LightboxHeader: React.FC<CommonProps> = (commonProps: CommonProps) => {
	const view = commonProps.currentView as ViewTypeExtended
	if (!view) {
		return null
	}

	return (
		<HeaderStyled>
			<BirdLink birdIds={view.birdIds} birdNames={view.birdNames} dateTaken={view.dateTaken} />
		</HeaderStyled>
	)
}

export const LightboxEmptyFooter: React.FC = () => null
